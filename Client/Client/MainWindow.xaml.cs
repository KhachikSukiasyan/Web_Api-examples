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
using System.Drawing.Drawing2D;


namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    enum Function
    {
        Sin,
        Cos
    }
    public partial class MainWindow : Window
    {
        
        Line Xasis = new Line();
        Line Yasis = new Line();

        Polyline polyline;
        List<UIElement> addedLines = new List<UIElement>();

        bool isFirstEnter = false;

        public MainWindow()
        {
            InitializeComponent();

        }
        protected override void OnContentRendered(EventArgs e)
        {

        }
        private void PlotButton_Click(object sender, RoutedEventArgs e)
        {
            double coefA;
            double coefB;

            if (string.IsNullOrEmpty(AtextBox.Text) && string.IsNullOrEmpty(BtextBox.Text))
            {
                AtextBox.Text = "1";
                BtextBox.Text = "1";

                if ((bool)SinRadioButton.IsChecked)
                    Draw(Function.Sin, 1, 1);
                else
                    Draw(Function.Cos, 1, 1);
            }
            else
            {
                if (double.TryParse(AtextBox.Text, out coefA) && double.TryParse(BtextBox.Text, out coefB))
                {
                    if ((bool)SinRadioButton.IsChecked)
                    {
                        Draw(Function.Sin, coefA, coefB);
                    }
                    else
                    {
                        Draw(Function.Cos, coefA, coefB);
                    }

                }
                else
                    MessageBox.Show("Enter correct parameter","Error");
            }
        }


        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement item in addedLines)
            {
                MainCanvas.Children.Remove(item);
            }
            
        }



        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            if (!isFirstEnter)
            {
                // XAsis
                Xasis.Stroke = Brushes.Black;
                Xasis.X1 = 0;
                Xasis.X2 = MainCanvas.ActualWidth;
                Xasis.Y1 = MainCanvas.ActualHeight / 2;
                Xasis.Y2 = MainCanvas.ActualHeight / 2;
                Xasis.StrokeThickness = 0.5;
                MainCanvas.Children.Add(Xasis);


                //YAsis
                Yasis.Stroke = Brushes.Black;
                Yasis.X1 = MainCanvas.ActualWidth / 2;
                Yasis.X2 = MainCanvas.ActualWidth / 2;
                Yasis.Y1 = 0;
                Yasis.Y2 = MainCanvas.ActualHeight;
                Yasis.StrokeThickness = 0.5;
                MainCanvas.Children.Add(Yasis);


                isFirstEnter = true;
            }
            else
            {
                // XAsis
                Xasis.Y1 = MainCanvas.ActualHeight / 2;
                Xasis.Y2 = MainCanvas.ActualHeight / 2;
                Xasis.X2 = MainCanvas.ActualWidth;


                // YAsis
                Yasis.Y2 = MainCanvas.ActualHeight;
                Yasis.X1 = MainCanvas.ActualWidth / 2;
                Yasis.X2 = MainCanvas.ActualWidth / 2;

            }
        }

        private void Draw(Function func, double coefA, double coefB)
        {
            double syntheticX = 0.8;
            double syntheticY = 6;

            polyline = new Polyline { Stroke = Brushes.Black, StrokeThickness = 1 };

            double realA = -MainCanvas.ActualWidth / 2;
            double realB = MainCanvas.ActualWidth / 2;
            double algA = -60;
            double algB = 60;
            double algDx = (algB - algA) / 1000;

            double scaleX = (realB - realA) / (algB - algA);


            Point tempPoint = new Point();
            if (func == Function.Sin)
            {
                for (double i = algA; i < algB; i += algDx)
                {
                    tempPoint.X = realA + scaleX * i + MainCanvas.ActualWidth;
                    tempPoint.Y = Math.Sin(i * 1 / coefA * syntheticX) * coefB * syntheticY + MainCanvas.ActualHeight / 2;

                    polyline.Points.Add(tempPoint);
                }
            }
            else
            for (double i = algA; i < algB; i += algDx)
            {
                tempPoint.X = realA + scaleX * i + MainCanvas.ActualWidth;
                tempPoint.Y = Math.Cos(i * 1/coefA * syntheticX) * coefB * syntheticY + MainCanvas.ActualHeight / 2;

                polyline.Points.Add(tempPoint);
            }
            MainCanvas.Children.Add(polyline);
            addedLines.Add(polyline);

        }



    }
}
