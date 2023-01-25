using SimplyGradients.ViewModels;
using System.Windows;

namespace SimplyGradients.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LinearGradientViewer_GradientCollectionUpdated(object sender)
        {
            gradientPresenter.InvalidateVisual();
            header.InvalidateVisual();
            listviewGradientStops.Items.Refresh();
        }
    }
}
