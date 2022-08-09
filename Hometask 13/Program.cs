using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;




//namespace App
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            string sourcePath = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\FIRST";
//            string destinationPath = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\SECOND";


//            FileManager fm = new(sourcePath, destinationPath);

//            fm.DirectorySynchronization();
//        }

//    }


//    class FileManager
//    {
//        public string RootDirectory { get; set; }

//        public string AdditionalDirectory { get; set; }

//        private Dictionary<string, List<string>> PathDictionary { get; set; }

//        private DirectoryInfo directoryInfo;

//        public FileManager(string rootDirectory, string additionalDirectory)
//        {
//            RootDirectory = rootDirectory;
//            AdditionalDirectory = additionalDirectory;
//            directoryInfo = new DirectoryInfo(rootDirectory);
//        }

//        public void DirectorySynchronization()
//        {
//            PathDictionary = WalkDirectoryTree(directoryInfo, new Dictionary<string, List<string>>()); 


//            ClearDirectory(); 

//            foreach (var item in PathDictionary) 
//            {
//                if (Directory.Exists(item.Key.Replace(RootDirectory, AdditionalDirectory)))
//                {
//                    if (item.Value.Count > 0)
//                    {
//                        foreach (var file in item.Value)
//                        {
//                            File.Copy($@"{item.Key}\{file}",
//                            $@"{item.Key.Replace(RootDirectory, AdditionalDirectory)}\{file}");
//                        }
//                    }
//                }
//                else
//                {
//                    Directory.CreateDirectory($"{item.Key.Replace(RootDirectory, AdditionalDirectory)}");

//                    if (item.Value.Count > 0)
//                    {
//                        foreach (var file in item.Value)
//                        {
//                            File.Copy($@"{item.Key}\{file}",
//                            $@"{item.Key.Replace(RootDirectory, AdditionalDirectory)}\{file}");
//                        }
//                    }
//                }
//            }
//            Console.WriteLine("Directories synchronized");
//        }
//        private void ClearDirectory()
//        {
//            if (Directory.Exists(AdditionalDirectory))
//            {
//                Directory.Delete(AdditionalDirectory, true);
//                Directory.CreateDirectory(AdditionalDirectory);
//            }
//            else Directory.CreateDirectory(AdditionalDirectory);
//        }

//        private Dictionary<string, List<string>> WalkDirectoryTree(DirectoryInfo root, Dictionary<string, List<string>> filesInfoDic)
//        {

//            FileInfo[] files = null;
//            DirectoryInfo[] subDirs = null;

//            try
//            {
//                files = root.GetFiles("*.*");
//                subDirs = root.GetDirectories();
//            }
//            catch (UnauthorizedAccessException e)
//            {
//                Console.WriteLine(e.Message);
//            }

//            catch (DirectoryNotFoundException e)
//            {
//                Console.WriteLine(e.Message);
//            }

//            if (files != null)
//            {
//                foreach (FileInfo file in files)
//                {
//                    if (filesInfoDic.ContainsKey(root.FullName))
//                    {
//                        filesInfoDic[root.FullName].Add(file.Name);
//                    }
//                    else
//                    {
//                        filesInfoDic[root.FullName] = new List<string>();
//                        filesInfoDic[root.FullName].Add(file.Name);
//                    }
//                }

//                foreach (DirectoryInfo dirInfo in subDirs)
//                {
//                    filesInfoDic[dirInfo.FullName] = new List<string>();

//                    filesInfoDic = WalkDirectoryTree(dirInfo, filesInfoDic);
//                }
//            }

//            return filesInfoDic;
//        }
//    }

//}

// ------------------------------------------------------------------------------------









namespace App
{
    class Program
    {
        static DirectoryWatcher directoryWatcher = new DirectoryWatcher();
        static DirectoryInfo directoryInfo1 = new DirectoryInfo(@"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\FIRST");
        static DirectoryInfo directoryInfo2 = new DirectoryInfo(@"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\SECOND");

