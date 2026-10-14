using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Models
{
    namespace RecipeApp.Models
    {
        public class Recipe
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public List<string> Ingredients { get; set; }

            public Recipe()
            {
                Ingredients = new List<string>();
            }

            public Recipe(int id, string name, List<string> ingredients)
            {
                Id = id;
                Name = name;
                Ingredients = ingredients ?? new List<string>();
            }
        }
    }
}
