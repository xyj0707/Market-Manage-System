using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCases.CategoriesUseCases;
using UseCases.ProductsUseCases;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [Authorize(Policy = "Cashiers")]
    public class SalesController : Controller
    {
        private readonly IViewCategoriesUseCase viewCategoriesUseCase;
        private readonly IViewSelectedProductUseCase viewSelectedProductUseCase;
        private readonly ISellProductUseCase sellProductUseCase;
        private readonly IViewProductsInCategoryUseCase viewProductsInCategoryUseCase;

        public SalesController(IViewCategoriesUseCase viewCategoriesUseCase,
            IViewSelectedProductUseCase viewSelectedProductUseCase,
            ISellProductUseCase sellProductUseCase,
            IViewProductsInCategoryUseCase viewProductsInCategoryUseCase)
        {
            this.viewCategoriesUseCase = viewCategoriesUseCase;
            this.viewSelectedProductUseCase = viewSelectedProductUseCase;
            this.sellProductUseCase = sellProductUseCase;
            this.viewProductsInCategoryUseCase = viewProductsInCategoryUseCase;
        }
        public IActionResult Index()
        {
            var salesViewModel = new SalesViewModel
            {
                Categories = viewCategoriesUseCase.Execute()
            };
            
            return View(salesViewModel);
        }

        public IActionResult SellProductPartial(int productId)
        {
            var product = viewSelectedProductUseCase.Execute(productId);
            return PartialView("_SellProduct",product);

        }

        public IActionResult Sell(SalesViewModel salesViewModel)
        {
            if(ModelState.IsValid)
            {
                //sell the product
                //var prod = ProductRepository.GetProductById(salesViewModel.SelectedProductId);
                //if(prod != null) 
                //{
                //    TransactionsRepository.Add(
                //        "Cashier1",
                //        salesViewModel.SelectedProductId,
                //        prod.Name,
                //        prod.Price.HasValue ? prod.Price.Value : 0,
                //        prod.Quantity.HasValue ? prod.Quantity.Value : 0,
                //        salesViewModel.QuantityToSell
                //        );
                //    prod.Quantity -= salesViewModel.QuantityToSell;
                //    ProductRepository.UpdateProduct(salesViewModel.SelectedProductId, prod);
                //}
                sellProductUseCase.Execute(
                    "cashier1",
                    salesViewModel.SelectedProductId,
                    salesViewModel.QuantityToSell);
            }
            var product = viewSelectedProductUseCase.Execute(salesViewModel.SelectedProductId);
            salesViewModel.SelectedCategoryId = (product?.CategoryId == null) ? 0 : product.CategoryId.Value;
            salesViewModel.Categories = viewCategoriesUseCase.Execute();
            return View("Index",salesViewModel);
        }

        public IActionResult ProductsByCategoryPartial(int categoryId)
        {
            var products = viewProductsInCategoryUseCase.Execute(categoryId);
            return PartialView("_Products", products);
        }
    }
}
