﻿using System;


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Hometask_13
//{
//    internal class Program
//    {
//        static void Main(string[] args)
//        {
//            string sourcePath = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\FIRST";
//            string destinationPath = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\SECOND";
//            var source = new DirectoryInfo(sourcePath);
//            var destination = new DirectoryInfo(destinationPath);

//            CopyFolderContents(sourcePath, destinationPath, "", true, true);
//            DeleteAll(source, destination);
//        }


//        public static void CopyFolderContents(string sourceFolder, string destinationFolder, string mask, Boolean createFolders, Boolean recurseFolders)
//        {
//            try
//            {
//                var exDir = sourceFolder;
//                var dir = new DirectoryInfo(exDir);
//                var destDir = new DirectoryInfo(destinationFolder);

//                SearchOption so = (recurseFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

//                foreach (string sourceFile in Directory.GetFiles(dir.ToString(), mask, so))
//                {
//                    FileInfo srcFile = new FileInfo(sourceFile);
//                    string srcFileName = srcFile.Name;

//                    // Create a destination that matches the source structure
//                    FileInfo destFile = new FileInfo(destinationFolder + srcFile.FullName.Replace(sourceFolder, ""));

//                    if (!Directory.Exists(destFile.DirectoryName) && createFolders)
//                    {
//                        Directory.CreateDirectory(destFile.DirectoryName);
//                    }

//                    //Check if src file was modified and modify the destination file
//                    if (srcFile.LastWriteTime > destFile.LastWriteTime || !destFile.Exists)
//                    {
//                        File.Copy(srcFile.FullName, destFile.FullName, true);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                //Debug.WriteLine(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace);
//            }
//        }

//        private static void DeleteAll(DirectoryInfo source, DirectoryInfo target)
//        {
//            if (!source.Exists)
//            {
//                target.Delete(true);
//                return;
//            }

//            // Delete each existing file in target directory not existing in the source directory.
//            foreach (FileInfo fi in target.GetFiles())
//            {
//                var sourceFile = Path.Combine(source.FullName, fi.Name);
//                if (!File.Exists(sourceFile)) //Source file doesn't exist, delete target file
//                {
//                    fi.Delete();
//                }
//            }

//            // Delete non existing files in each subdirectory using recursion.
//            foreach (DirectoryInfo diTargetSubDir in target.GetDirectories())
//            {
//                DirectoryInfo nextSourceSubDir = new DirectoryInfo(Path.Combine(source.FullName, diTargetSubDir.Name));
//                DeleteAll(nextSourceSubDir, diTargetSubDir);
//            }
//        }
//    }

//}


namespace RealTimeApp
{
    class Program
    {
        private static void Main(string[] args)
        {
            string DirMain = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\FIRST";

            string DirAdditional = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\SECOND";

            var source = new DirectoryInfo(DirMain);
            var destination = new DirectoryInfo(DirAdditional);

            CopyFolderContents(DirMain, DirAdditional, "", true, true);
            DeleteAll(source, destination);

            RealTimeFileManager realTimeFileManager = new RealTimeFileManager(DirMain, DirAdditional);

            using var watcher = new FileSystemWatcher(realTimeFileManager.RootDirectory);


            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

            watcher.Changed += realTimeFileManager.OnChanged;
            watcher.Created += realTimeFileManager.OnCreated;
            watcher.Deleted += realTimeFileManager.OnDeleted;
            watcher.Renamed += realTimeFileManager.OnRenamed;
            watcher.Error += realTimeFileManager.OnError;

            //watcher.Filter = "*.txt";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }

        public static void CopyFolderContents(string sourceFolder, string destinationFolder, string mask, Boolean createFolders, Boolean recurseFolders)
        {
            try
            {
                var exDir = sourceFolder;
                var dir = new DirectoryInfo(exDir);
                var destDir = new DirectoryInfo(destinationFolder);

                SearchOption so = (recurseFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

                foreach (string sourceFile in Directory.GetFiles(dir.ToString(), mask, so))
                {
                    FileInfo srcFile = new FileInfo(sourceFile);
                    string srcFileName = srcFile.Name;

                    // Create a destination that matches the source structure
                    FileInfo destFile = new FileInfo(destinationFolder + srcFile.FullName.Replace(sourceFolder, ""));

                    if (!Directory.Exists(destFile.DirectoryName) && createFolders)
                    {
                        Directory.CreateDirectory(destFile.DirectoryName);
                    }

                    //Check if src file was modified and modify the destination file
                    if (srcFile.LastWriteTime > destFile.LastWriteTime || !destFile.Exists)
                    {
                        File.Copy(srcFile.FullName, destFile.FullName, true);
                    }
                }
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace);
            }
        }

        private static void DeleteAll(DirectoryInfo source, DirectoryInfo target)
        {
            if (!source.Exists)
            {
                target.Delete(true);
                return;
            }

            // Delete each existing file in target directory not existing in the source directory.
            foreach (FileInfo fi in target.GetFiles())
            {
                var sourceFile = Path.Combine(source.FullName, fi.Name);
                if (!File.Exists(sourceFile)) //Source file doesn't exist, delete target file
                {
                    fi.Delete();
                }
            }

            // Delete non existing files in each subdirectory using recursion.
            foreach (DirectoryInfo diTargetSubDir in target.GetDirectories())
            {
                DirectoryInfo nextSourceSubDir = new DirectoryInfo(Path.Combine(source.FullName, diTargetSubDir.Name));
                DeleteAll(nextSourceSubDir, diTargetSubDir);
            }
        }
    }

