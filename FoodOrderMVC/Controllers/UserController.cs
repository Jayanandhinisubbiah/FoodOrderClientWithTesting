using CallingAPIInClient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace CallingAPIInClient.Controllers
{
    public class UserController : Controller
    {
        // GET: UserController
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<UserList> UserInfo = new List<UserList>();
            // HttpClient client = new HttpClient();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("https://localhost:7172/api/User");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var ProdResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the product list  
                    UserInfo = JsonConvert.DeserializeObject<List<UserList>>(ProdResponse);

                }

                //returning the product list to view
                return View(UserInfo);
            }
        }

        // GET: UserController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: UserController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: UserController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: UserController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: UserController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: UserController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: UserController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        public async Task<IActionResult> ViewReport(int UserId)
        {
            List<Content> U = new List<Content>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7172/api/User/ViewReport" + UserId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    U = JsonConvert.DeserializeObject<List<Content>>(apiResponse);
                }
            }
            return View(U);
        }
    }
}
