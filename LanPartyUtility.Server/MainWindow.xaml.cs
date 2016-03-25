using MahApps.Metro.Controls;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LanPartyUtility.Server
{
    public partial class MainWindow : MetroWindow
    {
        MainWindowViewModel viewModel;

        public MainWindow()
        {
            viewModel = new MainWindowViewModel();
            this.Closing += viewModel.OnWindowClosing;
            this.DataContext = viewModel;
            
            InitializeComponent();

            this.TerminalGrid.Children.Add(this.viewModel.Terminal);
            Grid.SetColumn(this.viewModel.Terminal, 0);
            this.viewModel.Terminal.Background = new SolidColorBrush(Color.FromRgb(30, 30, 30));
            this.viewModel.Terminal.Foreground = Brushes.WhiteSmoke;
            this.viewModel.Terminal.FontFamily = new FontFamily("Lucida Console");
            this.viewModel.Terminal.BorderBrush = Brushes.Gray;
            this.viewModel.Terminal.BorderThickness = new Thickness(0, 1, 1, 0);
            this.viewModel.Terminal.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
        }

        private void ShowSettings(object sender, RoutedEventArgs e)
        {
            var flyout = this.Flyouts.Items[0] as Flyout;
            if (flyout == null)
            {
                return;
            }

            flyout.IsOpen = !flyout.IsOpen;
        }
    }
}
