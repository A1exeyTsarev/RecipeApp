using RecipeApp.Models;
using RecipeApp.Models.RecipeApp.Models;
using RecipeApp.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RecipeApp.Pages
{
    public partial class RecipesPage : UserControl
    {
        private readonly DataService _dataService;
        private ObservableCollection<RecipeViewModel> _recipes;
        private ObservableCollection<string> _products;

        public RecipesPage()
        {
            InitializeComponent();
            _dataService = new DataService();
            LoadData();
        }

        private void LoadData()
        {
            _products = new ObservableCollection<string>(_dataService.LoadProducts());
            var recipes = _dataService.LoadRecipes();

            _recipes = new ObservableCollection<RecipeViewModel>(
                recipes.Select(r => new RecipeViewModel(r, _products.ToList())));

            ListRecipes.ItemsSource = _recipes;
        }

        private void SaveRecipes()
        {
            var list = _recipes.Select(r => r.Recipe).ToList();
            _dataService.SaveRecipes(list);
        }

        private void RefreshStatuses()
        {
            var products = _dataService.LoadProducts();
            foreach (var vm in _recipes)
            {
                vm.UpdateStatus(products);
            }
            ListRecipes.Items.Refresh();
        }

        private void BtnAddRecipe_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddRecipeDialog();

            if (dialog.ShowDialog() == true && !string.IsNullOrEmpty(dialog.RecipeName))
            {
                var ingredients = dialog.Ingredients
                    .Split(',')
                    .Select(i => i.Trim())
                    .Where(i => !string.IsNullOrEmpty(i))
                    .ToList();

                if (ingredients.Any())
                {
                    var recipe = new Recipe(
                        _dataService.GetNextRecipeId(),
                        dialog.RecipeName,
                        ingredients
                    );

                    var vm = new RecipeViewModel(recipe, _products.ToList());
                    _recipes.Add(vm);
                    SaveRecipes();
                    RefreshStatuses();
                }
                else
                {
                    MessageBox.Show("Добавьте хотя бы один ингредиент!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void DeleteRecipe_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is RecipeViewModel vm)
            {
                if (MessageBox.Show($"Удалить рецепт \"{vm.Name}\"?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    _recipes.Remove(vm);
                    SaveRecipes();
                }
            }
        }

        // ============ VIEW MODEL ============
        public class RecipeViewModel : INotifyPropertyChanged
        {
            public Recipe Recipe { get; }
            public string Name => Recipe.Name;
            public string IngredientsList => string.Join(", ", Recipe.Ingredients);

            private string _statusText;
            private string _statusColor;
            private string _missingText;
            private bool _hasMissing;

            public string StatusText
            {
                get => _statusText;
                set { _statusText = value; OnPropertyChanged(nameof(StatusText)); }
            }

            public string StatusColor
            {
                get => _statusColor;
                set { _statusColor = value; OnPropertyChanged(nameof(StatusColor)); }
            }

            public string MissingText
            {
                get => _missingText;
                set { _missingText = value; OnPropertyChanged(nameof(MissingText)); }
            }

            public bool HasMissing
            {
                get => _hasMissing;
                set { _hasMissing = value; OnPropertyChanged(nameof(HasMissing)); }
            }

            public RecipeViewModel(Recipe recipe, System.Collections.Generic.List<string> products)
            {
                Recipe = recipe;
                UpdateStatus(products);
            }

            public void UpdateStatus(System.Collections.Generic.List<string> products)
            {
                var available = Recipe.Ingredients.Where(i => products.Contains(i)).ToList();
                var missing = Recipe.Ingredients.Where(i => !products.Contains(i)).ToList();

                if (!missing.Any())
                {
                    StatusText = "✅ Можно готовить!";
                    StatusColor = "#27AE60";
                    MissingText = "";
                    HasMissing = false;
                }
                else
                {
                    StatusText = "❌ Нет продуктов";
                    StatusColor = "#E74C3C";
                    MissingText = "Нет: " + string.Join(", ", missing);
                    HasMissing = true;
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string name) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}