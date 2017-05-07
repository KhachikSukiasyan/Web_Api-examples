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
using System.Windows.Shapes;

namespace ClientSide
{
    /// <summary>
    /// Логика взаимодействия для CreateFolderWindow.xaml
    /// </summary>
    public partial class CreateFolderWindow : Window
    {
        public delegate void passEnteredInfo(string info);
        passEnteredInfo mainDelegate;

        public CreateFolderWindow(passEnteredInfo del)
        {
            mainDelegate = del;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainDelegate(FolderTextBox.Text);
            Close();
        }

    }
}
