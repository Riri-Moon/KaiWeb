using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using MySql.Data.MySqlClient;
using Kai_Stranka.Models;

namespace Kai_Stranka.Pages
{
    public class PostsModel : PageModel
    {
        public void OnGet()
        {

        }

        public static void InsertData(Post.PostModel model)
        {
            using (var connection = new MySqlConnection("SERVER=remotemysql.com; DATABASE=Q9hz1aOZ3d; Uid=Q9hz1aOZ3d; Pwd=doh0BUmUlQ"))
            {

                connection.Open();

                try
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        var date = DateTime.Now;
                        var datetime = date.Year + "-" + date.Month + "-" + date.Day + " " + date.Hour + ":" + date.Minute;
                        // var cmd = $"";
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = $"Insert into Posts (Meno, Titul, Telo, Vytvorene) values (@Meno,@Titul,@Telo,@Vytvorene)";
                            command.Parameters.AddWithValue("@Meno", model.Name);
                            command.Parameters.AddWithValue("@Titul", model.Title);
                            command.Parameters.AddWithValue("@Telo", model.Body);
                            command.Parameters.AddWithValue("@Vytvorene", datetime);


                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        public void OnPost()
        {
            Post.PostModel model = new Post.PostModel();
            model.Body = Request.Form["Text"];
            model.Title = Request.Form["Title"];
            model.Name = Request.Form["Name"];

            InsertData(model);


        }



    }
}

