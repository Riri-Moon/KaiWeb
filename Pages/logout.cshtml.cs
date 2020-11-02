using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace Kai_Stranka.Pages
{
    public class logoutModel : PageModel
    {
        public void OnGet()
        {
        }

        public void OnPost()
        {
            var user = LoginModel.ReturnStatus();
            if (user.Logged)
            {

                using (var connection = new MySqlConnection("SERVER=remotemysql.com; DATABASE=Q9hz1aOZ3d; Uid=Q9hz1aOZ3d; Pwd=doh0BUmUlQ"))
                {
                    connection.Open();

                    try
                    {
                        if (connection.State == System.Data.ConnectionState.Open)
                        {
                            var command = connection.CreateCommand();
                            command.CommandText = $"Select Id,Name, Password, IsLogged from User ";
                            MySqlDataReader rdr = command.ExecuteReader();
                            rdr.Read();
                            if (rdr.HasRows && (bool)rdr.GetValue(3) == true)
                            {

                                connection.Close();
                                connection.Open();
                                command.CommandText = $"Update User SET IsLogged = (@isLogged) where (Name)='{user.Name}'";
                                command.Parameters.AddWithValue("@isLogged", "0");
                                command.ExecuteNonQuery();
                                ViewData["IsLogged"] = false;
                                connection.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    connection.Close();
                    ViewData["IsLoggedOut"] = true;
                }
            }

        }
    }
    
}
