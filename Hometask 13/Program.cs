//using System;
using System.Security.Principal;
using System.Diagnostics;



//// Task 1
////     Sync two dirs, FIRST & SECOND
////     - if a file exist in FIRST but not in SECOND, copy
////     - if a file exists in FIRST and in SECOND, but content
////     changed, then FIRST should overwrite the one from SECOND
////     - if a file exists in SECOND but not in FIRST, it should be removed

//// Task 2
////     Sync two dirs, FIRST & SECOND in real time
////     - listen to the changes in FIRST and sync the actions with SECOND 



//// ----------------------------------- T A S K 1 ---------------------------------------------


//namespace App
//{

//    //class Program
//    //{
//    //    static void Main(string[] args)
//    //    {
//    //        string sourcePath = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\FIRST";
//    //        string destinationPath = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\SECOND";
//    //        var source = new DirectoryInfo(sourcePath);
//    //        var destination = new DirectoryInfo(destinationPath);

//    //        CopyFolderContents(sourcePath, destinationPath, "", true, true);
//    //        DeleteAll(source, destination);
//    //    }


//    //    public static void CopyFolderContents(string sourceFolder, string destinationFolder, string mask, Boolean createFolders, Boolean recurseFolders)
//    //    {
//    //        try
//    //        {
//    //            var exDir = sourceFolder;
//    //            var dir = new DirectoryInfo(exDir);
//    //            var destDir = new DirectoryInfo(destinationFolder);

//    //            SearchOption so = (recurseFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

//    //            foreach (string sourceFile in Directory.GetFiles(dir.ToString(), mask, so))
//    //            {
//    //                FileInfo srcFile = new FileInfo(sourceFile);
//    //                string srcFileName = srcFile.Name;

//    //                // Create a destination that matches the source structure
//    //                FileInfo destFile = new FileInfo(destinationFolder + srcFile.FullName.Replace(sourceFolder, ""));

//    //                if (!Directory.Exists(destFile.DirectoryName) && createFolders)
//    //                {
//    //                    Directory.CreateDirectory(destFile.DirectoryName);
//    //                }

//    //                //Check if src file was modified and modify the destination file
//    //                if (srcFile.LastWriteTime > destFile.LastWriteTime || !destFile.Exists)
//    //                {
//    //                    File.Copy(srcFile.FullName, destFile.FullName, true);
//    //                }
//    //            }
//    //        }
//    //        catch (Exception ex)
//    //        {
//    //            Debug.WriteLine(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace);
//    //        }
//    //    }

//    //    private static void DeleteAll(DirectoryInfo source, DirectoryInfo target)
//    //    {
//    //        if (!source.Exists)
//    //        {
//    //            target.Delete(true);
//    //            return;
//    //        }

//    //        // Delete each existing file in target directory not existing in the source directory.
//    //        foreach (FileInfo fi in target.GetFiles())
//    //        {
//    //            var sourceFile = Path.Combine(source.FullName, fi.Name);
//    //            if (!File.Exists(sourceFile)) //Source file doesn't exist, delete target file
//    //            {
//    //                fi.Delete();
//    //            }
//    //        }

//    //        // Delete non existing files in each subdirectory using recursion.
//    //        foreach (DirectoryInfo diTargetSubDir in target.GetDirectories())
//    //        {
//    //            DirectoryInfo nextSourceSubDir = new DirectoryInfo(Path.Combine(source.FullName, diTargetSubDir.Name));
//    //            DeleteAll(nextSourceSubDir, diTargetSubDir);
//    //        }
//    //    }
//    //}


//    // ----------------------------------- T A S K 2 ---------------------------------------------



