using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Oracle.ManagedDataAccess.Client;
using OracleEfDemo.DbContext;
using OracleEfDemo.Dtos;
using OracleEfDemo.Helpers;
using OracleEfDemo.Models;
using System.Globalization;

namespace OracleEfDemo.Controllers
{
    [Authorize]
    public class ProductController(AppDbContext context, IMapper mapper, IToastNotification toastNotification, GenericRepository repository) : Controller
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IToastNotification _toastNotification = toastNotification;
        private readonly GenericRepository _repository = repository;

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

            var checkCategoryName = _context.Categories.Any(x => x.CategoryName == model.CategoryName);

            if (checkCategoryName)
            {
                _toastNotification.AddErrorToastMessage("Aynı isime ait başka bir kayıt mevcut.");
                return View(model);
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

            if (category == null)
            {
                _toastNotification.AddErrorToastMessage("Kategori bulunamadı.");
                return RedirectToAction(nameof(Categories));
            }

            try
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();

                _toastNotification.AddSuccessToastMessage("Kategori silindi.");
                return RedirectToAction(nameof(Categories));
            }
            catch (DbUpdateException ex) when (ex.GetBaseException() is OracleException oex)
            {
                if (oex.Number == 2292)
                {
                    _toastNotification.AddErrorToastMessage("Bu kategoriye bağlı kayıtlar olduğu için silinemez.");
                    return RedirectToAction(nameof(Categories));
                }

                throw;
            }
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

            var checkProductName = _context.Products.Any(x => x.ProductName == model.ProductName);

            if (checkProductName)
            {
                _toastNotification.AddErrorToastMessage("Aynı isime ait başka bir kayıt mevcut.");
                return View(model);
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
            var data = _context.Products.Where(x => x.CategoryId == categoryId && x.IsActive)
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

        [HttpPost]
        public IActionResult UpdateProductStock(int productId, string newStockQuantity)
        {
            var raw = (newStockQuantity ?? "").Trim();

            if (!decimal.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out var qty))
            {
                _toastNotification.AddErrorToastMessage("Geçersiz stok miktarı.");
                return RedirectToAction(nameof(Products));
            }

            _repository.SetOracleUserName(User.GetUserName());
            var (result, message) = _repository.TryUpdateStock(productId, qty);

            if (result)
                _toastNotification.AddSuccessToastMessage(message);
            else
                _toastNotification.AddErrorToastMessage(message);

            return RedirectToAction(nameof(Products));
        }
    }
}
