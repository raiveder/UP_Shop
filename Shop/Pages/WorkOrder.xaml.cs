using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Shop
{
    /// <summary>
    /// Логика взаимодействия для WorkOrder.xaml
    /// </summary>
    public partial class WorkOrder : Page
    {
        public WorkOrder()
        {
            InitializeComponent();

            cbSort.SelectedIndex = 0;
            cbFilter.SelectedIndex = 0;
        }

        private void tbClient_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            int id = Convert.ToInt32(tb.Uid);

            Order order = Base.Entities.Order.FirstOrDefault(x => x.OrderID == id);

            if (order.Client == null)
            {
                tb.Text = "Не указан";
            }
            else
            {
                tb.Text = order.User.UserSurname + "\n" + order.User.UserName + "\n" + order.User.UserPatronymic;
            }
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int id = Convert.ToInt32(btn.Uid);

            ChangeOrder change = new ChangeOrder(Base.Entities.Order.FirstOrDefault(x => x.OrderID == id));
            change.ShowDialog();
        }

        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Order> list = Base.Entities.Order.ToList();

            switch (cbSort.SelectedIndex)
            {
                case 0:
                    list = list.ToList();
                    break;
                case 1:
                    list = list.ToList();
                    list.Sort((x, y) => x.Cost.CompareTo(y.Cost));
                    break;
                case 2:
                    list = list.ToList();
                    list.Sort((x, y) => x.Cost.CompareTo(y.Cost));
                    list.Reverse();
                    break;
                default:
                    break;
            }

            switch (cbFilter.SelectedIndex)
            {
                case 0:
                    list = list.ToList();
                    break;
                case 1:
                    list = list.Where(x => x.Discount <= 10).ToList();
                    break;
                case 2:
                    list = list.Where(x => x.Discount > 10 && x.Discount < 15).ToList();
                    break;
                case 3:
                    list = list.Where(x => x.Discount >= 15).ToList();
                    break;
                default:
                    break;
            }

            lvOrders.ItemsSource = list;

            if (list.Count == 0)
            {
                MessageBox.Show("Результаты группировки отсутствуют", "Заказы", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Base.MainFrame.Navigate(new ShowProduct());
        }
    }
}