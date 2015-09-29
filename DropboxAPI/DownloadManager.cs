using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;


namespace DownloadManager
{
    class DownloadManager
    {
        public static bool IsConsole = false;

        static void Main(string[] args)
        {
            // Only one process in a time
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Count() > 1)
            {
                return;
            }

            try
            {
                Directory.Delete(@"c:\tempdownload\");
            }
            catch
            {
            }

            Console.ForegroundColor = ConsoleColor.Green;

            string Token = args[0];
            string API = args[1];
            string Username = args[2];
            string Password = args[3];

            for (int i = 4; i < args.Length; i += 4)
            {
                string ExeSource = string.Empty;
                string ExeDestination = string.Empty;
                string MediaSource = string.Empty;
                string MediaDestination = string.Empty;

                if (args.Length > i)
                    ExeSource = args[i];
                if (args.Length > i + 1)
                    ExeDestination = args[i + 1];
                if (args.Length > i + 2)
                    MediaSource = args[i + 2];
                if (args.Length > i + 3)
                    MediaDestination = args[i + 3];

                try
                {
                    bool exists = System.IO.Directory.Exists(@"c:\tempdownload\Exe\");

                    if (!exists)
                        System.IO.Directory.CreateDirectory(@"c:\tempdownload\Exe\");

                    Dropbox newDropboxDownloadExe = new Dropbox(ExeSource, @"c:\tempdownload\Exe\", Token, API, Username, Password, IsConsole);

                    MoveDirectory(@"c:\tempdownload\Exe\", ExeDestination);
                }
                catch
                {
                }


                if (MediaSource != string.Empty)
                {
                    try
                    {
                        bool exists = System.IO.Directory.Exists(@"c:\tempdownload\Media\");

                        if (!exists)
                            System.IO.Directory.CreateDirectory(@"c:\tempdownload\Media\");
                        Dropbox newDropboxDownloadMedia = new Dropbox(MediaSource, @"c:\tempdownload\Media\", Token, API, Username, Password, IsConsole);

                        MoveDirectory(@"c:\tempdownload\Media\", MediaDestination);
                    }
                    catch
                    {
                    }
                }

            }
            try
            {
                Directory.Delete(@"c:\tempdownload\",true);
            }
            catch
            {
            }

            if (IsConsole)
            {
                Console.WriteLine("Done");
                Console.ReadKey();
            }

        }



        public static void MoveDirectory(string source, string target)
        {
            var stack = new Stack<Folders>();
            stack.Push(new Folders(source, target));

            while (stack.Count > 0)
            {
                var folders = stack.Pop();
                Directory.CreateDirectory(folders.Target);
                foreach (var file in Directory.GetFiles(folders.Source, "*.*"))
                {
                    string targetFile = Path.Combine(folders.Target, Path.GetFileName(file));
                    if (File.Exists(targetFile)) File.Delete(targetFile);
                    File.Move(file, targetFile);
                }

                foreach (var folder in Directory.GetDirectories(folders.Source))
                {
                    stack.Push(new Folders(folder, Path.Combine(folders.Target, Path.GetFileName(folder))));
                }
            }

        }
        public class Folders
        {
            public string Source { get; private set; }
            public string Target { get; private set; }

            public Folders(string source, string target)
            {
                Source = source;
                Target = target;
            }
        }

    }




}

//Example for usage
//string Source = @"Applications/Floor/sharks";
//string Destination = @"C:\Installation-Wizard\applications\floor\game_exe\sharks";