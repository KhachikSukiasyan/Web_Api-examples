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
using ClientSide.Models;
namespace ClientSide
{
    /// <summary>
    /// Логика взаимодействия для ListItemControl.xaml
    /// </summary>
    /// 


    public partial class ListItemControl : UserControl
    {
        public Enums typeOfItem { get; set; }
        public string relativePath { get; set; }
        public ListItemControl()
        {
            
            InitializeComponent();
          //  icon.Source = new BitmapImage(new Uri($"/Resources/folder.png", UriKind.Relative));
        }

        //protected override void OnInitialized(EventArgs e)
        //{
        //    base.OnInitialized(e);
        //    itemImage.Source = new BitmapImage(new Uri("//application:,,,/Resources/folder.png", UriKind.Relative)); 

        //}
    }
}