using System;
//namespace App
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {


//            //File.Copy(FIRST, SECOND + @"\FILE.txt", true);
//            string sourcePath = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\FIRST\fdfe.txt";
//            string destinationPath = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\SECOND";



//FileAttributes attr = File.GetAttributes(sourcePath);

////detect whether its a directory or file
//if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
//    Console.WriteLine("Its a directory");
//else
//    Console.WriteLine("Its a file");
//            Console.WriteLine("---------------");

//            var source = new DirectoryInfo(sourcePath);
//            var destination = new DirectoryInfo(destinationPath);

//            CopyFolderContents(sourcePath, destinationPath, "", true, true);
//            DeleteAll(source, destination);

//            using var sourceWatcher = new FileSystemWatcher(sourcePath);

//            sourceWatcher.NotifyFilter = NotifyFilters.Attributes
//                                 | NotifyFilters.CreationTime
//                                 | NotifyFilters.DirectoryName
//                                 | NotifyFilters.FileName
//                                 | NotifyFilters.LastAccess
//                                 | NotifyFilters.LastWrite
//                                 | NotifyFilters.Security
//                                 | NotifyFilters.Size;


//            sourceWatcher.Changed += OnChanged;
//            sourceWatcher.Created += OnCreated;
//            sourceWatcher.Deleted += OnDeleted;
//            sourceWatcher.Renamed += OnRenamed;
//            sourceWatcher.Error += OnError;


//            sourceWatcher.IncludeSubdirectories = true;
//            sourceWatcher.EnableRaisingEvents = true;
//            Console.WriteLine("Press enter to exit.");
//            Console.ReadLine();
//        }

//        private static void OnChanged(object sender, FileSystemEventArgs e)
//        {
//            if (e.ChangeType != WatcherChangeTypes.Changed)
//            {
//                return;
//            }
//            string destinationPath = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\SECOND";
//            var fullPath = e.FullPath;
//            var name = e?.Name;
//            string destination = Path.Combine(destinationPath, name);
//            var Destination = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\SECOND";

//            Console.WriteLine("e.Name " + e.Name);
//            if (Directory.Exists(e.FullPath))
//            { // dir
//                Directory.CreateDirectory(e.FullPath.Replace("FIRST", "SECOND"));
//            }
//            else
//            { // file

//                try
//                {
//                    File.Copy(fullPath, destination, true);
//                }
//                catch (Exception)
//                {
//                    Directory.CreateDirectory(e.FullPath.Replace("FIRST", "SECOND"));
//                }
//            }


//            Console.WriteLine($"Changed: {e.FullPath}");
//        }

//        private static void OnCreated(object sender, FileSystemEventArgs e)
//        {
//            Console.WriteLine(e.ChangeType + "ChangeType" + " " + e.Name + "Name" + " " + e.FullPath + "FullPath");
//            string value = $"Created: {e.FullPath}";

//            var Destination = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\SECOND";
//            var name = e.Name;
//            var fullPath = e.FullPath;
//            string destination = Path.Combine(Destination, name);
//            Console.WriteLine(destination);
//            Console.WriteLine(e.FullPath + "***");
//            Console.WriteLine($"Copy to: {destination}");
//            Thread.Sleep(1000);

//            Console.WriteLine("FILEEE" + "  " + Directory.Exists(e.FullPath)); // True when dir


//            if (Directory.Exists(e.FullPath))
//            { // dir
//                Directory.CreateDirectory(e.FullPath.Replace("FIRST", "SECOND"));
//            }
//            else
//            { // file
//                try
//                {
//                    File.Copy(fullPath, destination, true);
//                }
//                catch (Exception)
//                {
//                    Directory.CreateDirectory(e.FullPath.Replace("FIRST", "SECOND"));
//                }

//            }


//            //try
//            //{

//            //}
//            //catch (Exception)
//            //{
//            //    var Source = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\FIRST";
//            //    var source = new DirectoryInfo(Source);
//            //    var destinationI = new DirectoryInfo(Destination);
//            //    //File.Copy(fullPath, destination, true);
//            //    //File.SetAttributes(destination, FileAttributes.Normal);
//            //    //CopyFolderContents(Source, Destination, "", true, true);
//            //    //DeleteAll(source, destinationI);
//            //    if (!Directory.Exists(destination)) Directory.CreateDirectory(destination);


//            //}


//            Console.WriteLine(value);
//        }

//        private static void OnDeleted(object sender, FileSystemEventArgs e)
//        {
//            Console.WriteLine($"Deleted: {e.FullPath}");

//            var name = e.Name;
//            string destinationPath = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\SECOND";
//            string destination = Path.Combine(destinationPath, name);
//            try
//            {
//                File.Delete(destination);
//            }
//            catch (Exception)
//            {
//                if (!Directory.Exists(destination)) Directory.Delete(destination);

//            }

//            if (Directory.Exists(e.FullPath))
//            { // dir
//                Directory.Delete(e.FullPath.Replace("FIRST", "SECOND"), true);
//            }
//            else
//            { // file

//                try
//                {
//                    Directory.Delete(e.FullPath.Replace("FIRST", "SECOND"), true);
//                }
//                catch (Exception)
//                {
//                    //Directory.Delete(e.FullPath, true);
//                    Console.WriteLine(e.FullPath + " FUUUULL");
//                    File.Delete(e.FullPath.Replace("FIRST", "SECOND"));
//                }
//            }

//        }

//        private static void OnRenamed(object sender, RenamedEventArgs e)
//        {
//            Console.WriteLine($"Renamed:");
//            Console.WriteLine($"    Old: {e.OldFullPath}");
//            Console.WriteLine($"    New: {e.FullPath}");

//            var oldFileName = Path.GetFileName(e.OldFullPath);
//            var newFileName = Path.GetFileName(e.FullPath);
//            var Destination = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\SECOND";
//            var oldDestinationPath = Path.Combine(Destination, oldFileName);
//            var newDestinationPath = Path.Combine(Destination, newFileName);
//            var info = new FileInfo(oldDestinationPath);
//            Console.WriteLine("-------------------");
//            Console.WriteLine("REEENAME" + " " + e.FullPath + " old" + " " + e.OldName);
//            Console.WriteLine(oldDestinationPath + " new" + newDestinationPath);
//            if (Directory.Exists(e.FullPath))
//            { // dir
//                Directory.Move(oldDestinationPath, newDestinationPath);
//            }
//            else
//            { // file

//                try
//                {
//                    info.MoveTo(e.FullPath.Replace("FIRST", "SECOND"));
//                }
//                catch (Exception)
//                {
//                    Directory.Move(oldDestinationPath, newDestinationPath);

//                }
//            }

//        }

//        private static void OnError(object sender, ErrorEventArgs e) =>
//            PrintException(e.GetException());

//        private static void PrintException(Exception? ex)
//        {
//            if (ex != null)
//            {
//                Console.WriteLine($"Message: {ex.Message}");
//                Console.WriteLine("Stacktrace:");
//                Console.WriteLine(ex.StackTrace);
//                Console.WriteLine();
//                PrintException(ex.InnerException);
//            }
//        }











//public static void CopyFolderContents(string sourceFolder, string destinationFolder, string mask, Boolean createFolders, Boolean recurseFolders)
//{
//    try
//    {
//        var exDir = sourceFolder;
//        var dir = new DirectoryInfo(exDir);
//        var destDir = new DirectoryInfo(destinationFolder);

//        SearchOption so = (recurseFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

//        foreach (string sourceFile in Directory.GetFiles(dir.ToString(), mask, so))
//        {
//            FileInfo srcFile = new FileInfo(sourceFile);
//            string srcFileName = srcFile.Name;

//            // Create a destination that matches the source structure
//            FileInfo destFile = new FileInfo(destinationFolder + srcFile.FullName.Replace(sourceFolder, ""));

//            if (!Directory.Exists(destFile.DirectoryName) && createFolders)
//            {
//                Directory.CreateDirectory(destFile.DirectoryName);
//            }

//            //Check if src file was modified and modify the destination file
//            if (srcFile.LastWriteTime > destFile.LastWriteTime || !destFile.Exists)
//            {
//                File.Copy(srcFile.FullName, destFile.FullName, true);
//            }
//        }
//    }
//    catch (Exception ex)
//    {
//        //Debug.WriteLine(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace);
//    }
//}

//private static void DeleteAll(DirectoryInfo source, DirectoryInfo target)
//{
//    if (!source.Exists)
//    {
//        target.Delete(true);
//        return;
//    }

//    // Delete each existing file in target directory not existing in the source directory.
//    foreach (FileInfo fi in target.GetFiles())
//    {
//        var sourceFile = Path.Combine(source.FullName, fi.Name);
//        if (!File.Exists(sourceFile)) //Source file doesn't exist, delete target file
//        {
//            fi.Delete();
//        }
//    }

//    // Delete non existing files in each subdirectory using recursion.
//    foreach (DirectoryInfo diTargetSubDir in target.GetDirectories())
//    {
//        DirectoryInfo nextSourceSubDir = new DirectoryInfo(Path.Combine(source.FullName, diTargetSubDir.Name));
//        DeleteAll(nextSourceSubDir, diTargetSubDir);
//    }
//}

//    }
//}





namespace RealTimeApp
{
    class MyClassCS
    {
        static void Main()
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
