using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using OracleEfDemo.DbContext;
using OracleEfDemo.Dtos;
using OracleEfDemo.Helpers;
using OracleEfDemo.Models;

namespace OracleEfDemo.Controllers
{
    [Authorize]
    public class OrderController(AppDbContext context, IMapper mapper, IToastNotification toastNotification, GenericRepository repository) : Controller
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IToastNotification _toastNotification = toastNotification;
        private readonly GenericRepository _repository = repository;

        public IActionResult Orders()
        {
            var data = _context.Orders.Include(x => x.Customers).Include(x => x.OrderItems).Select(x => new OrderListDto
            {
                Id = x.Id,
                OrderNumber = x.OrderNumber,
                CustomerName = x.Customers.FullName,
                OrderDate = x.CreatedDate,
                Total = x.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice)
            }).ToList();

            return View(data);
        }

        public IActionResult AddorUpdateOrder(int id)
        {
            var orders = new OrdersDto();

            if (id > 0)
            {
                orders = _mapper.Map<OrdersDto>(_context.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Products).Where(x => x.Id == id).FirstOrDefault());
            }

            ViewData["Categories"] = _mapper.Map<List<CategoriesDto>>(_context.Categories.ToList());
            ViewData["Customer"] = _mapper.Map<List<CustomersDto>>(_context.Customers.ToList());

            return View(orders);
        }

        [HttpPost]
        public IActionResult AddorUpdateOrder(OrdersDto model)
        {
            if (!ModelState.IsValid)
            {
                _toastNotification.AddWarningToastMessage("Eksik yada hatalı bilgi. Bilgileri kontrol edip tekrar deneyin.");
                return RedirectToAction(nameof(Orders));
            }

            _repository.SetOracleUserName(User.GetUserName());

            if (model.Id > 0)
            {
                var orderData = _context.Orders.Include(x => x.OrderItems).First(x => x.Id == model.Id);

                if (orderData == null)
                {
                    _toastNotification.AddWarningToastMessage("Sipariş bulunamadı.");
                    return RedirectToAction(nameof(Orders));
                }

                foreach (var item in orderData.OrderItems)
                {
                    var modelQuantity = model.OrderItemsDto.FirstOrDefault(x => x.ProductId == item.ProductId)?.Quantity ?? 0;
                    var quantityDifference = item.Quantity - modelQuantity;

                    if (quantityDifference != 0)
                    {
                        var (result, message) = _repository.TryUpdateStock(item.ProductId, quantityDifference);

                        if (!result)
                        {
                            _toastNotification.AddWarningToastMessage("Seçilen ürünlerin içinde stoğu eksiye düşecek ürünler vardır. Lütfen kontrol ediniz.");
                            return RedirectToAction(nameof(Orders));
                        }
                    }
                }

                _mapper.Map(model, orderData);
                _context.Orders.Update(orderData);

                _toastNotification.AddSuccessToastMessage("Sipariş başarıyla güncellendi.");
            }
            else
            {
                foreach (var item in model.OrderItemsDto)
                {
                    var (result, message) = _repository.TryUpdateStock(item.ProductId, -1 * item.Quantity);

                    if (!result)
                    {
                        _toastNotification.AddWarningToastMessage("Seçilen ürünlerin içinde stoğu eksiye düşecek ürünler vardır. Lütfen kontrol ediniz.");
                        return RedirectToAction(nameof(Orders));
                    }
                }

                var data = _mapper.Map<Orders>(model);
                _context.Orders.Add(data);

                _toastNotification.AddSuccessToastMessage("Yeni sipariş başarıyla eklendi.");
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Orders));
        }

        public IActionResult RemoveOrder(int id)
        {
            var orderData = _context.Orders.Find(id);

            if (orderData != null)
            {
                _context.Orders.Remove(orderData);
                _context.SaveChanges();

                _toastNotification.AddSuccessToastMessage("Sipariş silindi.");
                return RedirectToAction(nameof(Orders));
            }

            _toastNotification.AddWarningToastMessage("Sipariş bulunamadı.");
            return RedirectToAction(nameof(Orders));
        }

        [HttpGet]
        public IActionResult CustomerOrders(int customerId)
        {
            var orders = _repository.GetCustomerOrders(customerId);

            return PartialView("_CustomerOrdersPartial", orders);
        }
    }
}
