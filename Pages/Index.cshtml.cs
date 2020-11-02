using System.Xml;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Kai_Stranka.Models;
using System;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Xml.Linq;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using Kai_Stranka.Services;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using System.Collections.Generic;

namespace Kai_Stranka.Pages
{
    public class IndexModel : PageModel
    {
        List<string> list = new List<string>();

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void SelectData(Post.PostModel model)
        {

            using (var connection = new MySqlConnection("SERVER=remotemysql.com; DATABASE=Q9hz1aOZ3d; Uid=Q9hz1aOZ3d; Pwd=doh0BUmUlQ"))
            {
                connection.Open();

                try
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        var date = DateTime.Now;
                        var datetime = date.Year + "-" + date.Month + "-" + date.Day;
                        // var cmd = $"";
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = $"Select Meno, Titul, Telo, Vytvorene from Posts  order by Vytvorene desc";
                            using (MySqlDataReader rdr = command.ExecuteReader())
                            {
                                if (rdr.HasRows)
                                {
                                    var count = rdr.FieldCount;
                                    while (rdr.Read())
                                    {
                                        for (var i = 0; i < count; i++)
                                        {
                                            if (rdr.GetValue(i) != null)
                                                list.Add(rdr.GetValue(i).ToString());

                                        }
                                    }

                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                connection.Close();
                ViewData["Results"] = list;

                return;
            }
        }

        private bool IsLogged()
        {
            return true;
        }
        public void OnGet()
        {
            Post.PostModel model = new Post.PostModel();
            SelectData(model);

        }
    }
}
