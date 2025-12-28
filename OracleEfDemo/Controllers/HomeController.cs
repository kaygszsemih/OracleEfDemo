using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OracleEfDemo.DbContext;
using OracleEfDemo.Dtos;
using OracleEfDemo.Helpers;
using OracleEfDemo.Models;
using System.Diagnostics;

namespace OracleEfDemo.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly GenericRepository _repository;
        public HomeController(AppDbContext context, IMapper mapper, GenericRepository repository)
        {
            _context = context;
            _mapper = mapper;
            _repository = repository;
        }

        public IActionResult Index()
        {
            var model = new DashboardDto
            {
                Customers = _repository.GetCustomerList<CustomerListDto>(),
                Orders = _context.Orders.Include(x => x.Customers).Include(x => x.OrderItems).Select(x => new OrderListDto
                {
                    Id = x.Id,
                    OrderNumber = x.OrderNumber,
                    CustomerName = x.Customers.FullName,
                    OrderDate = x.CreatedDate,
                    Total = x.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice)
                }).ToList()
            };

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
