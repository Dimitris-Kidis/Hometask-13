using System;
using System.Security.Principal;
using System.Diagnostics;



// Task 1
//     Sync two dirs, FIRST & SECOND
//     - if a file exist in FIRST but not in SECOND, copy
//     - if a file exists in FIRST and in SECOND, but content
//     changed, then FIRST should overwrite the one from SECOND
//     - if a file exists in SECOND but not in FIRST, it should be removed

// Task 2
//     Sync two dirs, FIRST & SECOND in real time
//     - listen to the changes in FIRST and sync the actions with SECOND 



// ----------------------------------- T A S K 1 ---------------------------------------------


namespace App
{

    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        string sourcePath = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\FIRST";
    //        string destinationPath = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\SECOND";
    //        var source = new DirectoryInfo(sourcePath);
    //        var destination = new DirectoryInfo(destinationPath);

    //        CopyFolderContents(sourcePath, destinationPath, "", true, true);
    //        DeleteAll(source, destination);
    //    }


    //    public static void CopyFolderContents(string sourceFolder, string destinationFolder, string mask, Boolean createFolders, Boolean recurseFolders)
    //    {
    //        try
    //        {
    //            var exDir = sourceFolder;
    //            var dir = new DirectoryInfo(exDir);
    //            var destDir = new DirectoryInfo(destinationFolder);

    //            SearchOption so = (recurseFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

    //            foreach (string sourceFile in Directory.GetFiles(dir.ToString(), mask, so))
    //            {
    //                FileInfo srcFile = new FileInfo(sourceFile);
    //                string srcFileName = srcFile.Name;

    //                // Create a destination that matches the source structure
    //                FileInfo destFile = new FileInfo(destinationFolder + srcFile.FullName.Replace(sourceFolder, ""));

    //                if (!Directory.Exists(destFile.DirectoryName) && createFolders)
    //                {
    //                    Directory.CreateDirectory(destFile.DirectoryName);
    //                }

    //                //Check if src file was modified and modify the destination file
    //                if (srcFile.LastWriteTime > destFile.LastWriteTime || !destFile.Exists)
    //                {
    //                    File.Copy(srcFile.FullName, destFile.FullName, true);
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.WriteLine(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace);
    //        }
    //    }

    //    private static void DeleteAll(DirectoryInfo source, DirectoryInfo target)
    //    {
    //        if (!source.Exists)
    //        {
    //            target.Delete(true);
    //            return;
    //        }

    //        // Delete each existing file in target directory not existing in the source directory.
    //        foreach (FileInfo fi in target.GetFiles())
    //        {
    //            var sourceFile = Path.Combine(source.FullName, fi.Name);
    //            if (!File.Exists(sourceFile)) //Source file doesn't exist, delete target file
    //            {
    //                fi.Delete();
    //            }
    //        }

    //        // Delete non existing files in each subdirectory using recursion.
    //        foreach (DirectoryInfo diTargetSubDir in target.GetDirectories())
    //        {
    //            DirectoryInfo nextSourceSubDir = new DirectoryInfo(Path.Combine(source.FullName, diTargetSubDir.Name));
    //            DeleteAll(nextSourceSubDir, diTargetSubDir);
    //        }
    //    }
    //}


    // ----------------------------------- T A S K 2 ---------------------------------------------





    class Program
    {
        static void Main(string[] args)
        {
            string FIRST = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\FIRST\nnn.txt";
            string SECOND = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\SECOND";


            //File.Copy(FIRST, SECOND + @"\FILE.txt", true);
            string sourcePath = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\FIRST";
            string destinationPath = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\SECOND";

            var source = new DirectoryInfo(sourcePath);
            var destination = new DirectoryInfo(destinationPath);

            using var sourceWatcher = new FileSystemWatcher(sourcePath);

            sourceWatcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;


            sourceWatcher.Changed += OnChanged;
            sourceWatcher.Created += OnCreated;
            sourceWatcher.Deleted += OnDeleted;
            sourceWatcher.Renamed += OnRenamed;
            sourceWatcher.Error += OnError;


            sourceWatcher.IncludeSubdirectories = true;
            sourceWatcher.EnableRaisingEvents = true;
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            string destinationPath = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\SECOND";
            var fullPath = e.FullPath;
            var name = e?.Name;
            string destination = Path.Combine(destinationPath, name);

            File.Copy(fullPath, destination, true);
            File.SetAttributes(destination, FileAttributes.Normal);
            Console.WriteLine($"Changed: {e.FullPath}");
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(e.ChangeType + "ChangeType" + " " + e.Name + "Name" + " " + e.FullPath + "FullPath");
            string value = $"Created: {e.FullPath}";

            var Destination = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\SECOND";
            var name = e.Name;
            var fullPath = e.FullPath;
            string destination = Path.Combine(Destination, name);
            Console.WriteLine(destination);
            Console.WriteLine(e.FullPath + "***");
            Console.WriteLine($"Copy to: {destination}");
            Thread.Sleep(1000);


            File.Copy(fullPath, destination, true);
            File.SetAttributes(destination, FileAttributes.Normal);

            Console.WriteLine(value);
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Deleted: {e.FullPath}");

            var name = e.Name;
            string destinationPath = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\SECOND";
            string destination = Path.Combine(destinationPath, name);
            File.SetAttributes(destination, FileAttributes.Normal);
            File.Delete(destination);
        }

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"Renamed:");
            Console.WriteLine($"    Old: {e.OldFullPath}");
            Console.WriteLine($"    New: {e.FullPath}");

            var oldFileName = Path.GetFileName(e.OldFullPath);
            var newFileName = Path.GetFileName(e.FullPath);
            var Destination = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\SECOND";
            var oldDestinationPath = Path.Combine(Destination, oldFileName);
            var newDestinationPath = Path.Combine(Destination, newFileName);
            var info = new FileInfo(oldDestinationPath);
            info.MoveTo(newDestinationPath);
        }

        private static void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private static void PrintException(Exception? ex)
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


    }








}
