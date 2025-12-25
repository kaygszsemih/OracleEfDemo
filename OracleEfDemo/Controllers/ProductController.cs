using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using OracleEfDemo.DbContext;
using OracleEfDemo.Dtos;
using OracleEfDemo.Models;

namespace OracleEfDemo.Controllers
{
    [Authorize]
    public class ProductController(AppDbContext context, IMapper mapper, IToastNotification toastNotification) : Controller
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IToastNotification _toastNotification = toastNotification;

        public IActionResult Categories()
        {
            var data = _context.Categories.ToList();
            return View(_mapper.Map<List<CategoriesDto>>(data));
        }

        public IActionResult AddorUpdateCategory(int id)
        {
            var data = new CategoriesDto();

            if (id > 0)
            {
                data = _mapper.Map<CategoriesDto>(_context.Categories.Find(id));
            }

            return View(data);
        }

        [HttpPost]
        public IActionResult AddorUpdateCategory(CategoriesDto model)
        {
            if (!ModelState.IsValid)
            {
                _toastNotification.AddWarningToastMessage("Eksik yada hatalı bilgi. Bilgileri kontrol edip tekrar deneyin.");
                return View();
            }

            if (model.Id > 0)
            {
                var category = _context.Categories.Find(model.Id);
                _mapper.Map(model, category);

                _context.Categories.Update(category!);

                _toastNotification.AddSuccessToastMessage("Kategori başarıyla güncellendi.");
            }
            else
            {
                var data = _mapper.Map<Categories>(model);
                _context.Categories.Add(data);

                _toastNotification.AddSuccessToastMessage("Yeni kategori başarıyla eklendi.");
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Categories));
        }

        public IActionResult RemoveCategory(int id)
        {
            var category = _context.Categories.Find(id);

            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();

                _toastNotification.AddSuccessToastMessage("Kategori silindi.");
                return RedirectToAction(nameof(Categories));
            }

            _toastNotification.AddWarningToastMessage("Kategori bulunamadı.");
            return RedirectToAction(nameof(Categories));
        }

        public IActionResult Products()
        {
            var data = _context.Products.ToList();
            return View(_mapper.Map<List<ProductsDto>>(data));
        }

        public IActionResult AddorUpdateProduct(int id)
        {
            var product = new ProductsDto();

            if (id > 0)
            {
                product = _mapper.Map<ProductsDto>(_context.Products.Find(id));
            }

            ViewData["Categories"] = _mapper.Map<List<CategoriesDto>>(_context.Categories.ToList());

            return View(product);
        }

        [HttpPost]
        public IActionResult AddorUpdateProduct(ProductsDto model)
        {
            if (!ModelState.IsValid)
            {
                _toastNotification.AddWarningToastMessage("Eksik yada hatalı bilgi. Bilgileri kontrol edip tekrar deneyin.");
                return View();
            }

            if (model.Id > 0)
            {
                var productData = _context.Products.Find(model.Id);

                if (productData == null)
                {
                    _toastNotification.AddWarningToastMessage("İlgili ürün bulunamadı.");
                    return RedirectToAction(nameof(Products));
                }

                _mapper.Map(model, productData);
                _context.Products.Update(productData);

                _toastNotification.AddSuccessToastMessage("Ürün başarıyla güncellendi.");
            }
            else
            {
                var data = _mapper.Map<Products>(model);
                _context.Products.Add(data);

                _toastNotification.AddSuccessToastMessage("Yeni ürün başarıyla eklendi.");
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Products));
        }

        public IActionResult RemoveProduct(int id)
        {
            var productData = _context.Products.Find(id);

            if (productData != null)
            {
                _context.Products.Remove(productData);
                _context.SaveChanges();

                _toastNotification.AddSuccessToastMessage("Ürün silindi.");
                return RedirectToAction(nameof(Products));
            }

            _toastNotification.AddWarningToastMessage("Ürün bulunamadı.");
            return RedirectToAction(nameof(Products));
        }

        [HttpGet]
        public IActionResult GetProductWithCategoryId(int categoryId)
        {
            var data = _context.Products.Where(x => x.CategoryId == categoryId)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.ProductName,
                    price = x.Price
                })
                .ToList();

            if (data.Count == 0)
            {
                return Json(new
                {
                    success = false,
                    message = "Bu kategoriye ait ürün bulunamadı."
                });
            }

            return Json(new
            {
                success = true,
                data
            });
        }
    }
}
