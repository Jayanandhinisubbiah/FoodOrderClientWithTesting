using CallingAPIInClient.Models;

namespace CallingAPIInClient.ViewModels
{
    public class FoodViewModel
    {
        public int FoodId { get; set; }
        public string CategoryName { get; set; }
        public string FoodName { get; set; }
        public float price { get; set; }
        public IFormFile ImageView { get; set; }
        public string Detail { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
