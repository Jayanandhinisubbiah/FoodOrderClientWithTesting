using CallingAPIInClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace CallingAPIInClient.Controllers
{
    public class CartsController : Controller
    {
        public async Task<IActionResult> AddtoCart(int? FoodId)
        {

            string UserId = HttpContext.Session.GetString("UserId");
            Food food = new();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7172/api/Carts/AddtoCart" + FoodId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    food = JsonConvert.DeserializeObject<Food>(apiResponse);
                }
            }
            return View(food);
        }
        //[HttpPost]
        //public async Task<IActionResult> AddtoCart(int Qnt, Food food)
        //{


        //    string UserId = HttpContext.Session.GetString("UserId");
        //    int b = int.Parse(UserId);
        //    using (var httpClient = new HttpClient())
        //    {


        //        StringContent content1 = new StringContent(JsonConvert.SerializeObject(Qnt, food.FoodId), Encoding.UTF8, "application/json");

        //        using (var response = await httpClient.PostAsync("https://localhost:7172/api/Carts", content1))
        //        {
        //            string apiResponse = await response.Content.ReadAsStringAsync();
        //            C = JsonConvert.DeserializeObject<Cart>(apiResponse);
        //        }
        //    }
        //    return RedirectToAction("ViewCart");
        //}
        [HttpPost]
        public async Task<IActionResult> AddtoCart(Cart C)
        {

            string UserId = HttpContext.Session.GetString("UserId");
            int b = int.Parse(UserId);
            C.UserId = b;
            C.Food = null;
            C.User = null;
            Cart cart = new Cart();

            using (var httpClient = new HttpClient())
            {

                StringContent content1 = new StringContent(JsonConvert.SerializeObject(C), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("https://localhost:7172/api/Carts/AddtoCart", content1))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    cart = JsonConvert.DeserializeObject<Cart>(apiResponse);
                }
            }
            return RedirectToAction("GetAllFoods", "Foods");
            //return RedirectToAction("ViewCart"/*, new { UserId = cart.UserId }*/);
        }
        public async Task<IActionResult> ViewCart()
        {

            string UserId = HttpContext.Session.GetString("UserId");
            int b = int.Parse(UserId);
            List<Cart> U = new List<Cart>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7172/api/Carts/ViewCart" + b))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    U = JsonConvert.DeserializeObject<List<Cart>>(apiResponse);
                }
            }
            return View(U);
        }
        [HttpPost]
        public async Task<IActionResult> Viewcart()
        {

            string UserId = HttpContext.Session.GetString("UserId");
            int b = int.Parse(UserId);
            //C.UserId = b;
            //C.Food = null;
            //C.User = null;
            Cart cart = new Cart();

            using (var httpClient = new HttpClient())
            {

                using (var response = await httpClient.GetAsync("https://localhost:7172/api/Carts/" + b))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    cart = JsonConvert.DeserializeObject<Cart>(apiResponse);
                }
            }
            return RedirectToAction("EmptyList");
        }
        [HttpGet]
        public async Task<IActionResult> EmptyList()
        {

            string UserId = HttpContext.Session.GetString("UserId");
            int b = int.Parse(UserId);
            Cart U = new Cart();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:7172/api/Carts/EmptyList" + b))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    U = JsonConvert.DeserializeObject<Cart>(apiResponse);
                }
            }
            return RedirectToAction("OrderDetails");
        }
        [HttpGet]
        public async Task<IActionResult> OrderDetails()
        {
            List<OrderDetails> OrderInfo = new List<OrderDetails>();
            // HttpClient client = new HttpClient();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("https://localhost:7172/api/Carts/OrderDetails");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var ProdResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the product list  
                    OrderInfo = JsonConvert.DeserializeObject<List<OrderDetails>>(ProdResponse);

                }
                //returning the product list to view  
                return RedirectToAction("Buy");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Buy()
        {
            string UserId = HttpContext.Session.GetString("UserId");
            int b = int.Parse(UserId);
            OrderMaster? om = new OrderMaster();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7172/api/Carts/Buy" + b))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    om = JsonConvert.DeserializeObject<OrderMaster>(apiResponse);
                }
            }
            return View(om);
        }
        [HttpPost]
        public async Task<IActionResult> Buy(int OrderId,OrderMaster O)
        {
           
            OrderMaster? o = new OrderMaster();

            using (var httpClient = new HttpClient())
            {

                StringContent content1 = new StringContent(JsonConvert.SerializeObject(O), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PutAsync("https://localhost:7172/api/Carts/Payment"+OrderId, content1))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    o = JsonConvert.DeserializeObject<OrderMaster>(apiResponse);
                }
            }
            if (o.Type == "Online")
            {

                return RedirectToAction("Online", new { OrderId = o.OrderId });
            }
            else
            {
                return RedirectToAction("Offline", new { OrderId = o.OrderId });
            }
        }
        [HttpGet]
        public async Task<IActionResult> Online(int OrderId)
        {
            string UserId = HttpContext.Session.GetString("UserId");
            int b = int.Parse(UserId);
            OrderMaster? om = new OrderMaster();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7172/api/Carts/On" + OrderId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    om = JsonConvert.DeserializeObject<OrderMaster>(apiResponse);
                }
            }
            return View(om);
        }
        [HttpPost]
        public async Task<IActionResult> Online(int OrderId, OrderMaster O)
        {

            OrderMaster? o = new OrderMaster();

            using (var httpClient = new HttpClient())
            {

                StringContent content1 = new StringContent(JsonConvert.SerializeObject(O), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PutAsync("https://localhost:7172/api/Carts/On" + OrderId, content1))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Thankyou","Home");
        }
            public async Task<IActionResult> Edit(int? CartId)
        {

            Cart C = new();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7172/api/Carts/EditCart" + CartId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    C = JsonConvert.DeserializeObject<Cart>(apiResponse);
                }
                HttpContext.Session.SetString("CartId", C.CartId.ToString());
                HttpContext.Session.SetString("UserId", C.UserId.ToString());
                HttpContext.Session.SetString("FoodId", C.FoodId.ToString());


            }
            return View(C);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int CartId,Cart C)
        {
            C.CartId = Convert.ToInt32(HttpContext.Session.GetString("CartId"));
            C.UserId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            C.FoodId = Convert.ToInt32(HttpContext.Session.GetString("FoodId"));

            //string UserId = HttpContext.Session.GetString("UserId");
            //int b = int.Parse(UserId);
            //C.UserId = b;
            C.Food = null;
            C.User = null;
            Cart cart = new Cart();

            using (var httpClient = new HttpClient())
            {

                StringContent content1 = new StringContent(JsonConvert.SerializeObject(C), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PutAsync("https://localhost:7172/api/Carts/Edit" + CartId, content1))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    cart = JsonConvert.DeserializeObject<Cart>(apiResponse);
                }
            }
            return RedirectToAction("ViewCart");
        }
        public async Task<IActionResult> Delete(int? CartId)
        {

            Cart C = new();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7172/api/Carts/DCart" + CartId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    C = JsonConvert.DeserializeObject<Cart>(apiResponse);
                }
            }
            return View(C);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCart(int CartId, Cart C)
        {
            string UserId = HttpContext.Session.GetString("UserId");
            int b = int.Parse(UserId);
            C.UserId = b;
            C.Food = null;
            C.User = null;
            Cart cart = new Cart();

            using (var httpClient = new HttpClient())
            {

                using (var response = await httpClient.DeleteAsync("https://localhost:7172/api/Carts/Delete" + CartId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    cart = JsonConvert.DeserializeObject<Cart>(apiResponse);
                }
            }
            return RedirectToAction("ViewCart");
        }
        [HttpGet]
        public async Task<IActionResult> Offline(int OrderId)
        {
            string UserId = HttpContext.Session.GetString("UserId");
            int b = int.Parse(UserId);
            OrderMaster? om = new OrderMaster();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7172/api/Carts/On" + OrderId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    om = JsonConvert.DeserializeObject<OrderMaster>(apiResponse);
                }
            }
            return View(om);
        }
        [HttpPost]
        public async Task<IActionResult> Offline(int OrderId, OrderMaster O)
        {

            OrderMaster? o = new OrderMaster();

            using (var httpClient = new HttpClient())
            {

                StringContent content1 = new StringContent(JsonConvert.SerializeObject(O), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PutAsync("https://localhost:7172/api/Carts/On" + OrderId, content1))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Thankyou","Home");
        }
    }

}

