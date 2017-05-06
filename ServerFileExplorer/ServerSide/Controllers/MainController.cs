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
        bool isFirstEnter = true;
        List<string> content = new List<string>();
        // GET: api/Main
        public string[] Get()
        {
            if (isFirstEnter)
            {
                // string s = Directory.GetCurrentDirectory();
                DirectoryInfo projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent;
                DirectoryInfo[] projectDirectorysFolders = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.GetDirectories();
                bool contains = false;
                foreach (DirectoryInfo item in projectDirectorysFolders)
                {
                    if (item.Name == "root")
                        contains = true;
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



        public string Post(Model model)
        {
            string FileContent;
            DirectoryInfo projectDirectorysFolders = Directory.GetParent(Directory.GetCurrentDirectory())
                .Parent.GetDirectories("root").First<DirectoryInfo>();

            string pathOfFile = projectDirectorysFolders.FullName + '\\' + model.path;

            FileStream file = new FileStream(pathOfFile, FileMode.Open);
            StreamReader reader = new StreamReader(file);
            FileContent = reader.ReadToEnd();
            reader.Close();
            file.Close();

            return FileContent;
        }

        


        // PUT: api/Main/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Main/5
        public void Delete(int id)
        {
        }

        //------------------------------- retrieve all data
        private string[] Explore(DirectoryInfo root)
        {
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
            }

            for (int i = 0; i < dirs.Length; i++)
            {
                ExploreHelper(dirs[i], deepness + 1);
            }
            return content.ToArray();
        }

    }
}
