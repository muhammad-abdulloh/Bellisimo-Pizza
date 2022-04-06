using Microsoft.AspNetCore.Http;

namespace BellisimoPizza.Service.DTOs.Pizzas
{
    public class PizzaForCreationDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Size { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public IFormFile ImageUrl { get; set; }
    }
}
