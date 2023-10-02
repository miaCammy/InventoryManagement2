using InventoryManagement.ViewModel;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace InventoryManagement.Helper
{
    public class CommonHelper
    {
        private static IConfiguration _config;
        
        public CommonHelper(IConfiguration config)
        {
            _config = config;
        }

        public bool UserExist(string query)
        {
            bool exist = false;
            string connStr = _config["ConnectionStrings:DefaultConnection"];
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sqlQuery = query;
                MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
                MySqlDataReader sdr = cmd.ExecuteReader();

                if(sdr.HasRows)
                {
                    exist = true;
                }

                conn.Close();
            }
            return exist;
        }

        public int DMLTransaction(string query)
        {
            int result;
            string connStr = _config["ConnectionStrings:DefaultConnection"];
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sqlQuery = query;
                MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
                result = cmd.ExecuteNonQuery();
                conn.Close();
            }

            return result;
        }

        public UserViewModel GetUserByUserName(string query)
        {
            UserViewModel user = new UserViewModel();
            string connStr = _config["ConnectionStrings:DefaultConnection"];
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sqlQuery = query;
                MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
                using(MySqlDataReader sdr = cmd.ExecuteReader())
                {
                    while(sdr.Read())
                    {
                        user.IdUser = Convert.ToInt32(sdr["IdUser"]);
                        user.UserName = sdr["UserName"].ToString();
                        user.UserPassword = sdr["UserPassword"].ToString();
                        user.Name = sdr["Name"].ToString();
                    }
                }
                conn.Close();
            }

            return user;
        }

        public List<InventoryViewModel> GetInventoryItem(string query)
        {
            List<InventoryViewModel> inventoryList = new List<InventoryViewModel>();
            string connStr = _config["ConnectionStrings:DefaultConnection"];
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sqlQuery = query;
                MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
                using (MySqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        InventoryViewModel tempModel = new InventoryViewModel();

                        tempModel.Id = Convert.ToInt32(sdr["Id"]);
                        tempModel.ItemName = sdr["ItemName"].ToString();
                        tempModel.ItemDescription = sdr["ItemDescription"].ToString();
                        tempModel.ItemSku = sdr["ItemSku"].ToString();
                        tempModel.ItemQuantity = Convert.ToInt32(sdr["ItemQuantity"]);
                        tempModel.ItemMinimumQuantity = Convert.ToInt32(sdr["ItemMinimumQuantity"]);

                        inventoryList.Add(tempModel);
                    }
                }
                conn.Close();
            }

            return inventoryList;
        }

        public InventoryViewModel FindItem(string query)
        {
            InventoryViewModel inventoryModel = new InventoryViewModel();
            string connStr = _config["ConnectionStrings:DefaultConnection"];
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sqlQuery = query;
                MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
                using (MySqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        inventoryModel.Id = Convert.ToInt32(sdr["Id"]);
                        inventoryModel.ItemName = sdr["ItemName"].ToString();
                        inventoryModel.ItemDescription = sdr["ItemDescription"].ToString();
                        inventoryModel.ItemSku = sdr["ItemSku"].ToString();
                        inventoryModel.ItemQuantity = Convert.ToInt32(sdr["ItemQuantity"]);
                        inventoryModel.ItemMinimumQuantity = Convert.ToInt32(sdr["ItemMinimumQuantity"]);
                    }
                }
                conn.Close();
            }

            return inventoryModel;
        }

        public void UpdateQty(string query)
        {
            string connStr = _config["ConnectionStrings:DefaultConnection"];
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                if(!string.IsNullOrEmpty(query))
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
    }
}
