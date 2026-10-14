using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using RecipeApp.Pages;

namespace RecipeApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Стартовая страница - Продукты
            MainFrame.Navigate(new ProductsPage());
            SetActiveButton(BtnProducts);
        }

        private void BtnProducts_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProductsPage());
            SetActiveButton(BtnProducts);
        }

        private void BtnRecipes_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new RecipesPage());
            SetActiveButton(BtnRecipes);
        }

        private void BtnRecommend_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new RecommendPage());
            SetActiveButton(BtnRecommend);
        }

        private void SetActiveButton(Button active)
        {
            var buttons = new[] { BtnProducts, BtnRecipes, BtnRecommend };

            foreach (var btn in buttons)
            {
                btn.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80)); // #2C3E50
                btn.Foreground = Brushes.White;
            }

            active.Background = new SolidColorBrush(Color.FromRgb(52, 152, 219)); // #3498DB
            active.Foreground = Brushes.White;
        }
    }
}