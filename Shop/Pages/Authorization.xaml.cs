using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Shop
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Page
    {
        private bool _checkEntriesCAPTCHA;
        private string _CAPTCHA;
        private DispatcherTimer _timer;
        private int _time;

        public Authorization()
        {
            InitializeComponent();

            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(Timer_Tick);
            _timer.Interval = new TimeSpan(0, 0, 1);
        }

        private void btnEntry_Click(object sender, RoutedEventArgs e)
        {
            User user = Base.Entities.User.FirstOrDefault(x => x.UserLogin == tboxLogin.Text);

            if (user == null || pbPassword.Password != user.UserPassword)
            {
                MessageBox.Show("Неверные данные для входа", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Warning);

                if (_checkEntriesCAPTCHA)
                {
                    StartTimer();
                    return;
                }

                if (canvas.Visibility == Visibility.Visible)
                {
                    _checkEntriesCAPTCHA = true;
                }

                _CAPTCHA = GetCAPTHA();
                return;
            }

            if (spCAPTCHA.Visibility == Visibility.Visible && tboxCAPTCHA.Text.ToLower() != _CAPTCHA.ToLower())
            {
                MessageBox.Show("Неверный код CAPTCHA", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Warning);
                StartTimer();
                return;
            }

            Base.User = user;
            Base.TBUser.Text = user.UserSurname + " " + user.UserName + " " + user.UserPatronymic;
            Base.SPEntry.Visibility = Visibility.Visible;
            Base.MainFrame.Navigate(new ShowProduct());
        }

        /// <summary>
        /// Визуализация блокировки входа
        /// </summary>
        private void StartTimer()
        {
            tbTimer.Visibility = Visibility.Visible;
            tbTimer.Text = "Повторить попытку входа можно будет через 10 с";
            tboxLogin.IsEnabled = false;
            tboxCAPTCHA.IsEnabled = false;
            pbPassword.IsEnabled = false;
            btnEntry.IsEnabled = false;
            btnEntryGuest.IsEnabled = false;
            _time = 10;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _time--;
            tbTimer.Text = "Повторить попытку входа можно будет через " + _time + " с";

            if (_time == 0)
            {
                _timer.Stop();
                _CAPTCHA = GetCAPTHA();
                tbTimer.Visibility = Visibility.Collapsed;
                tboxLogin.IsEnabled = true;
                tboxCAPTCHA.IsEnabled = true;
                pbPassword.IsEnabled = true;
                btnEntry.IsEnabled = true;
                btnEntryGuest.IsEnabled = true;
            }
        }

        /// <summary>
        /// Генерирует CAPTCHA и выводит её на экран
        /// </summary>
        /// <returns>Текст CAPTCHA</returns>
        private string GetCAPTHA()
        {
            canvas.Children.Clear();
            Random rand = new Random();

            string result = "";
            char c = '0';
            int count = 4;

            for (int i = 0; i < count; i++)
            {
                switch (rand.Next(0, 3))
                {
                    case 0:
                        c = (char)rand.Next(48, 58);
                        break;
                    case 1:
                        c = (char)rand.Next(65, 91);
                        break;
                    case 2:
                        c = (char)rand.Next(97, 123);
                        break;
                }
                result += c;

                TextBlock tb = new TextBlock()
                {
                    Text = c.ToString(),
                    Padding = new Thickness(i * 25 + 5, rand.Next(21), rand.Next(21), 10),
                    FontSize = rand.Next(20, 26)
                };

                switch (rand.Next(0, 4))
                {
                    case 0:
                        tb.FontStyle = FontStyles.Italic;
                        break;
                    case 1:
                        tb.FontWeight = FontWeights.Bold;
                        break;
                    case 2:
                        tb.FontStyle = FontStyles.Italic;
                        tb.FontWeight = FontWeights.Bold;
                        break;
                }

                canvas.Children.Add(tb);
            }

            for (int i = 0; i < 15; i++)
            {
                Line line = new Line()
                {
                    X1 = rand.Next(101),
                    Y1 = rand.Next(51),
                    X2 = rand.Next(101),
                    Y2 = rand.Next(51),
                    Stroke = new SolidColorBrush(Color.FromRgb((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256)))
                };
                canvas.Children.Add(line);
            }

            spCAPTCHA.Visibility = Visibility.Visible;
            return result;
        }

        private void btnEntryGuest_Click(object sender, RoutedEventArgs e)
        {
            Base.TBUser.Text = "Гость";
            Base.SPEntry.Visibility = Visibility.Visible;
            Base.MainFrame.Navigate(new ShowProduct());
        }
    }
}