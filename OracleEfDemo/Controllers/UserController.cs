using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using OracleEfDemo.DbContext;

namespace OracleEfDemo.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;

        public UserController(AppDbContext context, IMapper mapper, IToastNotification toastNotification)
        {
            _context = context;
            _mapper = mapper;
            _toastNotification = toastNotification;
        }


    }
}
