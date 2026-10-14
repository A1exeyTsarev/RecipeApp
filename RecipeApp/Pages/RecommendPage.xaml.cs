using RecipeApp.Models;
using RecipeApp.Models.RecipeApp.Models;
using RecipeApp.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RecipeApp.Pages
{
    public partial class RecommendPage : UserControl
    {
        private readonly DataService _dataService;
        private ObservableCollection<RecommendViewModel> _recommendations;
        private readonly Random _random = new Random();

        public RecommendPage()
        {
            InitializeComponent();
            _dataService = new DataService();
            LoadRecommendations();
        }

        private void LoadRecommendations()
        {
            var products = _dataService.LoadProducts();
            var recipes = _dataService.LoadRecipes();

            _recommendations = new ObservableCollection<RecommendViewModel>(
                recipes.Select(r => new RecommendViewModel(r, products))
                       .OrderByDescending(vm => vm.Percentage)
            );

            ListRecommendations.ItemsSource = _recommendations;
        }

        private void BtnPickRandom_Click(object sender, RoutedEventArgs e)
        {
            LoadRecommendations();

            if (!_recommendations.Any())
            {
                MessageBox.Show(
                    "Нет рецептов! Добавьте хотя бы один рецепт.",
                    "Информация",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
                return;
            }

            var perfect = _recommendations.Where(vm => vm.Percentage == 100).ToList();

            if (perfect.Any())
            {
                var selected = perfect[_random.Next(perfect.Count)];

                MessageBox.Show(
                    $"🍳 Сегодня готовим: {selected.Name}!\n\n" +
                    $"📋 Ингредиенты:\n{selected.IngredientsList}",
                    "Рекомендация!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
            else if (_recommendations.Any())
            {
                var max = _recommendations.Max(vm => vm.Percentage);
                var best = _recommendations.Where(vm => vm.Percentage == max).ToList();
                var selected = best[_random.Next(best.Count)];

                MessageBox.Show(
                    $"🍳 Попробуйте: {selected.Name}\n\n" +
                    $"Доступно: {selected.Percentage}% ингредиентов\n" +
                    $"Не хватает: {selected.MissingList}",
                    "Рекомендация!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }

        // ============ VIEW MODEL ============
        public class RecommendViewModel : INotifyPropertyChanged
        {
            public Recipe Recipe { get; }
            public string Name => Recipe.Name;
            public string IngredientsList => string.Join(", ", Recipe.Ingredients);
            public int Percentage { get; }
            public string PercentageText => $"{Percentage}%";
            public string PercentageWidth => $"{Percentage}%";
            public string MissingList { get; }
            public string FullStatus { get; }
            public string StatusColor { get; }

            public string ProgressColor
            {
                get
                {
                    if (Percentage >= 80) return "#27AE60";
                    if (Percentage >= 50) return "#F39C12";
                    return "#E74C3C";
                }
            }

            public RecommendViewModel(Recipe recipe, System.Collections.Generic.List<string> products)
            {
                Recipe = recipe;

                var available = recipe.Ingredients.Where(i => products.Contains(i)).ToList();
                var missing = recipe.Ingredients.Where(i => !products.Contains(i)).ToList();

                Percentage = recipe.Ingredients.Any()
                    ? (available.Count * 100) / recipe.Ingredients.Count
                    : 0;

                MissingList = missing.Any() ? string.Join(", ", missing) : "Все есть!";

                if (Percentage == 100)
                {
                    FullStatus = "✅ Все ингредиенты есть! Можно готовить!";
                    StatusColor = "#27AE60";
                }
                else if (Percentage >= 50)
                {
                    FullStatus = $"⚠️ Не хватает: {MissingList}";
                    StatusColor = "#F39C12";
                }
                else
                {
                    FullStatus = $"❌ Много продуктов не хватает: {MissingList}";
                    StatusColor = "#E74C3C";
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string name) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}