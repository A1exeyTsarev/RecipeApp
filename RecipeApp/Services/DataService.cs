using RecipeApp.Models;
using RecipeApp.Models.RecipeApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace RecipeApp.Services
{
    public class DataService
    {
        private readonly string _productsFile;
        private readonly string _recipesFile;
        private int _nextRecipeId = 1;

        public DataService()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string folder = Path.Combine(appData, "RecipeApp");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            _productsFile = Path.Combine(folder, "products.json");
            _recipesFile = Path.Combine(folder, "recipes.json");
        }

        // ============ ПРОДУКТЫ ============
        public List<string> LoadProducts()
        {
            if (!File.Exists(_productsFile))
            {
                // Создаем тестовые продукты при первом запуске
                var defaultProducts = new List<string>
                {
                    "Молоко", "Яйца", "Мука", "Сахар", "Соль",
                    "Масло растительное", "Сметана", "Творог",
                    "Помидоры", "Огурцы", "Лук", "Картофель",
                    "Курица", "Рис", "Макароны", "Сыр", "Хлеб"
                };
                SaveProducts(defaultProducts);
                return defaultProducts;
            }

            try
            {
                string json = File.ReadAllText(_productsFile);
                return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }

        public void SaveProducts(List<string> products)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(products, options);
                File.WriteAllText(_productsFile, json);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка сохранения продуктов: {ex.Message}",
                    "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        // ============ РЕЦЕПТЫ ============
        public List<Recipe> LoadRecipes()
        {
            if (!File.Exists(_recipesFile))
            {
                // Создаем тестовые рецепты при первом запуске
                var defaultRecipes = new List<Recipe>
                {
                    new Recipe(1, "Блины", new List<string> { "Молоко", "Яйца", "Мука", "Сахар", "Соль", "Масло растительное" }),
                    new Recipe(2, "Омлет", new List<string> { "Яйца", "Молоко", "Соль", "Масло растительное" }),
                    new Recipe(3, "Салат Оливье", new List<string> { "Картофель", "Морковь", "Яйца", "Колбаса", "Огурцы", "Сметана", "Соль" }),
                    new Recipe(4, "Курица с рисом", new List<string> { "Курица", "Рис", "Морковь", "Лук", "Соль", "Специи", "Масло растительное" }),
                    new Recipe(5, "Сырники", new List<string> { "Творог", "Яйца", "Мука", "Сахар", "Соль", "Масло растительное" }),
                    new Recipe(6, "Борщ", new List<string> { "Говядина", "Картофель", "Капуста", "Свекла", "Морковь", "Лук", "Чеснок", "Помидоры", "Соль", "Специи" }),
                    new Recipe(7, "Макароны по-флотски", new List<string> { "Макароны", "Говядина", "Лук", "Соль", "Масло растительное" })
                };
                SaveRecipes(defaultRecipes);
                return defaultRecipes;
            }

            try
            {
                string json = File.ReadAllText(_recipesFile);
                var recipes = JsonSerializer.Deserialize<List<Recipe>>(json) ?? new List<Recipe>();

                if (recipes.Any())
                    _nextRecipeId = recipes.Max(r => r.Id) + 1;

                return recipes;
            }
            catch
            {
                return new List<Recipe>();
            }
        }

        public void SaveRecipes(List<Recipe> recipes)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(recipes, options);
                File.WriteAllText(_recipesFile, json);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка сохранения рецептов: {ex.Message}",
                    "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        public int GetNextRecipeId()
        {
            return _nextRecipeId++;
        }
    }
}