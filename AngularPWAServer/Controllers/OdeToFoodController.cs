using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AngularPWAServer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AngularPWAServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OdeToFoodController : ControllerBase
    {
        private IWebHostEnvironment _hostingEnvironment;


        public OdeToFoodController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        [Route("/latest-recipes/")]
        public IEnumerable<Recipe> LatestRecipes()
        {
            return new List<Recipe> 
            { 
                new Recipe 
                { 
                    Id = 1, 
                    Title = "Strawberry Cream Cheese-filled Banana Bread", 
                    PhotoUrl = "https://angularpwaserver.azurewebsites.net/recipes-phots/banana-bread.jpg", 
                    Description = @"In a medium bowl, mix the cream cheese, 1 tablespoon of honey, and strawberries until smooth.
Microwave the cream cheese mixture in 30 second intervals for a total of 5 minutes, stirring between each interval.This helps cook out the moisture so that the mixture doesn’t create a large pocket of steam in the bread." 
                },
                new Recipe
                {
                    Id = 2,
                    Title = "Surf And Turf Steak Roll Up",
                    Description = "Seared flank steak stuffed with sweet chunks of fresh lobster and topped with a buttery Parmesan cream sauce is undoubtedly a combination made in heaven. Make this when you really want to treat yourself to an indulgent dinner at home!",
                    PhotoUrl = "https://angularpwaserver.azurewebsites.net/recipes-phots/meat-with-cream.jpg"
                },
                new Recipe
                {
                    Id = 3,
                    Title = "Apple Pie French Toast",
                    Description = @"Ingredients
for 2 servings

2 eggs
1 cup milk(240 mL)
1 teaspoon cinnamon
½ teaspoon salt
1 teaspoon vanilla
2 tablespoons brown sugar
1 medium grated apple, divided
2 tablespoons butter
7 breads
almond and cashew, for serving
maple syrup, for serving",
                    PhotoUrl = "https://angularpwaserver.azurewebsites.net/recipes-phots/apple-pie.jpeg"},
            };
        }

        [HttpGet]
        [Route("/grandmas-cookbook/")]
        public IEnumerable<Recipe> GrandmasCookbook()
        {
            return new List<Recipe>
            {
                new Recipe
                {
                    Id = 1,
                    Title = "Sărmăluțe în foi de viță cu carne de porc",
                    Description = "Sărmăluțe în foi de viță cu carne de porc, cu sos de mărar, cu smântână  sau cu iaurt. Sarmale in foi de vita reteta culinara. ",
                    PhotoUrl = "https://angularpwaserver.azurewebsites.net/recipes-phots/sarmale.jpg"
                },
                 new Recipe
                {
                    Id = 1,
                    Title = "Cozonac",
                    Description = "REŢETĂ COZONAC ca la bunica. Ingrediente: 1kg faina, 7 oua, 70 gr drojdie, 500 ml lapte, 125 ml ulei, 125 g unt, esenta de vanilie, esenta de rom, de rom dupa gust, coaja rasa de la o lamaie, cacao, 200 gr nuca, stafide, rahat.",
                    PhotoUrl = "https://angularpwaserver.azurewebsites.net/recipes-phots/cozonac.jpg"
                },
            };
        }


        //GET api/download/12345abc
        [HttpGet("/recipes-phots/{name}")]
        public  IActionResult Download(string name)
        {
            string path = Path.Combine(_hostingEnvironment.ContentRootPath, "RecipePhotos", name);
            Stream stream = new FileStream(path, FileMode.Open);
            if (stream == null)
                return NotFound(); // returns a NotFoundResult with Status404NotFound response.

            return File(stream, "application/octet-stream", name); // returns a FileStreamResult
        }
    }
}