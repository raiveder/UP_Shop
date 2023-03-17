using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Shop
{
    /// <summary>
    /// Логика взаимодействия для ShowOrder.xaml
    /// </summary>
    public partial class ShowOrder : Window
    {
        private List<Product> _productsList;
        private List<int> _countProducts;
        private double _cost;
        private double _costWithoutDiscount;

        public ShowOrder(List<Product> products)
        {
            InitializeComponent();

            if (Base.User == null)
            {
                rdClient.Height = new GridLength(0, GridUnitType.Pixel);
            }
            else
            {
                tbClient.Text = Base.TBUser.Text;
            }

            cbPoints.ItemsSource = Base.Entities.PointOfIssue.ToList();
            cbPoints.SelectedValuePath = "ID_Point";
            cbPoints.DisplayMemberPath = "Adress";

            _productsList = products;
            lvProducts.ItemsSource = _productsList;

            _countProducts = new List<int>();
            for (int i = 0; i < _productsList.Count; i++)
            {
                _countProducts.Add(1);
            }

            CalculationCost();
        }

        /// <summary>
        /// Вычисление суммы заказа и скидки
        /// </summary>
        private void CalculationCost()
        {
            _cost = 0;
            _costWithoutDiscount = 0;

            for (int i = 0; i < _productsList.Count; i++)
            {
                if (_productsList[i].ProductDiscount != 0)
                {
                    _cost += _productsList[i].ProductCostDiscount * _countProducts[i];
                }
                else
                {
                    _cost += _productsList[i].ProductCost * _countProducts[i];
                }

                _costWithoutDiscount += _productsList[i].ProductCost * _countProducts[i];
            }

            tbCost.Text = "Сумма заказа: " + Math.Round(_cost, 2) + " руб.";

            if (_costWithoutDiscount != _cost)
            {
                tbDiscount.Visibility = Visibility.Visible;
                tbDiscount.Text = "Скидка: " + Math.Round(100 - _cost * 100 / _costWithoutDiscount, 1) + "%";
            }
            else
            {
                tbDiscount.Visibility = Visibility.Collapsed;
            }
        }

        private void tbCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int index = _productsList.FindIndex(x => x.ProductArticleNumber == tb.Uid);

            if (int.TryParse(tb.Text, out int count))
            {
                _countProducts[index] = count;

                if (count == 0)
                {
                    DeleteProduct(index);
                }

                CalculationCost();
            }
            else
            {
                MessageBox.Show("Количество товара может быть представлено только целым числом", "Оформление заказа", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int index = _productsList.FindIndex(x => x.ProductArticleNumber == btn.Uid);
            DeleteProduct(index);
        }

        /// <summary>
        /// Удаление продукта из заказа
        /// </summary>
        /// <param name="index">Индекс продукта в списке _productsList</param>
        private void DeleteProduct(int index)
        {
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить товар?", "Оформление заказа", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _productsList.RemoveAt(index);
                _countProducts.RemoveAt(index);
                List<Product> products = new List<Product>();
                products.AddRange(_productsList);
                lvProducts.ItemsSource = products;

                CalculationCost();

                if (_productsList.Count == 0)
                {
                    MessageBox.Show("Все товары удалены из заказа.\nПосле закрытия уведомления будет осуществлён переход на окно с товарами", "Оформление заказа",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
            }
        }

        private void btnPlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            if (cbPoints.SelectedValue == null)
            {
                MessageBox.Show("Выберите пункт выдачи", "Оформление заказа", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            int? idUser = null;

            if (Base.User != null)
            {
                idUser = Base.User.UserID;
            }

            Order order = new Order()
            {
                OrderDate = DateTime.Now.Date,
                OrderDeliveryDate = DateTime.Now.Date.AddDays(GetDelivetyTime()),
                OrderPickupPoint = (int)cbPoints.SelectedValue,
                Client = idUser,
                Code = new Random().Next(100, 1000),
                OrderStatus = 1
            };

            Base.Entities.Order.Add(order);
            try
            {
                Base.Entities.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Не удалось оформить заказ", "Оформление заказа", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            for (int i = 0; i < _productsList.Count; i++)
            {
                OrderProduct temp = new OrderProduct()
                {
                    OrderID = order.OrderID,
                    Product = _productsList[i].ProductArticleNumber,
                    Count = _countProducts[i]
                };
                Base.Entities.OrderProduct.Add(temp);
            }

            try
            {
                Base.Entities.SaveChanges();
                MessageBox.Show("Заказ успешно оформлен. Сейчас будет открыт талон заказа", "Оформление заказа", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Не удалось оформить заказ", "Оформление заказа", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string filename = CreatePDF("Order", order.Code);
            Process.Start(filename);

            _productsList.Clear();
            _countProducts.Clear();
            Close();
        }

        /// <summary>
        /// Создание PFD документа с заказом
        /// </summary>
        /// <param name="filename">Имя создаваемого файла без расширения</param>
        /// <returns>Имя файла с расширением</returns>
        private string CreatePDF(string filename, int code)
        {
            filename += ".pdf";

            List<Order> orders = Base.Entities.Order.ToList();

            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont fontHeader = new XFont("Arial", 25, XFontStyle.Bold);
            XFont font = new XFont("Arial", 20, XFontStyle.Regular);

            int y = 10;

           
            gfx.DrawString("Заказ №" + orders[orders.Count - 1].OrderID + 1, fontHeader, XBrushes.Black, new XRect(10, y, page.Width, page.Height), XStringFormats.TopCenter);
            gfx.DrawString("Дата оформления заказа: " + DateTime.Now.Date.ToString("dd MMMM yyyy"), font, XBrushes.Black, new XRect(10, y += 50, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(tbCost.Text, font, XBrushes.Black, new XRect(10, y += 30, page.Width, page.Height), XStringFormats.TopLeft);

            if (tbDiscount.Visibility == Visibility.Visible)
            {
                gfx.DrawString(tbDiscount.Text, font, XBrushes.Black, new XRect(10, y += 30, page.Width, page.Height), XStringFormats.TopLeft);
            }

            gfx.DrawString("Пункт выдачи: " + cbPoints.Text, font, XBrushes.Black, new XRect(10, y += 30, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("Срок доставки: " + GetDelivetyTime() + " дней", font, XBrushes.Black, new XRect(10, y += 30, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("Код получения: " + code, fontHeader, XBrushes.Black, new XRect(10, y += 50, page.Width, page.Height), XStringFormats.TopLeft);

            PrintOrderComposition(document, gfx, page, y);

            document.Save(filename);

            return filename;
        }

        /// <summary>
        /// Получение срока доставки заказа
        /// </summary>
        /// <returns>Количество дней доставки заказа</returns>
        private int GetDelivetyTime()
        {
            foreach (Product item in _productsList)
            {
                if (item.ProductCount <= 3)
                {
                    return 6;
                }
            }

            return 3;
        }

        private void PrintOrderComposition(PdfDocument document, XGraphics gfx, PdfPage page, int y)
        {
            XFont fontHeader = new XFont("Arial", 25, XFontStyle.Bold);
            XFont font = new XFont("Arial", 20, XFontStyle.Regular);

            gfx.DrawString("Состав заказа", fontHeader, XBrushes.Black, new XRect(0, y += 50, page.Width, page.Height), XStringFormats.TopCenter);
            gfx.DrawString(1 + ". " + _productsList[0].ProductArticleNumber + ", " + _productsList[0].ProductName, font,
                        XBrushes.Black, new XRect(10, y += 50, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString(_countProducts[0] + " " + _productsList[0].UnitsOfMeasurement.Unit, font,
                XBrushes.Black, new XRect(page.Width - 70, y, page.Width, page.Height), XStringFormats.TopLeft);

            for (int i = 1; i < _productsList.Count; i++)
            {
                gfx.DrawString(i + 1 + ". " + _productsList[i].ProductArticleNumber + ", " + _productsList[i].ProductName, font,
                    XBrushes.Black, new XRect(10, y += 25, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(_countProducts[i] + " " + _productsList[i].UnitsOfMeasurement.Unit, font,
                    XBrushes.Black, new XRect(page.Width - 70, y, page.Width, page.Height), XStringFormats.TopLeft);
            }
        }
    }
}