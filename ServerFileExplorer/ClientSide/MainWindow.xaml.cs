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

        Uri address = new Uri("http://localhost:50446/Main/");
        static JavaScriptSerializer jss = new JavaScriptSerializer();
        string[] UnorderedResult;
        ListBoxItem lbi;
        ListItemControl lic;
        HttpResponseMessage message;
        public MainWindow()
        {
            InitializeComponent();
        }

        protected  override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            AddRoot();
            //-------------------- Configuring Httpclient
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Explore();
        }

        private void AddRoot()
        {
            //-------------------- Adding root folder on start
            lbi = new ListBoxItem();
            lic = new ListItemControl();

            lbi.Content = lic;
            lic.itemImage.Source = new BitmapImage(new Uri("//application:,,,/Resources/folder.png", UriKind.Relative));
            lic.itemText.Text = "root";
            lic.typeOfItem = Enums.Folder;
            MainListBox.Items.Add(lbi);
        }

        private async void Explore()
        {
            MainListBox.Items.Clear();
            AddRoot();
            //--------------------Sending GET query,retrieving All data from server

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
                    lic.typeOfItem = Enums.File;
                }
                else
                {
                    lic.itemImage.Source = new BitmapImage(new Uri("//application:,,,/Resources/folder.png", UriKind.Relative));
                    lic.typeOfItem = Enums.Folder;
                }
                lbi.Content = lic;
                MainListBox.Items.Add(lbi);
            }
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

        // -------------- Button handlers

        private async void CreateFile_Click(object sender, RoutedEventArgs e)
        {
            if (MainListBox.SelectedItem != null)
            {
                string FileName = string.Empty;

                CreateFileWindow createFileWindow = new CreateFileWindow(s => { FileName = s; });
                createFileWindow.ShowDialog();

                ListBoxItem item = MainListBox.SelectedItem as ListBoxItem;
                ListItemControl currentItem = item.Content as ListItemControl;

                
                CreateDeleteUpdateModel model = new CreateDeleteUpdateModel()
                { path = currentItem.relativePath,
                    typeOfItemSelected = currentItem.typeOfItem,
                    typeOfItemToChange = Enums.File,
                    newItemName = FileName,
                    action = ActionToDo.Create
                };

                var stringContent = new StringContent(jss.Serialize(model), Encoding.UTF8, "application/json");
                await client.PutAsync(address, stringContent);

                Explore();
            }
            else
            {
                MessageBox.Show("Select anything in Browser");
            }
        }

        private async void CreateFolder_Click(object sender, RoutedEventArgs e)
        {
            if (MainListBox.SelectedItem != null)
            {
                string FolderName = string.Empty;

                CreateFolderWindow createFileWindow = new CreateFolderWindow(s => { FolderName = s; });
                createFileWindow.ShowDialog();

                ListBoxItem item = MainListBox.SelectedItem as ListBoxItem;
                ListItemControl currentItem = item.Content as ListItemControl;

               
                CreateDeleteUpdateModel model = new CreateDeleteUpdateModel()
                {
                    path = currentItem.relativePath,
                    typeOfItemSelected = currentItem.typeOfItem,
                    typeOfItemToChange = Enums.Folder,
                    newItemName = FolderName,
                    action = ActionToDo.Create
                };

                var stringContent = new StringContent(jss.Serialize(model), Encoding.UTF8, "application/json");
                await client.PutAsync(address, stringContent);

                Explore();
            }
            else
            {
                MessageBox.Show("Select anything in Browser");
            }
        }

        private async void Update_Click(object sender, RoutedEventArgs e)
        {
            if (MainListBox.SelectedItem != null)
            {
                ListBoxItem item = MainListBox.SelectedItem as ListBoxItem;
                ListItemControl currentItem = item.Content as ListItemControl;

                if (currentItem.typeOfItem == Enums.File)
                {
                    string FileName = string.Empty;
                   
                    CreateDeleteUpdateModel model = new CreateDeleteUpdateModel()
                    {
                        path = currentItem.relativePath,
                        contentToWrite = MainTextBox.Text,
                        action = ActionToDo.Update
                    };

                    var stringContent = new StringContent(jss.Serialize(model), Encoding.UTF8, "application/json");
                    await client.PutAsync(address, stringContent);

                    Explore();
                }
                else
                {
                    MessageBox.Show("Select file in Browser");
                }
            }
            else
            {
                MessageBox.Show("Select anything in Browser");
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MainListBox.SelectedItem != null)
            {
                ListBoxItem item = MainListBox.SelectedItem as ListBoxItem;
                ListItemControl currentItem = item.Content as ListItemControl;
                string path = (item.Content as ListItemControl).relativePath;

                if (path == Directory.GetParent(Directory.GetCurrentDirectory()).Parent.GetDirectories("root")[0].FullName)
                {
                    MessageBox.Show("You cant delete the root");
                    return;
                }

                CreateDeleteUpdateModel model = new CreateDeleteUpdateModel()
                {
                    path = currentItem.relativePath,
                    typeOfItemSelected = currentItem.typeOfItem,
                    action = ActionToDo.Delete
                };

                var stringContent = new StringContent(jss.Serialize(model), Encoding.UTF8, "application/json");
                await client.PutAsync(address, stringContent);

                Explore();
            }
            else
            {
                MessageBox.Show("Select anything in Browser");
            }

        }

        private async void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainListBox.SelectedItem == null)
            {
                return;
            }

            ListBoxItem item = MainListBox.SelectedItem as ListBoxItem;
            ListItemControl currentItem = item.Content as ListItemControl;

            if (currentItem.typeOfItem == Enums.File)
            {

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
