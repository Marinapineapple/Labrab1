using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Labrab1
{
    class FileV
    {
        const string pathStart = "E:\\Start.txt";
        public string Name { get; set; }

        public string Size { get; set; }

        public string Create { get; set; }

        public string Modify { get; set; }

        public string Note { get; set; }

        public string Directory { get; set; }

        public static string PathToActiveDirectory { get; set; } // Путь к активному каталогу
        public static  List<FileV> Catalog = new List<FileV>();
        public static List<FileV> file = new List<FileV>();//список, где хранятся проинициализированный
        public static List<FileV> fileList = new List<FileV>(); // Временный список.

        public void Write(FileV a)
        {
            
             FileStream fl = new FileStream(pathStart, FileMode.Append);
             StreamWriter writer = new StreamWriter(fl);

             writer.WriteLine("{0}", a.Name);
             writer.WriteLine("{0}", a.Size);
             writer.WriteLine("{0}", a.Create);
             writer.WriteLine("{0}", a.Modify);
             writer.WriteLine("{0}", a.Note);
             writer.WriteLine("{0}", a.Directory);
            
            writer.Close();
        }
        
        public void Direct()
        {
            DirectoryInfo di = new DirectoryInfo(FileV.PathToActiveDirectory);

            foreach (FileInfo f in di.GetFiles("*.*"))
            {
                FileV a = new FileV();
               
                a.Name = f.Name;
                a.Size = Convert.ToString(f.Length);
                a.Create = Convert.ToString(f.CreationTime);
                a.Modify = Convert.ToString(f.LastAccessTime);
                a.Note = "0";
                a.Directory = Convert.ToString(f.DirectoryName);

                Write(a);

                // file.Close();
            }
        }

        public void InitCatalog()
        {
            // Очистка списка.
            Catalog.Clear();

            for (int i = 0; i < FileV.file.Count; i++)
            {
                int flag = 1;
                 
                FileV p = new FileV();

                p.Directory = FileV.file[i].Directory;

                for (int j = 0; j < FileV.Catalog.Count; j++)
                {
                    if (p.Directory == Catalog[j].Directory)
                    {
                        flag = 0;
                        break;
                    }
                }
                
                if (flag == 1)//если не найдена директория, то добавляется записи в спиоск
                    Catalog.Add(p);
            }
        }

        public void InitFile()
        {
               file.Clear();// Очистка списка.

            // Обход файла для записи в список проинициализированных файлов.
            FileStream File = new FileStream(pathStart, FileMode.Open);
            StreamReader Reader = new StreamReader(File);

            while (!Reader.EndOfStream) // Начальное состояние.
            {
                FileV p = new FileV();

                // Достаём строки и вынимаем характеристики
                string Name = Reader.ReadLine();
                string Size = Reader.ReadLine();
                string Created = Reader.ReadLine();
                string Modified = Reader.ReadLine();
                string Noted = Reader.ReadLine();
                string Directory = Reader.ReadLine();

                p.Name = Name;
                p.Size = Size;
                p.Create = Created;
                p.Modify = Modified;
                p.Note = Noted;
                p.Directory = Directory;

                file.Add(p);// Заполнение списка.
            }
            Reader.Close();
        }

        public void ApplyFile()
        {
            int flagWriteNew = 1;
            
            // Очистка файла.
            FileStream p = new FileStream(pathStart, FileMode.Create);
            StreamWriter Writer = new StreamWriter(p);
            Writer.Close();

            // Очистка меток временного списка для АКТИВНОЙ директории
            for (int i = 0; i < FileV.fileList.Count; i++)
            {
                if (FileV.fileList[i].Directory == FileV.PathToActiveDirectory)
                {
                    if (FileV.fileList[i].Note != "remove")
                        FileV.fileList[i].Note = "0";
                    else
                        FileV.fileList[i].Note = "new";
                }
            }

            // Берем список "file".
            // Записываем НОВУЮ информацию о ТЕКУЩЕМ каталоге.
            // Записываем СТАРУЮ информацию об остальных ПРОИНИЦИАЛИЗИРОВАННЫХ каталогах из списка "file".
            DirectoryInfo d = new DirectoryInfo(FileV.PathToActiveDirectory);

            for (int i = 0; i < FileV.file.Count; i++)
            {

                FileV Old = new FileV();

                // Узнаем информацию о файле.
                Old.Name = FileV.file[i].Name;
                Old.Size = FileV.file[i].Size;
                Old.Create = FileV.file[i].Create;
                Old.Modify = FileV.file[i].Modify;
                Old.Note = FileV.file[i].Note;
                Old.Directory = FileV.file[i].Directory;

                // Записываем НОВУЮ информацию о ТЕКУЩЕМ каталоге.
                if (flagWriteNew == 1)
                {
                    foreach (FileInfo f in d.GetFiles())
                    {
                        FileV New = new FileV();

                        // Узнаем НОВУЮ информацию о файле в текущей директории.
                        New.Name = f.Name;
                        New.Size = Convert.ToString(f.Length);
                        New.Create = Convert.ToString(f.CreationTime);
                        New.Modify = Convert.ToString(f.LastWriteTime);
                        New.Note = Old.Note;
                        New.Note = "0";
                        New.Directory = Convert.ToString(f.DirectoryName);

                        // Если директории совпадают, перезаписываем НОВУЮ информацию.
                        if (Old.Directory == FileV.PathToActiveDirectory)
                        {
                           // Запись в файл НОВОЙ инфы о директории.
                                Write(New);

                                flagWriteNew = 0;
                            
                        }
                        else
                            break;
                    }
                }
                // Записываем СТАРУЮ информацию об остальных ПРОИНИЦИАЛИЗИРОВАННЫХ каталогах.
                if (Old.Directory != FileV.PathToActiveDirectory)
                    Write(Old);
            }
                // Очистка списка.
                file.Clear();
            
        }
    }
}
