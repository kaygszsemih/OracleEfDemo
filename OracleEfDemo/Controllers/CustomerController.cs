using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Oracle.ManagedDataAccess.Client;
using OracleEfDemo.DbContext;
using OracleEfDemo.Dtos;
using OracleEfDemo.Helpers;
using OracleEfDemo.Models;

namespace OracleEfDemo.Controllers
{
    [Authorize]
    public class CustomerController(AppDbContext context, IMapper mapper, IToastNotification toastNotification, GenericRepository repository) : Controller
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IToastNotification _toastNotification = toastNotification;
        private readonly GenericRepository _repository = repository;

        public IActionResult Customers()
        {
            return View(_repository.GetCustomerList<CustomerListDto>());
        }

        public IActionResult AddorUpdateCustomer(int id)
        {
            var customer = new CustomersDto();

            if (id > 0)
            {
                customer = _mapper.Map<CustomersDto>(_context.Customers.Find(id));
            }

            return View(customer);
        }

        [HttpPost]
        public IActionResult AddorUpdateCustomer(CustomersDto model)
        {
            if (!ModelState.IsValid)
            {
                _toastNotification.AddWarningToastMessage("Eksik yada hatalı bilgi. Bilgileri kontrol edip tekrar deneyin.");
                return View();
            }

            if (model.Id > 0)
            {
                var customerData = _context.Customers.Find(model.Id);

                if (customerData == null)
                {
                    _toastNotification.AddWarningToastMessage("Müşteri bulunamadı.");
                    return RedirectToAction(nameof(Customers));
                }

                _mapper.Map(model, customerData);
                _context.Customers.Update(customerData);

                _toastNotification.AddSuccessToastMessage("Müşteri başarıyla güncellendi.");
            }
            else
            {
                var data = _mapper.Map<Customers>(model);
                _context.Customers.Add(data);

                _toastNotification.AddSuccessToastMessage("Yeni müşteri başarıyla eklendi.");
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Customers));
        }

        public IActionResult RemoveCustomer(int id)
        {
            var customerData = _context.Customers.Find(id);

            if (customerData == null)
            {
                _toastNotification.AddWarningToastMessage("Müşteri bulunamadı.");
                return RedirectToAction(nameof(Customers));
            }

            try
            {
                _context.Customers.Remove(customerData);
                _context.SaveChanges();

                _toastNotification.AddSuccessToastMessage("Müşteri silindi.");
                return RedirectToAction(nameof(Customers));
            }
            catch (DbUpdateException ex) when (ex.GetBaseException() is OracleException oex)
            {
                if (oex.Number == 2292)
                {
                    _toastNotification.AddErrorToastMessage("Bu müşteriye ait sipariş kayıtları olduğu için silinemez.");
                    return RedirectToAction(nameof(Customers));
                }

                throw;
            }
        }
    }
}
