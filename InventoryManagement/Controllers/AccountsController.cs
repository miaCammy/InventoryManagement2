using InventoryManagement.Helper;
using InventoryManagement.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;

namespace InventoryManagement.Controllers
{
    public class AccountsController : Controller
    {
        private IConfiguration _config;
        CommonHelper _helper;

        public AccountsController(IConfiguration config)
        {
            _config = config;
            _helper = new CommonHelper(_config);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            string UserExistQuery = $"SELECT * FROM user WHERE UserName='{registerViewModel.UserName}'";
            bool userExists = _helper.UserExist(UserExistQuery);

            if(userExists == true)
            {
                ViewBag.Error = true;
                return View("Register", "Accounts");
            }

            string query = "INSERT INTO user (UserName, UserPassword, Name)" +
                           $"VALUES ('{registerViewModel.UserName}','{registerViewModel.UserPassword}','{registerViewModel.Name}')";

            int result = _helper.DMLTransaction(query);

            if(result > 0)
            {
                EntryIntoSession(registerViewModel.UserName);
                ViewBag.Success = true;
                return RedirectToAction("Login", "LoginPartial");
            }

            return View();
        }

        public void EntryIntoSession(string userName)
        {
            HttpContext.Session.SetString("UserName", userName);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
