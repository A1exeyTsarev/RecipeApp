using System.Windows;
using System.Windows.Input;

namespace RecipeApp.Pages
{
    public partial class AddRecipeDialog : Window
    {
        public string RecipeName => TxtRecipeName.Text.Trim();
        public string Ingredients => TxtIngredients.Text.Trim();

        public AddRecipeDialog()
        {
            InitializeComponent();
            TxtRecipeName.Focus();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (TrySave())
            {
                DialogResult = true;
                Close();
            }
        }

        private bool TrySave()
        {
            if (string.IsNullOrEmpty(RecipeName))
            {
                MessageBox.Show("Введите название рецепта!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtRecipeName.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(Ingredients))
            {
                MessageBox.Show("Введите ингредиенты!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtIngredients.Focus();
                return false;
            }

            return true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void TxtRecipeName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                TxtIngredients.Focus();
        }

        private void TxtIngredients_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (TrySave())
                {
                    DialogResult = true;
                    Close();
                }
            }
        }
    }
}