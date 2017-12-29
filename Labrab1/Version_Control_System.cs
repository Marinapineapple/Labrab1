using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Labrab1
{
    class Version_Control_System
    {
        const string pathStart = "E:\\Start.txt";
        public void New()
        {
            FileV a = new FileV();
            DirectoryInfo di = new DirectoryInfo(FileV.PathToActiveDirectory);
            //Directory da = new Directory(pathCatalog);
            //Сharacteristics a = da.a;

            foreach (FileInfo f in di.GetFiles("*.*"))
            {

                int FlagFileNew = 1;
                int FlagRemove = 0;

                a.Name = f.Name;
                a.Size = Convert.ToString(f.Length);
                a.Create = Convert.ToString(f.CreationTime);
                a.Modify = Convert.ToString(f.LastAccessTime);

                for (int i = 0; i < FileV.fileList.Count; i++)
                {
                    if ((FileV.fileList[i].Name == a.Name)
                        && (FileV.fileList[i].Note != "0")
                        && (FileV.fileList[i].Directory == FileV.PathToActiveDirectory))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;

                        if (FileV.fileList[i].Note == "remove")
                            Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("file {0}<<-- {1}d/ed\n", a.Name, FileV.fileList[i].Note);
                        Console.Write("size {0}\n", a.Size);
                        Console.Write("create {0}\n", a.Create);
                        Console.Write("modify {0}\n\n", a.Modify);
                        Console.ResetColor();

                        FlagRemove = 1;
                        break;
                    }
                }
                if (FlagRemove == 1)
                {
                    continue;
                }
                for (int i = 0; i < FileV.file.Count; i++)
                {
                    if (FileV.file[i].Directory == FileV.PathToActiveDirectory)
                    {
                        if (a.Name == FileV.file[i].Name) // Сравнение имён файлов на текущий момент в Файле.
                        {
                            if (a.Size != FileV.file[i].Size)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;

                                Console.Write("file {0}\n", a.Name);
                                Console.Write("size {0} b <<-- {1} b\n", FileV.file[i].Size, a.Size);
                                Console.Write("create {0}\n", a.Create);
                                Console.Write("modify {0}\n\n", a.Modify);

                                Console.ResetColor();
                            }
                            else if (a.Modify != FileV.file[i].Modify && a.Size != FileV.file[i].Size)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;

                                Console.Write("file {0}\n", a.Name);
                                Console.Write("size {0}<<-- {1} b\n", a.Size, FileV.file[i].Size);
                                Console.Write("create {0}\n", a.Create);
                                Console.Write("modify {0} <<-- {1}\n\n", FileV.file[i].Modify, a.Modify);

                                Console.ResetColor();
                            }

                            else if (a.Size == FileV.file[i].Size && a.Create == FileV.file[i].Create)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write("file {0}\n", FileV.file[i].Name);
                                Console.Write("size {0} b\n", FileV.file[i].Size);
                                Console.Write("create {0}\n", FileV.file[i].Create);
                                Console.Write("modify {0}\n\n", FileV.file[i].Modify);

                                Console.ResetColor();
                            }

                            FlagFileNew = 0;
                            break;
                        }
                    }
                }
                if (FlagFileNew == 1)
                {
                    int FlagAdd = 1;
                    for (int i = 0; i < FileV.fileList.Count; i++)
                    {
                        if ((FileV.fileList[i].Name == a.Name) && (FileV.fileList[i].Note == "add") && (FileV.fileList[i].Directory == FileV.PathToActiveDirectory))
                        {
                            FlagAdd = 0;
                            break;
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.Green;

                    if (FlagAdd == 0)
                    {
                        Console.Write("file {0} <<--add\n", a.Name);
                        Console.Write("size {0} b\n", a.Size);
                        Console.Write("create {0}\n", a.Create);
                        Console.Write("modify {0}\n\n", a.Modify);
                    }
                    else
                    {
                        Console.Write("file {0} <<--new\n", a.Name);
                        Console.Write("size {0} b\n", a.Size);
                        Console.Write("create {0}\n", a.Create);
                        Console.Write("modify {0}\n\n", a.Modify);
                    }
                    Console.ResetColor();
                }
            }
        }

        public void Delete()
        {
            FileV a = new FileV();
            DirectoryInfo di = new DirectoryInfo(FileV.PathToActiveDirectory);


            for (int i = 0; i < FileV.file.Count; i++)
            {
                int FlagDelete = 1;

                if (FileV.file[i].Directory == FileV.PathToActiveDirectory)
                {
                    foreach (FileInfo f in di.GetFiles("*.*"))
                    {
                        a.Name = f.Name;
                        a.Size = Convert.ToString(f.Length);
                        a.Create = Convert.ToString(f.CreationTime);
                        a.Modify = Convert.ToString(f.LastAccessTime);

                        if (FileV.file[i].Name == a.Name)
                        {
                            FlagDelete = 0;
                            break;
                        }
                    }

                    if (FlagDelete == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;

                        Console.Write("file {0} <<--delete\n", FileV.file[i].Name);
                        Console.Write("size {0} b\n", FileV.file[i].Size);
                        Console.Write("create {0}\n", FileV.file[i].Create);
                        Console.Write("modify {0}\n\n", FileV.file[i].Modify);

                        Console.ResetColor();
                    }
                }
            }
        }

        public void IdentifyComand(string comand)
        {
            String[] Com = comand.Split(new[] { ' ' }, 2); // Разделяем строку на подкоманды.

            if (Com[0] == "init")
            {
                if (Com.Length == 1)
                {
                    Console.WriteLine("Не введён путь к каталогу.\n");
                }

                else if (Com.Length == 2)
                {
                    if (!Directory.Exists(Com[1]))
                    {
                        Console.WriteLine("Каталог не существует.\n");
                    }
                    else
                    {
                        int flagWrite = 1;
                        // Запоминаем путь к активному каталогу.
                        FileV.PathToActiveDirectory = Com[1];

                        // Проверка директорий на инициализацию.
                        for (int i = 0; i < FileV.Catalog.Count; i++)
                        {
                            if (FileV.file[i].Directory == FileV.PathToActiveDirectory)
                                flagWrite = 0;
                        }
                        if (flagWrite == 1)
                        {
                            FileV p = new FileV();
                            init();
                            Console.WriteLine("Каталог: {0} проинициализирован.", FileV.PathToActiveDirectory);
                        }
                        else
                            Console.WriteLine("Каталог: {0} уже был проинициализирован.", FileV.PathToActiveDirectory);
                    }
                }
            }
            else if (Com[0] == "status")
            {
                if (Com.Length == 1)
                {
                    if (FileV.PathToActiveDirectory == null)
                    {
                        Console.WriteLine("Ни один из каталогов не проинициализирован!\n");
                    }
                    else
                    {
                        New();
                        Delete();
                    }
                }
                else
                {
                    Console.WriteLine("Неверный формат команды!\n");
                }
            }
            else if (Com[0] == "add")
            {
                if (Com.Length == 1)
                {
                    Console.WriteLine("Не введёно имя файла.\n");
                }

                else if (Com.Length == 2)
                {
                    if (!File.Exists(FileV.PathToActiveDirectory + "\\" + Com[1]))
                    {
                        Console.WriteLine("Файл не существует.\n");
                    }
                    else
                    {
                        add(Com[1]);
                    }
                }
            }

            else if (Com[0] == "remove")
            {
                if (Com.Length == 1)
                {
                    Console.WriteLine("Не введёно имя файла.\n");
                }

                else if (Com.Length == 2)
                {
                    remove(Com[1]);
                }
            }

            else if (Com[0] == "apply")
            {
                if (Com.Length == 1)
                {
                    if (FileV.PathToActiveDirectory == null)
                    {
                        Console.WriteLine("Ни один из каталогов не проинициализирован!\n");
                    }
                    else
                    {
                        apply();
                        Console.WriteLine("Все изменения сохранены в каталоге {0}!\n", FileV.PathToActiveDirectory);
                    }
                }
                else if (Com.Length == 2)
                {
                    if (!Directory.Exists(Com[1]))
                    {
                        Console.WriteLine("Ошибка. Каталог не существует.\n");
                    }
                    else
                    {
                        // Запоминаем путь к активному каталогу.
                        FileV.PathToActiveDirectory = Com[1];

                        apply();
                        Console.WriteLine("Все изменения сохранены в каталоге {0}!\n", FileV.PathToActiveDirectory);

                    }
                }
            }
            else if (Com[0] == "listbranch")
            {

                if (Com.Length == 1)
                {
                    if (FileV.PathToActiveDirectory == null)
                    {
                        Console.WriteLine("Ни один каталог не проинициализирован!\n");
                    }
                    else
                        listbranch();
                }
                else
                    Console.WriteLine("Неверный формат команды!\n");
            }

            else if (Com[0] == "checkout")
            {
                if (Com.Length == 1)
                {
                    Console.WriteLine("Не введён путь к каталогу или номер каталога.\n");
                }
                else if (Com.Length == 2)
                {
                    checkout(Com[1]);
                }
            }

            else if (Com[0] == "exit")
            {
                Console.WriteLine("ПОКА");
                Console.Read();
                Environment.Exit(0);
            }

        }

        public void init()
        {
            FileV p = new FileV();

            // Запись информации в файл о новой директории.
            p.Direct();

            // Заполнение списков НОВЫМИ данными.
            p.InitFile();
            p.InitCatalog();
        }

        public void apply()
        {
            FileV p = new FileV();

            // Запись НОВОЙ информации в файл о выбранной директории.
            p.ApplyFile();

            // Заполнение списков НОВЫМИ данными.
            p.InitFile();
            p.InitCatalog();
        }

        private void add(string file_name)
        {
            int flagAdd = 0; // 0 - не удалось найти, 1 - поиск удачно завершился.

            for (int i = 0; i < FileV.fileList.Count; i++)
            {
                if ((FileV.fileList[i].Name == file_name) && (FileV.fileList[i].Directory == FileV.PathToActiveDirectory))
                {
                    FileV.fileList[i].Note = "add";
                    Console.WriteLine("Файл добавлен под версионный контроль!\n", file_name);
                    flagAdd = 1;
                    break;
                }
            }

            //Если не нашли среди проинициализированных, то добавляем во временный список с меткой "add". 
            if (flagAdd == 0)
            {
                DirectoryInfo d = new DirectoryInfo(FileV.PathToActiveDirectory);
                foreach (FileInfo f in d.GetFiles(file_name))
                {
                    FileV p = new FileV();

                    // Узнаем информацию о файле.
                    p.Name = f.Name;
                    p.Size = Convert.ToString(f.Length);
                    p.Create = Convert.ToString(f.CreationTime);
                    p.Modify = Convert.ToString(f.LastWriteTime);
                    p.Note = "add";
                    p.Directory = Convert.ToString(f.DirectoryName);

                    FileV.fileList.Add(p);
                    Console.WriteLine("Файл добавлен под версионный контроль!\n", p.Name);

                    // Добавляем во временный список, который будет хранить метку add.
                }

            }
        }

        private void remove(string file_name)
        {
            int flagRemove = 0;

            for (int i = 0; i < FileV.fileList.Count; i++)
            {
                if ((FileV.fileList[i].Name == file_name) && (FileV.fileList[i].Directory == FileV.PathToActiveDirectory))
                {
                    FileV.fileList[i].Note = "remove";
                    Console.WriteLine("Файл убран из-под версионного контроля!\n", file_name);
                    flagRemove = 1;
                    break;
                }
            }

            if (flagRemove == 0)
            {
                DirectoryInfo d = new DirectoryInfo(FileV.PathToActiveDirectory);
                foreach (FileInfo f in d.GetFiles(file_name))
                {
                    FileV p = new FileV();

                    // Узнаем информацию о файле.
                    p.Name = f.Name;
                    p.Size = Convert.ToString(f.Length);
                    p.Create = Convert.ToString(f.CreationTime);
                    p.Modify = Convert.ToString(f.LastWriteTime);
                    p.Note = "remove";
                    p.Directory = Convert.ToString(f.DirectoryName);

                    Console.WriteLine("Файл убран из-под версионного контроля!\n", p.Name);

                    // Добавляем во временный список, который будет хранить метку remove.
                    FileV.fileList.Add(p);

                }
            }
        }

        public void listbranch()
        {
            FileV p = new FileV();

            p.InitCatalog();//последний каталог проинициализирован будет

            Console.WriteLine("Список проинициализированных каталогов:");
            // Вывод проинициализированных каталогов
            for (int i = 0; i < FileV.Catalog.Count; i++)
            {
                Console.WriteLine("{0}", FileV.Catalog[i].Directory);
            }
            Console.WriteLine("");
        }

        public void checkout(string dir_path)
        {
            int flagFound = 0;

            for (int i = 0; i < FileV.Catalog.Count; i++)
            {
                if (FileV.Catalog[i].Directory == @dir_path)
                {
                    FileV.PathToActiveDirectory = @dir_path;
                    flagFound = 1;
                    break;
                }
            }
            if (flagFound == 1)
                Console.WriteLine("checkout <--Установлен активный каталог: {0}!\n", FileV.PathToActiveDirectory);
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("checkout <--Ошибка. Каталог: {0} не найден среди проинициализированных!\n", dir_path);
                Console.ResetColor();
            }
        }
    }
}
