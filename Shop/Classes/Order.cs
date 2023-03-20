using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Shop
{
    public partial class Order
    {
        public SolidColorBrush BackgroundColor
        {
            get
            {
                bool checkCount = false;

                foreach (OrderProduct item in Base.Entities.OrderProduct.Where(x => x.OrderID == OrderID))
                {
                    if (item.Product1.ProductCount <= 3)
                    {
                        if (item.Product1.ProductCount == 0)
                        {
                            return (SolidColorBrush)new BrushConverter().ConvertFrom("#ff8c00");
                        }

                        checkCount = true;
                        break;
                    }
                }

                if (checkCount)
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFrom("#20b2aa");
                }

                return Brushes.White;
            }
        }

        public double Cost
        {
            get
            {
                double cost = 0;

                foreach (OrderProduct item in Base.Entities.OrderProduct.Where(x => x.OrderID == OrderID))
                {
                    if (item.Product1.ProductDiscount != 0)
                    {
                        cost += item.Product1.ProductCostDiscount * item.Count;
                    }
                    else
                    {
                        cost += item.Product1.ProductCost * item.Count;
                    }
                }

                return Math.Round(cost, 2);
            }
        }

        public double Discount
        {
            get
            {
                double cost = 0;
                double costWithoutDiscount = 0;

                foreach (OrderProduct item in Base.Entities.OrderProduct.Where(x => x.OrderID == OrderID))
                {
                    if (item.Product1.ProductDiscount != 0)
                    {
                        cost += item.Product1.ProductCostDiscount * item.Count;
                    }
                    else
                    {
                        cost += item.Product1.ProductCost * item.Count;
                    }

                    costWithoutDiscount += item.Product1.ProductCost * item.Count;
                }

                if (cost != costWithoutDiscount)
                {
                    return Math.Round(100 - cost * 100 / costWithoutDiscount, 1);
                }

                return 0;
            }
        }

        public Visibility DiscountVisibility
        {
            get
            {
                if (Discount == 0)
                {
                    return Visibility.Collapsed;
                }

                return Visibility.Visible;
            }
        }

        public string Composition
        {
            get
            {
                string text = null;

                foreach (OrderProduct item in Base.Entities.OrderProduct.Where(x => x.OrderID == OrderID))
                {
                    text += item.Product1.ProductArticleNumber + ", " + item.Product1.ProductName + "\n";
                }

                return text.Substring(0, text.Length - 1);
            }
        }
    }
}