    internal class RealTimeFileManager
    {
        public string RootDirectory { get; set; }

        public string AdditionalDirectory { get; set; }

        public RealTimeFileManager(string rootDirectory, string additionalDirectory)
        {
            RootDirectory = rootDirectory;
            AdditionalDirectory = additionalDirectory;
        }

        public void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            if (IsFile(e.FullPath))
            {
                File.Copy(e.FullPath, $@"{AdditionalDirectory}\{e.Name}", true);
                Console.WriteLine($@"Changed File: {e.FullPath} -> {AdditionalDirectory}\{e.Name}");
            }
        }

        public void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (IsFile(e.FullPath))
            {
                if (!File.Exists($@"{AdditionalDirectory}\{e.Name}"))
                {
                    using (FileStream fileStream = File.Create($@"{AdditionalDirectory}\{e.Name}")) ;
                    Console.WriteLine($@"Created File: {AdditionalDirectory}\{e.Name}");
                }
            }
            else
            {
                if (!Directory.Exists($@"{AdditionalDirectory}\{e.Name}"))
                {
                    Directory.CreateDirectory($@"{AdditionalDirectory}\{e.Name}");
                    Console.WriteLine($@"Created Directory: {AdditionalDirectory}\{e.Name}");
                }
            }
        }

        public void OnDeleted(object sender, FileSystemEventArgs e)
        {
            if (IsFile(e.FullPath))
            {
                if (File.Exists($@"{AdditionalDirectory}\{e.Name}"))
                {
                    File.Delete($@"{AdditionalDirectory}\{e.Name}");
                    Console.WriteLine($@"Deleted File: {e.FullPath} -> {AdditionalDirectory}\{e.Name}");
                }
                else Console.WriteLine($"Deleted: {e.FullPath}");
            }
            else
            {
                if (Directory.Exists($@"{AdditionalDirectory}\{e.Name}"))
                {
                    Directory.Delete($@"{AdditionalDirectory}\{e.Name}", true);
                    Console.WriteLine($@"Deleted Directory: {e.FullPath} -> {AdditionalDirectory}\{e.Name}");
                }
                else Console.WriteLine($"Deleted: {e.FullPath}");
            }
        }

        public void OnRenamed(object sender, RenamedEventArgs e)
        {
            if (IsFile(e.FullPath))
            {
                if (!File.Exists($@"{AdditionalDirectory}\{e.Name}"))
                {
                    File.Move($@"{AdditionalDirectory}\{e.OldName}", $@"{AdditionalDirectory}\{e.Name}");
                    Console.WriteLine($@"Renamed File: {AdditionalDirectory}\{e.OldName} -> {AdditionalDirectory}\{e.Name}");
                }
                else Console.WriteLine($"This name exists !!! the default name was set");
            }
            else
            {
                if (!Directory.Exists($@"{AdditionalDirectory}\{e.Name}"))
                {
                    Directory.Move($@"{AdditionalDirectory}\{e.OldName}", $@"{AdditionalDirectory}\{e.Name}");
                    Console.WriteLine($@"Reanamed Directory: {AdditionalDirectory}\{e.OldName} -> {AdditionalDirectory}\{e.Name}");
                }
                else Console.WriteLine($"This name exists !!! the default name was set");
            }
        }

        public void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        public void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }

        public bool IsFile (string path)
        {
            try
            {
                FileAttributes attr = File.GetAttributes(path);

                //detect whether its a directory or file
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {

                return false;
            }
            
        }


        
    }
}
