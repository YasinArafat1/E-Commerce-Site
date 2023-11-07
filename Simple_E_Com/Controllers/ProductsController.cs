using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Simple_E_Com.Models;
using Simple_E_Com.Models.ViewModels;

namespace Simple_E_Com.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ShopDbContext db;
        private readonly IWebHostEnvironment _he;

        public ProductsController(ShopDbContext db, IWebHostEnvironment he)
        {
            this.db = db;
            _he = he;

        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
              return db.Products != null ? 
                          View(await db.Products.ToListAsync()) :
                          Problem("Entity set 'ShopDbContext.Products'  is null.");
        }

        public async  Task<IActionResult> Create()
        {

            return View();
        }


        [HttpPost]
        public async  Task<IActionResult> Create(ProductVM productVM)
        {


            if (ModelState.IsValid)
            {
                Product product = new Product()
                {
                    Name = productVM.Name,
                    Unit = productVM.Unit,
                    Price = productVM.Price,
                    Quantity = productVM.Quantity,
                };
                //form image 
                var file = productVM.PictureFile;
                string webroot = _he.WebRootPath;
                string folder = "Images";
                string imgFileName = Path.GetFileName(productVM.PictureFile.FileName);
                string fileToSave = Path.Combine(webroot, folder, imgFileName);


                if (file != null)
                {
                    using (var stream = new FileStream(fileToSave, FileMode.Create))
                    {
                        productVM.PictureFile.CopyTo(stream);
                        product.Image = "/" + folder + "/" + imgFileName;

                    }
                }
                db.Products.Add(product);
                await  db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
                  

          

            return View(productVM);
        }



    }
}
