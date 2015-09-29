using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DownloadManager
{

    class DropNetTest
    {
        public DropNetTest(string Source, string DestinationDirectory, string Tokken, string API, bool IsConsole)
        {
            DropNet.DropNetClient client = new DropNet.DropNetClient("zm69gt2beapaz3h", "a9x7omgjht2pca8", "Ax8xn9_W4x0AAAAAAAJr2xpPcj_IX-mwd4jGtPEux7GnJ9-5h8m7yHTX_JcfW7XO");

            DownloadFolder(client, Source, DestinationDirectory, API, IsConsole);
        }

        public void DownloadFolder(DropNet.DropNetClient client, string Dir, string DestinationDirectory, string API, bool IsConsole)
        {

            if (Directory.Exists(DestinationDirectory) == false)
            {
                if (IsConsole)
                {
                    Console.WriteLine("Creating folder:" + DestinationDirectory);
                }
                Directory.CreateDirectory(DestinationDirectory);
            }

            //var Data = client.GetFolderMeta(API, Dir);
            var Data = client.GetMetaData(Dir,false);

            foreach (var item in Data.Contents)
            {
                if (item.Is_Dir)
                {
                    DownloadFolder(client, item.Path, DestinationDirectory + @"\" + item.Path.Split('/')[item.Path.Split('/').Count() - 1], API, IsConsole);

                }
                else
                {
                    if (IsConsole)
                    {
                        Console.WriteLine("Downloading:" + Path.GetFileName(item.Path));
                    }

                    Byte[] NewFile = client.GetFile(item.Path);


                    using (var fileStream = new FileStream(
                    DestinationDirectory + @"\" + Path.GetFileName(item.Path), FileMode.CreateNew, FileAccess.ReadWrite))
                    {
                        fileStream.Write(NewFile, 0, NewFile.Length);
                        fileStream.Flush(true);

                    }


                }
            }
        }
    }
}