        public static void Main(string[] args)
        {
            Console.WriteLine("Initializing watching...");
            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
            try
            {
                fileSystemWatcher.Path = @"C:\Users\dmitrii.romanenco\Downloads\Hometask 13\FIRST";
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }


            fileSystemWatcher.Changed += OnChanged;
            fileSystemWatcher.Created += OnChanged;
            fileSystemWatcher.Renamed += OnChanged;
            fileSystemWatcher.Deleted += OnDeleted;

            fileSystemWatcher.IncludeSubdirectories = true;
            fileSystemWatcher.EnableRaisingEvents = true;

            Console.ReadLine();
        }

        private static async void OnDeleted(object sender, FileSystemEventArgs eventArgs)
        {
            directoryWatcher.Remove(directoryInfo1, directoryInfo2);
            await directoryWatcher.CheckFolders(directoryInfo1, directoryInfo2);
        }

        private static void OnChanged(object source, FileSystemEventArgs eventArgs)
        {
            directoryWatcher.SynchronizeFolders(directoryInfo1, directoryInfo2);
            directoryWatcher.Remove(directoryInfo1, directoryInfo2);
        }

    }
}
















namespace App
{
    class DirectoryWatcher
    {
        public async Task CheckFolders(DirectoryInfo basePath, DirectoryInfo destPath)
        {
            await Task.WhenAll(basePath.GetFiles().Select(async file =>
            {
                if (File.Exists(Path.Combine(destPath.FullName, file.Name)) &&
                    await CreateHashSha1(Path.Combine(destPath.FullName, file.Name)) ==
                    await CreateHashSha1(Path.Combine(basePath.FullName, file.Name)))
                {
                    return;
                }
                else
                {
                    SynchronizeFolders(basePath, destPath);
                    Remove(basePath, destPath);
                }
            }));

        }

        public void SynchronizeFolders(DirectoryInfo basePath, DirectoryInfo destPath)
        {
            foreach (var file in basePath.GetFiles())
            {
                if (File.Exists(Path.Combine(destPath.FullName, file.Name)))
                {
                    try
                    {
                        if (CreateHashSha1(Path.Combine(destPath.FullName, file.Name)) != CreateHashSha1(Path.Combine(basePath.FullName, file.Name)))
                        {
                            file.CopyTo(Path.Combine(destPath.FullName, file.Name), true);
                            Console.WriteLine($"File {file.FullName} was synchronized with file {destPath.FullName}\\{file.Name}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        file.CopyTo(Path.Combine(destPath.FullName, file.Name), true);
                        Console.WriteLine($"File {file.FullName} was created in {destPath.FullName}\\{file.Name}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            try
            {
                if (basePath.GetDirectories().Length > 0)
                {
                    foreach (var nextSubDirectory in basePath.GetDirectories())
                    {
                        var destNextSubDirectory = destPath.CreateSubdirectory(nextSubDirectory.Name);
                        Remove(nextSubDirectory, destNextSubDirectory);
                        SynchronizeFolders(nextSubDirectory, destNextSubDirectory);
                    }
                }
            }
            catch (StackOverflowException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Remove(DirectoryInfo basePath, DirectoryInfo destPath)
        {
            try
            {
                foreach (var file in destPath.GetFiles())
                {
                    if (File.Exists(Path.Combine(basePath.FullName, file.Name)) == false)
                    {
                        File.Delete(file.FullName);
                        Console.WriteLine($"File {file.FullName} was deleted!");
                    }
                }

                foreach (var directory in destPath.GetDirectories())
                {
                    if (Directory.Exists(Path.Combine(basePath.FullName, directory.Name)) == false)
                    {
                        Directory.Delete(directory.FullName, true);
                        Console.WriteLine($"Directory {directory.FullName} was deleted!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static async Task<string> CreateHashSha1(string path)
        {
            var sha = SHA256.Create();
            using var stream = File.OpenRead(path);
            var hash = await sha.ComputeHashAsync(stream);
            return BitConverter.ToString(hash);
        }
    }
}






