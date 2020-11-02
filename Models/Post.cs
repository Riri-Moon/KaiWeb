using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kai_Stranka.Models
{
    public class Post
    {
        public class PostModel
        {
            public string Title { get; set; }
            public string Body { get; set; }
            public string Name { get; set; }
            public DateTimeOffset PubDate { get; set; }
            public DateTimeOffset LastModified { get; set; }

        }




    }
}
