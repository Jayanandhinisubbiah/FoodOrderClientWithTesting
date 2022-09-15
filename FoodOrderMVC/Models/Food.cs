namespace CallingAPIInClient.Models
{
    public class Food
    {
        public int FoodId { get; set; }
        public string CategoryName { get; set; }
        public string FoodName { get; set; }
        public float price { get; set; }
        public string Image { get; set; }
        public string Detail { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }

    }
}
