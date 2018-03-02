using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MassTagger2.Models
{
    public class Post
    {
        [Key]
        public int id { get; set; }

        public int SubredditUserID { get; set; }

        public string Url { get; set; }

        public SubredditUser SubredditUser { get; set; }
    }
}