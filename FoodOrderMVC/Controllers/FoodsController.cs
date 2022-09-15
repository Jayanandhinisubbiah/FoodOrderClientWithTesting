using CallingAPIInClient.Models;
using CallingAPIInClient.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Hosting;
namespace CallingAPIInClient.Controllers
{
    public class FoodsController : Controller
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        
        public FoodsController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            this._environment = hostingEnvironment;
        }
        //public async Task<IActionResult> GetAllFoods(string? SearchText)
        //{
        //    List<Food> ProductInfo = new List<Food>();
        //    // HttpClient client = new HttpClient();
        //    HttpResponseMessage Res;
        //    using (var client = new HttpClient())
        //    {
        //        //Passing service base url  
        //        client.DefaultRequestHeaders.Clear();
        //        //Define request data format  
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        //Sending request to find web api REST service resource GetAllEmployees using HttpClient
        //        //if (SearchText != "" && SearchText != null)
        //        //{
        //        //     Res = await client.GetAsync("https://localhost:7172/api/Foods");
        //        //}
        //        //else
        //        //{
        //             Res = await client.GetAsync("https://localhost:7172/api/Foods");

        //        //}
        //        //Checking the response is successful or not which is sent using HttpClient  
        //        if (Res.IsSuccessStatusCode)
        //        {
        //            //Storing the response details recieved from web api   
        //            var ProdResponse = Res.Content.ReadAsStringAsync().Result;

        //            //Deserializing the response recieved from web api and storing into the product list  
        //            ProductInfo = JsonConvert.DeserializeObject<List<Food>>(ProdResponse);

        //        }
        //        //returning the product list to view  
        //        return View(ProductInfo);
        //    }

        //}
        public async Task<IActionResult> AddFood()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddFood(FoodViewModel food)
        {
            //string UserId = HttpContext.Session.GetString("UserId");
            //int b = int.Parse(UserId);
            //C.UserId = b;
            //C.Food = null;
            //C.User = null;
            //FoodViewModel f = new FoodViewModel();
            string uniqueFileName = ProcessUploadedFile(food);
            Food j = new Food
            {
                FoodName = food.FoodName,
                CategoryName = food.CategoryName,
                price = food.price,
                Image = uniqueFileName,
                Detail = food.Detail

            };
            using (var httpClient = new HttpClient())
            {

                StringContent content1 = new StringContent(JsonConvert.SerializeObject(j), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("https://localhost:7172/api/Foods", content1))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    j = JsonConvert.DeserializeObject<Food>(apiResponse);
                }
            }
            return RedirectToAction("Index");

        }

        private string ProcessUploadedFile(FoodViewModel food)
        {
            string uniqueFileName = null;
            if (food.ImageView != null)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "Image");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + food.ImageView.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream= new FileStream(filePath, FileMode.Create))
                    food.ImageView.CopyTo(fileStream);
            }

            return uniqueFileName;
        }

        public async Task<IActionResult> Index()
        {
            List<Food> ProductInfo = new List<Food>();
            // HttpClient client = new HttpClient();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("https://localhost:7172/api/Foods");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var ProdResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the product list  
                    ProductInfo = JsonConvert.DeserializeObject<List<Food>>(ProdResponse);

                }
                //returning the product list to view  
                return View(ProductInfo);
            }
        }

        public async Task<IActionResult> Edit(int? FoodId)
        {

            Food C = new();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7172/api/Foods/" + FoodId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    C = JsonConvert.DeserializeObject<Food>(apiResponse);
                }



            }
            FoodEditViewModel foodEditViewModel = new FoodEditViewModel
            {
                FoodId = C.FoodId,
                CategoryName = C.CategoryName,
                FoodName = C.FoodName,
                price = C.price,
                Detail = C.Detail,
                ExistingPhotoPath = C.Image
            };
            return View(foodEditViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(FoodEditViewModel model)
        {

            

                Food cart = new Food();
            cart.FoodId = model.FoodId;
            cart.CategoryName = model.CategoryName;
            cart.FoodName = model.FoodName;
            cart.price = model.price;
            cart.Detail = model.Detail;
            if(model.ImageView!=null)
            {
                if(model.ExistingPhotoPath!=null)
                {
                    string filePath = Path.Combine(_environment.WebRootPath, "Image", model.ExistingPhotoPath);
                    System.IO.File.Delete(filePath);
                }
                cart.Image = ProcessUploadedFile(model);
            }

                using (var httpClient = new HttpClient())
                {

                    StringContent content1 = new StringContent(JsonConvert.SerializeObject(cart), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync("https://localhost:7172/api/Foods/" + model.FoodId, content1))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        cart = JsonConvert.DeserializeObject<Food>(apiResponse);
                    }
                }
                return RedirectToAction("Index");
            }
        
    
        public async Task<IActionResult> Delete(int? FoodId)
        {

            Food C = new();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7172/api/Foods/" + FoodId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    C = JsonConvert.DeserializeObject<Food>(apiResponse);
                }
            }
            return View(C);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteFood(int FoodId)
        {

            Food food = new Food();

            using (var httpClient = new HttpClient())
            {

                using (var response = await httpClient.DeleteAsync("https://localhost:7172/api/Foods/" + FoodId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    food = JsonConvert.DeserializeObject<Food>(apiResponse);
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int? FoodId)
        {

            Food food = new();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7172/api/Foods/" + FoodId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    food = JsonConvert.DeserializeObject<Food>(apiResponse);
                }
            }
            return View(food);
        }
        public async Task<IActionResult> NewOrder()
        {
            List<NewOrder> NewOrderInfo = new List<NewOrder>();
            // HttpClient client = new HttpClient();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("https://localhost:7172/api/Foods/NewOrder");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var ProdResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the product list  
                    NewOrderInfo = JsonConvert.DeserializeObject<List<NewOrder>>(ProdResponse);

                }
                //returning the product list to view  
                return View(NewOrderInfo);
            }
        }
        //public async Task<IActionResult> Dispatch(int Id)
        //{

            //NewOrder C = new();
            //using (var httpClient = new HttpClient())
            //{
            //    using (var response = await httpClient.GetAsync("https://localhost:7172/api/Foods/DispatchNewOrder" + Id))
            //    {
            //        string apiResponse = await response.Content.ReadAsStringAsync();
            //        C = JsonConvert.DeserializeObject<NewOrder>(apiResponse);
            //    }
            //}
            //HttpContext.Session.SetString("OrderId", C.OrderId.ToString());

            //return View(C);
        //}
        //[HttpPost]
        public async Task<IActionResult> DispatchConfirmed(int Id)
        {
            NewOrder C = new();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7172/api/Foods/DispatchNewOrder" + Id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    C = JsonConvert.DeserializeObject<NewOrder>(apiResponse);
                }
            }
            HttpContext.Session.SetString("OrderId", C.OrderId.ToString());


            string OrderId = HttpContext.Session.GetString("OrderId");
            int b = int.Parse(OrderId);
            //List<NewOrder> food = new List<NewOrder>();

            using (var httpClient = new HttpClient())
            {

                using (var response = await httpClient.DeleteAsync("https://localhost:7172/api/Foods/DispatchOrder" + Id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    //food = JsonConvert.DeserializeObject<List<NewOrder>>(apiResponse);
                }
            }
            return RedirectToAction("EmptyOrder", new { OrderId = b });
        }
        //
        [HttpGet]
        public async Task<IActionResult> EmptyOrder(int OrderId)
        {

            string UserId = HttpContext.Session.GetString("OrderId");
            int b = int.Parse(UserId);
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:7172/api/Foods/EmptyOrder" + OrderId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("NewOrder");
        }


    }
}
