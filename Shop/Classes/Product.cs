using System;
using System.Windows;
using System.Windows.Media;

namespace Shop
{
    public partial class Product
    {
        public string Photo
        {
            get
            {
                if (ProductPhoto == null)
                {
                    return "\\Resources\\picture.png";
                }
                else
                {
                    return ProductPhoto;
                }
            }
        }

        public SolidColorBrush BackgroundColor
        {
            get
            {
                if (ProductDiscount > 15)
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFrom("#7fff00");
                }

                return Brushes.White;
            }
        }

        public Visibility VisibilityCrossedCost
        {
            get
            {
                if (ProductDiscount != 0)
                {
                    return Visibility.Visible;
                }

                return Visibility.Collapsed;
            }
        }

        public double ProductCostDiscount
        {
            get
            {
                if (ProductDiscount != 0)
                {
                    return Math.Round(ProductCost - ProductCost / 100 * ProductDiscount, 2);
                }

                return Math.Round(ProductCost, 2);
            }

            set { }
        }
    }
}