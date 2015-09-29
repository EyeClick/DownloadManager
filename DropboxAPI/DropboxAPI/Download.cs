using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DropBoxClient;
using System.IO;
using DropBoxClient.Entities;
using DropNet;
namespace DownloadManager
{
    class Dropbox
    {
        public Dropbox(string Source, string DestinationDirectory, string Tokken, string API, string Username, string Password, bool IsConsole)
        {
            OAuth2Client client = new DropBoxClient.OAuth2Client(Tokken);
            DropNet.DropNetClient DownloadClient = new DropNet.DropNetClient(Username, Password, Tokken);
            DownloadFolder(client, DownloadClient, Source, DestinationDirectory, API, IsConsole);
        }

        public void DownloadFolder(OAuth2Client client, DropNet.DropNetClient DownloadClient, string Dir, string DestinationDirectory, string API, bool IsConsole)
        {

            if (Directory.Exists(DestinationDirectory) == false)
            {
                if (IsConsole)
                {
                    Console.WriteLine("Creating folder:" + DestinationDirectory);
                }
                Directory.CreateDirectory(DestinationDirectory);
            }

            var Data = client.GetFolderMeta(API, Dir);

            foreach (var item in Data.Contents)
            {
                if (item.IsDirectory)
                {
                    DownloadFolder(client, DownloadClient, item.Path, DestinationDirectory + @"\" + item.Path.Split('/')[item.Path.Split('/').Count() - 1], API, IsConsole);

                }
                else
                {
                    if (IsConsole)
                    {
                        Console.WriteLine("Downloading:" + Path.GetFileName(item.Path));
                    }

                    var NewFile = DownloadClient.GetFile(item.Path);

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
