using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FitRectangle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var tmp = new FitRectangleViewModel
            {
                Canvas = Canvas
            };
            base.DataContext = tmp;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (base.DataContext is FitRectangleViewModel dataModel)
            {
                dataModel.InitMainRectangle();
                dataModel.InitSecondaryRectangles();

                dataModel.DrawRectangles();
            }
        }
    }
}