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

namespace Shop
{
    /// <summary>
    /// Логика взаимодействия для ChangeOrder.xaml
    /// </summary>
    public partial class ChangeOrder : Window
    {
        private Order _order;

        public ChangeOrder(Order order)
        {
            InitializeComponent();

            _order = order;
            cbStatus.ItemsSource = Base.Entities.Statuses.ToList();
            cbStatus.SelectedValuePath = "Id_status";
            cbStatus.DisplayMemberPath = "Status";

            tbOrder.Text = "Заказ №" + order.OrderID;
            dpDate.SelectedDate = order.OrderDeliveryDate;
            cbStatus.SelectedValue = order.OrderStatus;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            _order.OrderDeliveryDate = dpDate.SelectedDate.Value;
            _order.OrderStatus = (int)cbStatus.SelectedValue;

            try
            {
                Base.Entities.SaveChanges();
                MessageBox.Show("Заказ успешно изменён", "Изменение заказа", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
                Base.MainFrame.Navigate(new WorkOrder());
            }
            catch
            {
                MessageBox.Show("Не удалось изменить данные", "Изменение заказа", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}