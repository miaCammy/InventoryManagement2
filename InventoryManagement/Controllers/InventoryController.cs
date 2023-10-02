using InventoryManagement.Helper;
using InventoryManagement.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace InventoryManagement.Controllers
{
    public class InventoryController : Controller
    {
        private IConfiguration _config;
        CommonHelper _helper;

        public InventoryController(IConfiguration config)
        {
            _config = config;
            _helper = new CommonHelper(_config);
        }

        #region Inventory(NewItem)
        [HttpGet]
        public IActionResult Inventory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Inventory(InventoryViewModel inventoryViewModel)
        {
            string newItemToAdd = "INSERT INTO inventory (ItemName, ItemDescription, ItemSku, ItemQuantity, ItemMinimumQuantity)" +
                                  $"VALUES ('{inventoryViewModel.ItemName}','{inventoryViewModel.ItemDescription}','{inventoryViewModel.ItemSku}','{inventoryViewModel.ItemQuantity}','{inventoryViewModel.ItemMinimumQuantity}')";

            int result = _helper.DMLTransaction(newItemToAdd);

            if (result > 0)
            {
                return RedirectToAction("InventoryList", "Inventory");
            }

            return View();
        }
        #endregion

        #region InventoryList
        [HttpGet]
        public IActionResult InventoryList()
        {
            List<InventoryViewModel> inventoryList = new List<InventoryViewModel>();
            List<InventoryViewModel> inventoryMinValue = new List<InventoryViewModel>();

            string query = "SELECT * FROM inventory";
            inventoryList = _helper.GetInventoryItem(query);

            //bind the data to the view
            ViewData.Model = inventoryList;

            return View();
        }
        #endregion

        #region CheckIn
        [HttpGet]
        public IActionResult CheckIn(int id)
        {
            ViewData.Model = retrieveInventory(id);
            return View();
        }

        [HttpPost]
        public IActionResult CheckInQuantity(IFormCollection form)
        {
            int Id = Convert.ToInt32(form["Id"].ToString());
            int checkInAmount = Convert.ToInt32(form["CheckInAmount"].ToString());
            
            //find the item 1st
            InventoryViewModel tempItem = new InventoryViewModel();
            tempItem = retrieveInventory(Id);

            int currentItemAmount = tempItem.ItemQuantity;
            int totalCheckIn = currentItemAmount + checkInAmount; //to add on the current qty with checkin qty

            string checkInQuery = $"UPDATE inventory SET ItemQuantity='{totalCheckIn}' WHERE Id='{Id}'";

            int result = _helper.DMLTransaction(checkInQuery);

            if (result > 0)
            {
                return RedirectToAction("InventoryList", "Inventory");
            }

            return View();
        }
        #endregion

        #region CheckOut
        [HttpGet]
        public IActionResult CheckOut(int id)
        {
            ViewData.Model = retrieveInventory(id);

            return View();
        }

        [HttpPost]
        public IActionResult CheckOutQuantity(IFormCollection form)
        {
            int Id = Convert.ToInt32(form["Id"].ToString());
            int checkOutAmount = Convert.ToInt32(form["CheckOutAmount"].ToString());

            //find the item 1st
            InventoryViewModel tempItem = new InventoryViewModel();
            tempItem = retrieveInventory(Id);

            int currentItemAmount = tempItem.ItemQuantity;
            int totalCheckIn = currentItemAmount - checkOutAmount; //to add on the current qty with checkin qty

            string checkInQuery = $"UPDATE inventory SET ItemQuantity='{totalCheckIn}' WHERE Id='{Id}'";

            int result = _helper.DMLTransaction(checkInQuery);

            if (result > 0)
            {
                return RedirectToAction("InventoryList", "Inventory");
            }

            return View();
        }
        #endregion

        #region SearchItem
        [HttpPost]
        public IActionResult SearchItem(IFormCollection form)
        {
            string searchInventory = form["SearchInventory"].ToString();
            List<InventoryViewModel> inventoryListSearch = new List<InventoryViewModel>();

            string query = $"SELECT * FROM inventory WHERE ItemName LIKE '%{searchInventory}%' OR ItemDescription LIKE '%{searchInventory}%' OR ItemSku LIKE '%{searchInventory}%'";

            inventoryListSearch = _helper.GetInventoryItem(query);

            ViewData.Model = inventoryListSearch; //bind the data to the view 

            return View("InventoryList");
        }
        #endregion

        #region Method
        private InventoryViewModel retrieveInventory(int id)
        {
            InventoryViewModel inventoryItem = new InventoryViewModel();
            string query = $"SELECT * FROM inventory WHERE Id = {id}";
            inventoryItem = _helper.FindItem(query);

            return inventoryItem;
        }

        #endregion


        public IActionResult Index()
        {
            return View();
        }
    }
}
