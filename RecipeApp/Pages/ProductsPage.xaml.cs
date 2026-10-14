using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RecipeApp.Services;

namespace RecipeApp.Pages
{
    public partial class ProductsPage : UserControl
    {
        private readonly DataService _dataService;
        private ObservableCollection<string> _products;

        public ProductsPage()
        {
            InitializeComponent();
            _dataService = new DataService();
            LoadProducts();
        }

        private void LoadProducts()
        {
            var list = _dataService.LoadProducts();
            _products = new ObservableCollection<string>(list);
            ListProducts.ItemsSource = _products;
        }

        private void SaveProducts()
        {
            _dataService.SaveProducts(_products.ToList());
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            AddProduct();
        }

        private void TxtNewProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AddProduct();
        }

        private void AddProduct()
        {
            string name = TxtNewProduct.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите название продукта!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_products.Contains(name))
            {
                MessageBox.Show("Такой продукт уже есть!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _products.Add(name);
            SaveProducts();
            TxtNewProduct.Clear();
            TxtNewProduct.Focus();
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is string product)
            {
                if (MessageBox.Show($"Удалить \"{product}\"?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    _products.Remove(product);
                    SaveProducts();
                }
            }
        }

        private void BtnClearAll_Click(object sender, RoutedEventArgs e)
        {
            if (_products.Count == 0) return;

            if (MessageBox.Show("Удалить все продукты?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                _products.Clear();
                SaveProducts();
            }
        }
    }
}