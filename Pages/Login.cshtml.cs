using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Kai_Stranka.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;
using MySql.Data.MySqlClient;

namespace Kai_Stranka.Pages
{
    public class  LoginModel : PageModel
    {
        public bool IsLogged { get; set; }
        public int UserID { get; set; }
        public string IpAddress { get; set; }

        public void OnGet()
        {

        }

        public static LoginUser ReturnStatus()
        {

            using (var connection = new MySqlConnection("SERVER=remotemysql.com; DATABASE=Q9hz1aOZ3d; Uid=Q9hz1aOZ3d; Pwd=doh0BUmUlQ"))
            {
                connection.Open();
                LoginUser user = new LoginUser();
                try
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = $"Select Id,Name, Password, IsLogged, IpAddress from User ";
                            using (MySqlDataReader rdr = command.ExecuteReader())
                            {
                                rdr.Read();
                                if (rdr.HasRows)
                                {
                                    user.Id = (int)rdr.GetValue(0);
                                    user.Logged = (bool)rdr.GetValue(3);
                                    user.Name = (string)rdr.GetValue(1);
                                    user.IpAddress = (string)rdr.GetValue(4);
                                    return user;
                                }
                                else
                                {
                                    return null;
                                }
                            }
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }


        
    
    public void OnPost()
        {
            var name = Request.Form["login"];
            var pss = Request.Form["password"];
            var shassword = SHA512(pss);

            using (var connection = new MySqlConnection("SERVER=remotemysql.com; DATABASE=Q9hz1aOZ3d; Uid=Q9hz1aOZ3d; Pwd=doh0BUmUlQ"))
            {
                connection.Open();

                try
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        var command = connection.CreateCommand();
                        command.CommandText = $"Select Name, Password, IsLogged, IpAddress from User where Name='{name}' ";
                        MySqlDataReader rdr = command.ExecuteReader();
                        rdr.Read();
                        if (rdr.HasRows && (bool)rdr.GetValue(2) == false)
                        {
                            if ((string)rdr.GetValue(1) == shassword.ToLower())
                            {
                                connection.Close();
                                connection.Open();
                                command.CommandText = $"Update User SET IsLogged = (@isLogged) where (Name)='{name}'";
                                command.Parameters.AddWithValue("@isLogged", "1");
                                command.ExecuteNonQuery();
                                command.CommandText = $"Update User SET IpAddress = (@Ip) where (Name)='{name}'";
                                command.Parameters.AddWithValue("@Ip", HttpContext.Connection.RemoteIpAddress.ToString());
                                command.ExecuteNonQuery();
                                ViewData["IsLogged"] = true;
                                IsLogged = true;
                                connection.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                connection.Close();
            }
        }

        public static string SHA512(string inputString)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }


        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                //Do Hexadecimal sustavy
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
    }


}
