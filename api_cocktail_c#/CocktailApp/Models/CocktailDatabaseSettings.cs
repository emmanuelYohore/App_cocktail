namespace CocktailApp.Models
{
    public class CocktailDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string UsersCollectionName { get; set; }
        public string CocktailsCollectionName { get; set; }
        public string IngredientsCollectionName { get; set; }
        public string CocktailIngredientsCollectionName { get; set; }
        public string RatingsCollectionName { get; set; }
    }
}
