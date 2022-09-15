namespace CallingAPIInClient.Models
{
    public class Cart
    {
        public int CartId { get; set; }

        public int UserId { get; set; }


        public int FoodId { get; set; }
        public int Qnt { get; set; }
        public virtual ICollection<UserList> User { get; set; }
        public virtual Food Food { get; set; }
    }
}
