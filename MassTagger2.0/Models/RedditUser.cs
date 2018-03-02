using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MassTagger2.Models
{
    public class RedditUser
    {
        [Key]
        public int id { get; set; }

        public string Username { get; set; }

        public ICollection<SubredditUser> SubredditUsers { get; set; }
    }
}