using System.Windows;

namespace Shop
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Base.Entities = new PracticEntities();
            Base.MainFrame = frame;
            Base.SPEntry = spEntry;
            Base.TBUser = tbUser;

            Base.MainFrame.Navigate(new Authorization());
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            spEntry.Visibility = Visibility.Collapsed;
            tbUser.Text = "";
            Base.User = null;
            Base.MainFrame.Navigate(new Authorization());
        }
    }
}