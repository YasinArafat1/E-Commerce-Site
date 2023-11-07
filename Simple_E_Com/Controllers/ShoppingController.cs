using Microsoft.AspNetCore.Mvc;
using Simple_E_Com.Models;

namespace Simple_E_Com.Controllers
{
    public class ShoppingController : Controller
    {
        private ShopDbContext db;
        public ShoppingController(ShopDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            ViewBag.msg = TempData["msg"];
            return View(db.Products.ToList());
            
        }


        public IActionResult AddToCart(int pid, double qty)
        {
            bool itemFound = false;
            string msg = "";
            if (pid > 0 && qty > 0)
            {
                var prod = db.Products.FirstOrDefault(x => x.Id == pid);
                if (prod != null)
                {
                    List<Product> items = new List<Product>();
                    var xItems = HttpContext.Session.GetObject<List<Product>>("cart");
                    if (xItems != null)
                    {
                        foreach (var item in xItems)
                        {
                            if (pid == item.Id)
                            {
                                item.Quantity += qty;
                                itemFound = true;
                                msg = "Item Already Added,new quantity is added!";
                            }
                            items.Add(item);
                        }
                        if (!itemFound)
                        {
                            prod.Quantity = qty;
                            items.Add(prod);
                            msg = "new item is Added with Existing items ";
                        }
                        HttpContext.Session.SetObject<List<Product>>("cart", items);

                    }
                    else
                    {
                        prod.Quantity = qty;
                        items.Add(prod);
                        HttpContext.Session.SetObject<List<Product>>("cart", items);
                        msg = "new item is added to empty card";
                    }
                }
                else
                {
                    msg = "Item Not Found";
                }

            }
            else
            {
                msg = "Item id or qty could not be Zero";
            }
            TempData["msg"] = msg;
            return RedirectToAction(nameof(Index));



        }


        public IActionResult ShowCart()
        {
            List<Product> items = HttpContext.Session.GetObject<List<Product>>("cart");
            if (items != null && items.Count() != 0)
            {
                return View(items.ToList());
            }
            else
            {
                items = new List<Product>();
                return View();
            }

        }


        public IActionResult RemoveFromCart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            List<Product> productList = HttpContext.Session.GetObject<List<Product>>("cart");
            var removeItem = productList.FirstOrDefault(x => x.Id == id);
            productList.Remove(removeItem);
            HttpContext.Session.SetObject("cart", productList);
            return RedirectToAction(nameof(ShowCart));
        }



        public IActionResult CheckOut()
        {
            var us = HttpContext.Session.GetString("un");
            var id = HttpContext.Session.GetString("id");

            if (us!=null)
            {
                return RedirectToAction(nameof(ConfirmOrder));
            }
            else
            {
                return View(nameof(Login));
            }


           
        }

        public IActionResult ConfirmOrder()
        {

            return View();
        }

        public IActionResult Login(AppUser appUser)
        {
            var userName = db.AppUsers.FirstOrDefault(x => x.UserName == appUser.UserName);
            var password = db.AppUsers.FirstOrDefault(x=>x.Password== appUser.Password);
            if (userName != null && password != null)
            {
                HttpContext.Session.SetString("un", appUser.UserName);
                HttpContext.Session.SetString("id",appUser.Password);

                return RedirectToAction(nameof(ConfirmOrder));
            
            }
            else
            {
                TempData["wrongInfo"] = "Wrong Information";
                return View(appUser);
            }


            
        }


        public IActionResult SignUp()
        {


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([Bind("UserName","Password")] AppUser appUser)
        {
            string msg = "";
            if (ModelState.IsValid)
            {
                appUser.Role = 1;
                appUser.IsActive = 1;

                var checkUserName = db.AppUsers.FirstOrDefault(x => x.UserName.ToLower() == appUser.UserName.ToLower());
                if (checkUserName!=null)
                {
                    TempData["signUp"] = "User Already Exists ";

                }
                db.AppUsers.Add(appUser);
                await db.SaveChangesAsync();


                HttpContext.Session.SetString("un",appUser.UserName);
                HttpContext.Session.SetString("id",appUser.Password);

                appUser.UserName = null;
                appUser.Password = null;

                TempData["signUp"] = "successfully Created A/c";
                return RedirectToAction(nameof(Login));

            }
            else
            {
                msg = "Invalid UserName or Password";
                return View(appUser);
            }

            
        }



    }
}
