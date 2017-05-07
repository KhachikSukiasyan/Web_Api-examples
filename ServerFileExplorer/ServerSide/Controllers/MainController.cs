using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using ServerSide.Models;

namespace ServerSide.Controllers
{
    public class MainController : ApiController
    {
        static bool isFirstEnter = true;
        static List<string> content = new List<string>();
        
        public string[] Get()
        {
            if (isFirstEnter)
            {
                DirectoryInfo projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent;
                DirectoryInfo[] projectDirectorysFolders = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.GetDirectories();
                bool contains = false;
                foreach (DirectoryInfo item in projectDirectorysFolders)
                {
                    if (item.Name == "root")
                    {
                        contains = true;
                        break;
                    }
                }
                if (!contains)
                {
                    projectDirectory.CreateSubdirectory("root");
                }
               
                isFirstEnter = false;
            }

            DirectoryInfo root = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.GetDirectories("root")[0];
            return Explore(root);
        }


        //----------------- Read file content
        public string Post(Model model)
        {
            string FileContent;
            DirectoryInfo projectDirectorysFolders = Directory.GetParent(Directory.GetCurrentDirectory())
                .Parent.GetDirectories("root").First();

            string pathOfFile = projectDirectorysFolders.FullName + '\\' + model.path;

            FileStream file = new FileStream(pathOfFile, FileMode.Open);
            StreamReader reader = new StreamReader(file);
            FileContent = reader.ReadToEnd();
            reader.Close();
            file.Close();

            return FileContent;
        }

        
        public void Put(CreateDeleteUpdateModel model)
        {

         DirectoryInfo root = Directory.GetParent(Directory.GetCurrentDirectory())
                .Parent.GetDirectories("root").First();

            string pathToCreateFile;
            string pathToCreateFolder;

            switch (model.action)
            {
                // -------------------------------- CREATE
                case ActionToDo.Create:
                    if (model.typeOfItemToChange == TypeOfItem.File)
                    {
                        if (model.typeOfItemSelected == TypeOfItem.File)
                        {
                            string pathOfSelectedFile = root.FullName + '\\' + model.path;
                            FileInfo file = new FileInfo(pathOfSelectedFile);
                            DirectoryInfo containingFolder = file.Directory;

                            pathToCreateFile = containingFolder.FullName + '\\' + model.newItemName + ".txt";
                        }
                        else
                        {
                            pathToCreateFile = root.FullName + '\\' + model.path + '\\' + model.newItemName + ".txt";
                        }
                        FileInfo fi = new FileInfo(pathToCreateFile);
                        FileStream fs = fi.Create();
                        fs.Close();
                    }
                    else
                    {
                        if (model.typeOfItemSelected == TypeOfItem.File)
                        {
                            string pathOfSelectedFile = root.FullName + '\\' + model.path;
                            FileInfo file = new FileInfo(pathOfSelectedFile);
                            DirectoryInfo containingFolder = file.Directory;

                            pathToCreateFolder = containingFolder.FullName;
                        }
                        else
                        {
                            pathToCreateFolder = root.FullName + '\\' + model.path;
                        }

                        DirectoryInfo dir = new DirectoryInfo(pathToCreateFolder);
                        dir.CreateSubdirectory(model.newItemName);
                    }

                    break;
                // -------------------------------- UPDATE
                case ActionToDo.Update:
                    {
                        string pathOfSelectedFile = root.FullName + '\\' + model.path;

                        FileStream filestream = new FileStream(pathOfSelectedFile, FileMode.Open);
                        StreamWriter writer = new StreamWriter(filestream);
                        writer.Write(model.contentToWrite);
                        writer.Close();
                        filestream.Close();
                    }
                    break;
                // -------------------------------- DELETE
                case ActionToDo.Delete:
                    {
                        string selectedItemPath = root.FullName + '\\' + model.path;
                        if (model.typeOfItemSelected == TypeOfItem.Folder)
                            Directory.Delete(selectedItemPath, true);
                        else
                            File.Delete(selectedItemPath);
                    }
                    break;
                default:
                    break;
            }
        }

        //------------------------------- Retrieve all data
        private string[] Explore(DirectoryInfo root)
        {
            content.Clear();
            return ExploreHelper(root, 0);
        }

        private string[] ExploreHelper(DirectoryInfo root, int deepness)
        {
            
            FileInfo[] files = (from g in root.GetFiles() where g.Extension == ".txt" select g).ToArray();
            for (int i = 0; i < files.Length; i++)
            {
                string currentFilePath = files[i].FullName;
                string[] spltiiedPath = currentFilePath.Split(new string[] { "\\root\\" },StringSplitOptions.None);
                string currentFileRelativePath = string.Empty;

                for (int j = 1; j < spltiiedPath.Length; j++)
                {
                    currentFileRelativePath += spltiiedPath[j];
                }
                content.Add(currentFileRelativePath);
            }

            DirectoryInfo[] dirs = root.GetDirectories();
            for (int i = 0; i < dirs.Length; i++)
            {
                string currentFolderPath = dirs[i].FullName;
                string[] spltiiedPath = currentFolderPath.Split(new string[] { "\\root\\" }, StringSplitOptions.None);
                string currentFolderRelativePath = string.Empty;

                for (int j = 1; j < spltiiedPath.Length; j++)
                {
                    currentFolderRelativePath += spltiiedPath[j];
                }

                content.Add(currentFolderRelativePath);

                ExploreHelper(dirs[i], deepness + 1);
            }
            return content.ToArray();
        }

    }
}
