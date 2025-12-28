using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using OracleEfDemo.DbContext;
using OracleEfDemo.Dtos;
using OracleEfDemo.Helpers;
using OracleEfDemo.Models;
using System.Globalization;

namespace OracleEfDemo.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;
        private readonly UserManager<UserApp> _userManager;
        private readonly GenericRepository _repository;

        public UserController(AppDbContext context, IMapper mapper, IToastNotification toastNotification, UserManager<UserApp> userManager, GenericRepository repository)
        {
            _context = context;
            _mapper = mapper;
            _toastNotification = toastNotification;
            _userManager = userManager;
            _repository = repository;
        }

        public IActionResult Users()
        {
            var users = _context.Users.Select(x => new UserDto
            {
                Id = x.Id,
                FullName = x.FullName,
                PhoneNumber = x.PhoneNumber!,
                Email = x.Email!,
                Department = x.Department,
                Salary = x.Salary,
                CreatedDate = x.CreatedDate
            }).ToList();

            return View(users);
        }

        public async Task<IActionResult> AddorUpdateUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                return View();

            var data = await _userManager.FindByIdAsync(id);

            if (data == null)
            {
                _toastNotification.AddWarningToastMessage("Kullanıcı bulunamadı");
                return RedirectToAction(nameof(Users));
            }

            var mapData = _mapper.Map<UserDto>(data);

            return View(mapData);
        }

        [HttpPost]
        public async Task<IActionResult> AddorUpdateUser(UserDto model)
        {
            if (!ModelState.IsValid)
            {
                _toastNotification.AddWarningToastMessage("Eksik yada hatalı bilgi.");
                return View(model);
            }

            var checkUserName = _userManager.Users.Any(x => x.UserName == model.Email || x.Email == model.Email);

            if(checkUserName)
            {
                _toastNotification.AddErrorToastMessage("Bu e-posta adresine ait başka bir kayıt mevcut.");
                return View(model);
            }

            var result = new IdentityResult();

            if (string.IsNullOrEmpty(model.Id))
            {
                var data = new UserApp
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Department = model.Department,
                    Salary = model.Salary.GetValueOrDefault(),
                    CreatedDate = DateTime.Now
                };

                result = await _userManager.CreateAsync(data, "User123.");

                if (!result.Succeeded)
                {
                    _toastNotification.AddErrorToastMessage("Yeni kullanıcı oluşuturulurken hata meydana geldi.");
                    ModelState.AddModelError(string.Empty, string.Join("----", result.Errors.Select(e => e.Description)));
                    return View(model);
                }

                _toastNotification.AddSuccessToastMessage("Yeni kullanıcı başarıyla eklendi.");
                return RedirectToAction(nameof(Users));
            }

            var user = await _userManager.FindByIdAsync(model.Id);

            if (user != null)
            {
                var mapData = _mapper.Map(model, user);

                mapData.UpdatedDate = DateTime.Now;
                result = await _userManager.UpdateAsync(mapData);

                if (!result.Succeeded)
                {
                    _toastNotification.AddErrorToastMessage("Kullanıcı güncellenirken hata meydana geldi.");
                    ModelState.AddModelError(string.Empty, string.Join("----", result.Errors.Select(e => e.Description)));
                    return View(model);
                }

                _toastNotification.AddSuccessToastMessage("Yeni kullanıcı başarıyla güncellendi.");
                return RedirectToAction(nameof(Users));
            }

            _toastNotification.AddWarningToastMessage("Eksik yada hatalı bilgi.");
            return View(model);
        }

        public async Task<IActionResult> RemoveUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                _toastNotification.AddWarningToastMessage("Kullanıcı bulunamadı");
                return RedirectToAction(nameof(Users));
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                _toastNotification.AddErrorToastMessage("Kullanıcı silinirken hata meydana geldi.");
                return RedirectToAction(nameof(Users));
            }

            _toastNotification.AddSuccessToastMessage("Yeni kullanıcı başarıyla güncellendi.");
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public IActionResult UpdateEmployeesSalary(string salaryRate)
        {
            var raw = (salaryRate ?? "").Trim();

            if (!decimal.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out var rate))
            {
                _toastNotification.AddErrorToastMessage("Geçersiz zam oranı.");
                return RedirectToAction(nameof(Users));
            }

            var (totalSalary, result, message) = _repository.TryUpdateEmployeesSalary(rate);

            if (result)
                _toastNotification.AddSuccessToastMessage(message);
            else
                _toastNotification.AddErrorToastMessage(message);

            return RedirectToAction(nameof(Users));
        }
    }
}
