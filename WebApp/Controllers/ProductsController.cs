using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            var products = ProductRepository.GetProducts(loadCategory:true);
            return View(products);
        }

        public IActionResult Add()
        {
            ViewBag.Action = "add";
            var productViewModel = new ProductViewModel
            {
                Categories = CategoriesRepository.GetCategories()

        };
            
            return View(productViewModel);
        }
        [HttpPost]
        public IActionResult Add(ProductViewModel productViewModel)
        {
            if(ModelState.IsValid)
            {
                ProductRepository.AddProduct(productViewModel.Product);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Action = "add";
            productViewModel.Categories = CategoriesRepository.GetCategories();
            return View(productViewModel);
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Action = "edit";
            var productViewModel = new ProductViewModel
            {
                Product = ProductRepository.GetProductById(id)??new Product(),
                Categories = CategoriesRepository.GetCategories()
            };
            return View(productViewModel);
        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel productViewModel)
        {

            if(ModelState.IsValid)
            {
                ProductRepository.UpdateProduct(productViewModel.Product.ProductId, productViewModel.Product);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Action = "edit";
            productViewModel.Categories = CategoriesRepository.GetCategories();
            return View(productViewModel);           
        }

        public IActionResult Delete(int id)
        {
            ProductRepository.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ProductsByCategoryPartial(int categoryId)
        {
            var products = ProductRepository.GetProductsByCategoryId(categoryId);
            return PartialView("_Products", products);
        }

        
    }
}
