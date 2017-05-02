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

namespace ClientSide
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {
        bool isDeletedOrCreated = false;

        public MainWindow()
        {
            InitializeComponent();
        }
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            DirectoryInfo root = Directory.GetParent( Directory.GetCurrentDirectory()).Parent.GetDirectories("root")[0];
          //  string wanted_path = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));

            Explore(root);

        }

        private void Explore(DirectoryInfo root)
        {
            ExploreHelper(root, 0);
        }

        private void ExploreHelper(DirectoryInfo root,int marginMultiplyer)
        {
            ListBoxItem lbi = new ListBoxItem();
            ListItemControl lic = new ListItemControl();

            lbi.Content = lic;
            lic.itemImage.Source = new BitmapImage(new Uri("//application:,,,/Resources/folder.png", UriKind.Relative));
            lic.itemText.Text = root.Name;
            lic.typeOfItem = TypeOfItem.Folder;
            lic.fullPath = root.FullName;

            lic.Margin = new Thickness(lic.Margin.Left + marginMultiplyer * 10, lic.Margin.Top, lic.Margin.Right, lic.Margin.Bottom);
            MainListBox.Items.Add(lbi);

           
            FileInfo[] files = (from g in root.GetFiles() where g.Extension == ".txt" select g).ToArray();

            for (int i = 0; i < files.Length; i++)
            {
                lbi = new ListBoxItem();
                lic = new ListItemControl();
                lbi.Content = lic;

                lic.Margin = new Thickness(lic.Margin.Left + (marginMultiplyer + 1) * 10, lic.Margin.Top, lic.Margin.Right, lic.Margin.Bottom);
                lic.itemImage.Source = new BitmapImage(new Uri("//application:,,,/Resources/text.png", UriKind.Relative));
                lic.itemText.Text = files[i].Name;
                lic.typeOfItem = TypeOfItem.File;
                lic.fullPath = files[i].FullName;

                MainListBox.Items.Add(lbi);
            }


            DirectoryInfo[] dirs = root.GetDirectories();
            for (int i = 0; i < dirs.Length; i++)
            {
                ExploreHelper(dirs[i], marginMultiplyer + 1);
            }
        }

        //NOTE: some button handlers use this
        private string ClickHelper()
        {
            ListBoxItem item = MainListBox.SelectedItem as ListBoxItem;
            ListItemControl currentItem = item.Content as ListItemControl;

            string path = (item.Content as ListItemControl).fullPath;

            if (currentItem.typeOfItem == TypeOfItem.File)
            {
                int i;
                for (i = path.Length - 1; i > 0; i--)
                {
                    if (path[i] == '\\')
                        break;
                }
                path = path.Substring(0, i);
            }
            return path;
        }
        //Button handlers
        private void CreateFile_Click(object sender, RoutedEventArgs e)
        {
            if (MainListBox.SelectedItem != null)
            {
               string selectedItemPath = ClickHelper();

               File.Create(selectedItemPath + '\\' + "text.txt");

               isDeletedOrCreated = true;
               MainListBox.Items.Clear();
               DirectoryInfo root = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.GetDirectories("root")[0];
               Explore(root);
            }
            else
            {
                MessageBox.Show("Select directory in Browser");
            }
        }

        private void CreateFolder_Click(object sender, RoutedEventArgs e)
        {
            if (MainListBox.SelectedItem != null)
            {
                string selectedItemPath = ClickHelper();
                if (!Directory.Exists(selectedItemPath))
                    Directory.CreateDirectory(selectedItemPath);
            }
            else
            {
                MessageBox.Show("Select anything in Browser");
            }

        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (MainListBox.SelectedItem != null)
            {
                ListBoxItem item = MainListBox.SelectedItem as ListBoxItem;
                ListItemControl currentItem = item.Content as ListItemControl;
                string path = (item.Content as ListItemControl).fullPath;

                if (currentItem.typeOfItem == TypeOfItem.File)
                {
                    FileStream file = new FileStream(path, FileMode.Open);
                    StreamWriter writer = new StreamWriter(file);
                    writer.Write(MainTextBox.Text);
                    writer.Close();
                    file.Close();
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

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

            ListBoxItem item = MainListBox.SelectedItem as ListBoxItem;
            ListItemControl currentItem = item.Content as ListItemControl;
            string path = (item.Content as ListItemControl).fullPath;

            if (currentItem.typeOfItem == TypeOfItem.File)
            {
                File.Delete(path);
            }
            else
            {
                Directory.Delete(path);
            }

            isDeletedOrCreated = true;
            MainListBox.Items.Clear();
            DirectoryInfo root = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.GetDirectories("root")[0];
            Explore(root);
        }

        private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

            string path = (item.Content as ListItemControl).fullPath;


            if (currentItem.typeOfItem == TypeOfItem.File)
            {
                FileStream file = new FileStream(path, FileMode.Open);
                StreamReader reader = new StreamReader(file);
                MainTextBox.Text = reader.ReadToEnd();
                reader.Close();
                file.Close();
            }
            else
            {
                MainTextBox.Text = string.Empty;
            }
        }
    }
}
