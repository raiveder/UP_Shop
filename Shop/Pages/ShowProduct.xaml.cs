using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Shop
{
    /// <summary>
    /// Логика взаимодействия для ShowProduct.xaml
    /// </summary>
    public partial class ShowProduct : Page
    {
        private List<Product> _productsList = new List<Product>();

        public ShowProduct()
        {
            InitializeComponent();

            cbSort.SelectedIndex = 0;
            cbFilter.SelectedIndex = 0;

            if (Base.User == null || Base.User.UserRole == 3)
            {
                menuItemDelete.Visibility = Visibility.Collapsed;
            }
            else
            {
                menuItemAdd.Visibility = Visibility.Collapsed;
                btnWorkOrder.Visibility = Visibility.Visible;
            }
        }

        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void tbFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        /// <summary>
        /// Осуществление фильтрации, сортировки и поиска
        /// </summary>
        private void Filter()
        {
            List<Product> products = Base.Entities.Product.ToList();
            int count = products.Count;

            switch (cbSort.SelectedIndex)
            {
                case 1:
                    products.Sort((x, y) => x.ProductCost.CompareTo(y.ProductCost));
                    break;
                case 2:
                    products.Sort((x, y) => x.ProductCost.CompareTo(y.ProductCost));
                    products.Reverse();
                    break;
                default:
                    break;
            }

            switch (cbFilter.SelectedIndex)
            {
                case 1:
                    products = products.Where(x => x.ProductDiscount <= 9.99).ToList();
                    break;
                case 2:
                    products = products.Where(x => x.ProductDiscount >= 10 && x.ProductDiscount <= 14.99).ToList();
                    break;
                case 3:
                    products = products.Where(x => x.ProductDiscount >= 15).ToList();
                    break;
                default:
                    break;
            }

            if (tbFind.Text.Length > 0)
            {
                products = products.Where(x => x.ProductName.ToLower().Contains(tbFind.Text.ToLower())).ToList();
            }

            lvProduct.ItemsSource = products;
            tbCount.Text = products.Count + " из " + count;

            if (products.Count == 0)
            {
                MessageBox.Show("Товаров с подходящим фильтром не найдено", "Товары", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void MenuItemAdd_Click(object sender, RoutedEventArgs e)
        {
            if (lvProduct.SelectedItems.Count > 1)
            {
                MessageBox.Show("Выберите 1 товар", "Формирование заказа", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Product product = (Product)lvProduct.SelectedItem;

            if (_productsList.Contains(product))
            {
                MessageBox.Show("Этот товар уже добавлен в заказ", "Формирование заказа", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            _productsList.Add(product);
            if (_productsList.Count > 0)
            {
                btnShowOrder.Visibility = Visibility.Visible;
            }
            else
            {
                btnShowOrder.Visibility = Visibility.Collapsed;
            }
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lvProduct.SelectedItems.Count > 1)
            {
                MessageBox.Show("Выберите 1 товар", "Удаление товара", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Product product = (Product)lvProduct.SelectedItem;

            OrderProduct order = Base.Entities.OrderProduct.FirstOrDefault(x => x.Product == product.ProductArticleNumber);

            if (order == null)
            {
                try
                {
                    Base.Entities.Product.Remove(product);
                    Base.Entities.SaveChanges();
                    MessageBox.Show("Товар успешно удалён", "Удаление товара", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch
                {
                    MessageBox.Show("При удалении товара произошла ошибка", "Удаление товара", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Нельзя удалить товар, указанный в заказе", "Удаление товара", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnShowOrder_Click(object sender, RoutedEventArgs e)
        {
            ShowOrder order = new ShowOrder(_productsList);
            order.ShowDialog();

            if (_productsList.Count == 0)
            {
                btnShowOrder.Visibility = Visibility.Collapsed;
            }
        }

        private void btnWorkOrder_Click(object sender, RoutedEventArgs e)
        {
            Base.MainFrame.Navigate(new WorkOrder());
        }
    }
}