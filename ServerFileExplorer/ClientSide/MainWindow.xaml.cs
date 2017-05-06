using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using ClientSide.Models;

namespace ClientSide
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {
        static HttpClient client = new HttpClient();
        bool isDeletedOrCreated = false;
        static JavaScriptSerializer jss = new JavaScriptSerializer();
        string[] UnorderedResult;
        ListBoxItem lbi;
        ListItemControl lic;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            //-------------------- Adding root folder on start
            lbi = new ListBoxItem();
            lic = new ListItemControl();

            lbi.Content = lic;
            lic.itemImage.Source = new BitmapImage(new Uri("//application:,,,/Resources/folder.png", UriKind.Relative));
            lic.itemText.Text = "root";
            lic.typeOfItem = TypeOfItem.Folder;
            
            MainListBox.Items.Add(lbi);

            //--------------------Sending GET query
            HttpResponseMessage message;
            Uri address = new Uri("http://localhost:50446/Main");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            message = await client.GetAsync(address);
            string responseText = await message.Content.ReadAsStringAsync();

            UnorderedResult = jss.Deserialize<string[]>(responseText);

            foreach (string item in UnorderedResult)
            {
                 lbi = new ListBoxItem();
                 lic = new ListItemControl();
                
                lic.itemText.Text = item.Split('\\').Last();
                lic.Margin = new Thickness(lic.Margin.Left + Deepness(item) * 10, lic.Margin.Top, lic.Margin.Right, lic.Margin.Bottom);
                lic.relativePath = item;
                if (IsFile(item))
                {
                    lic.itemImage.Source = new BitmapImage(new Uri("//application:,,,/Resources/text.png", UriKind.Relative));
                    lic.typeOfItem = TypeOfItem.File;
                }
                else
                {
                    lic.itemImage.Source = new BitmapImage(new Uri("//application:,,,/Resources/folder.png", UriKind.Relative));
                    lic.typeOfItem = TypeOfItem.Folder;
                }
                lbi.Content = lic;
                MainListBox.Items.Add(lbi);
            } 

            //--------------------
        }

        private bool IsFile(string path)
        {
            string fileOrFolderName = path.Split('\\').Last();
            int index = fileOrFolderName.LastIndexOf('.');
            if (index != -1)
            {
                int fileformatLength = fileOrFolderName.Split('.').Last().Length;
                if (fileformatLength == 2 || fileformatLength == 3 || fileformatLength == 4)
                    return true;
            }
            return false;
        }

        private int Deepness(string path)
        {
            return path.Split('\\').Length;
        }



        //NOTE: some button handlers use this
        //private string ClickHelper()
        //{
        //    ListBoxItem item = MainListBox.SelectedItem as ListBoxItem;
        //    ListItemControl currentItem = item.Content as ListItemControl;

        //    string path = (item.Content as ListItemControl).fullPath;

        //    if (currentItem.typeOfItem == TypeOfItem.File)
        //    {
        //        int i;
        //        for (i = path.Length - 1; i > 0; i--)
        //        {
        //            if (path[i] == '\\')
        //                break;
        //        }
        //        path = path.Substring(0, i);
        //    }
        //    return path;
        //}
        //Button handlers
        private  void CreateFile_Click(object sender, RoutedEventArgs e)
        {
            //if (MainListBox.SelectedItem != null)
            //{
            //    string selectedItemPath = ClickHelper();
            //    FileStream fStream =  File.Create(selectedItemPath + '\\' + "text.txt");
            //    fStream.Close();

            //    isDeletedOrCreated = true;
            //    MainListBox.Items.Clear();
            //    DirectoryInfo root = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.GetDirectories("root")[0];
            // //   Explore(root);
            //}
            //else
            //{
            //    MessageBox.Show("Select directory in Browser");
            //}
        }

        private  void CreateFolder_Click(object sender, RoutedEventArgs e)
        {
            //if (MainListBox.SelectedItem != null)
            //{           
            //  //  string selectedItemPath = ClickHelper();

            //    DirectoryInfo dir = new DirectoryInfo(selectedItemPath);

            //    dir.CreateSubdirectory("newDirectory");

            //    isDeletedOrCreated = true;
            //    MainListBox.Items.Clear();
            //    DirectoryInfo root = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.GetDirectories("root")[0];
            //  //  Explore(root);

            //}
            //else
            //{
            //    MessageBox.Show("Select anything in Browser");
            //}

        }

        private  void Update_Click(object sender, RoutedEventArgs e)
        {
            //if (MainListBox.SelectedItem != null)
            //{
            //    ListBoxItem item = MainListBox.SelectedItem as ListBoxItem;
            //    ListItemControl currentItem = item.Content as ListItemControl;
            //  //  string path = (item.Content as ListItemControl).fullPath;

            //    if (currentItem.typeOfItem == TypeOfItem.File)
            //    {
                    
            //       FileStream file = new FileStream(path, FileMode.Open);
            //       StreamWriter writer = new StreamWriter(file);
            //       writer.Write(MainTextBox.Text);
            //       writer.Close();
            //       file.Close();
                    
            //    }
            //    else
            //    {
            //        MessageBox.Show("Select file in Browser");
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Select anything in Browser");
            //}
        }

        private  void Delete_Click(object sender, RoutedEventArgs e)
        {

            //ListBoxItem item = MainListBox.SelectedItem as ListBoxItem;
            //ListItemControl currentItem = item.Content as ListItemControl;
            //string path = (item.Content as ListItemControl).fullPath;

            //if (currentItem.typeOfItem == TypeOfItem.File)
            //{
            //    File.Delete(path);
            //}
            //else
            //{
            //    if (path == Directory.GetParent(Directory.GetCurrentDirectory()).Parent.GetDirectories("root")[0].FullName)
            //    {
            //        MessageBox.Show("You cant delete the root");
            //        return;
            //    }

            //    Directory.Delete(path,true);
            //}

            //isDeletedOrCreated = true;
            //MainListBox.Items.Clear();
            //DirectoryInfo root = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.GetDirectories("root")[0];
          //  Explore(root);

        }

        private async void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isDeletedOrCreated)
            {
                isDeletedOrCreated = false;
                return;
            }
            if (MainListBox.SelectedItem == null)
            {
                return;
            }

            ListBoxItem item = MainListBox.SelectedItem as ListBoxItem;
            ListItemControl currentItem = item.Content as ListItemControl;

            if (currentItem.typeOfItem == TypeOfItem.File)
            {
                HttpResponseMessage message;
                Uri address = new Uri("http://localhost:50446/Main/");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //-------------------- Creating content and passing
                Model model = new Model() { path = currentItem.relativePath };
                var stringContent = new StringContent(jss.Serialize(model), Encoding.UTF8, "application/json");

                message = await client.PostAsync(address, stringContent);
                string responseText = await message.Content.ReadAsStringAsync();

                string contentOfFile = jss.Deserialize<string>(responseText);
                MainTextBox.Text = contentOfFile;
            }
            else
            {
                MainTextBox.Text = string.Empty;
            }
        }
    }
}
