using InventoryManagement.Helper;
using InventoryManagement.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace InventoryManagement.Controllers
{
    public class LoginPartialController : Controller
    {
        private IConfiguration _config;
        CommonHelper _helper;

        public string Message { get; set; } = string.Empty;

        public LoginPartialController(IConfiguration config)
        {
            _config = config;
            _helper = new CommonHelper(_config);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if(string.IsNullOrEmpty(loginViewModel.UserName) && string.IsNullOrEmpty(loginViewModel.UserPassword))
            {
                return View();
            }
            else
            {
                bool userIsExist = SignInMethod(loginViewModel.UserName, loginViewModel.UserPassword);

                if(userIsExist == true)
                {
                    return RedirectToAction("InventoryList", "Inventory");
                    //add on path to go to the page that have access to inventory
                }

                return View();
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        private bool SignInMethod(string UserName, string UserPassword)
        {
            bool flag = false;

            string query = $"SELECT * FROM user WHERE UserName='{UserName}' AND UserPassword='{UserPassword}'";
            var userDetails = _helper.GetUserByUserName(query);
            if(userDetails.UserName != null)
            {
                flag = true;
                HttpContext.Session.SetString("UserName", UserName);
            }
            else
            {
                ViewBag.Error = false;
            }

            return flag;
        }
    }
}
