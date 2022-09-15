namespace CallingAPIInClient.Models
{
    public class NewOrder
    {
        public int Id { get; set; }
        public int OrderId { get; set; }

        //public int? count { get; set; }
        public string Email { get; set; }
        //public UserList user { get; set; }
        public string Image { get; set; }
        public string FoodName { get; set; }
        //public Food food { get; set; }
        public float Price { get; set; }
        public int Qnt { get; set; }
        public float TotalPrice { get; set; }

        public virtual OrderDetails? OrderDetails { get; set; }

        //public OrderDetails orderDetails { get; set; }

    }
}
