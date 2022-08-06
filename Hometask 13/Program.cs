using System;
using System.Security.Principal;



// Task 1
//     Sync two dirs, FIRST & SECOND
//     - if a file exist in FIRST but not in SECOND, copy
//     - if a file exists in FIRST and in SECOND, but content
//     changed, then FIRST should overwrite the one from SECOND
//     - if a file exists in SECOND but not in FIRST, it should be removed

// Task 2
//     Sync two dirs, FIRST & SECOND in real time
//     - listen to the changes in FIRST and sync the actions with SECOND 

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            string FIRST = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\FIRST";
            string SECOND = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\SECOND";

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
            Console.WriteLine($"Changed: {e.FullPath}");
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";

            var Destination = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\SECOND";
            var name = e.Name;
            var fullPath = e.FullPath;
            string destination = Path.Combine(Destination, name);
            Console.WriteLine(e.FullPath + "***");
            Console.WriteLine($"Copy to: {destination}");
            Thread.Sleep(1000);
            

            File.Copy(fullPath, destination, true);

            Console.WriteLine(value);
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Deleted: {e.FullPath}");

            var name = e.Name;
            string destinationPath = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\SECOND";
            string destination = Path.Combine(destinationPath, name);
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