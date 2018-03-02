using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MassTagger2.Models
{
    public class Subreddit
    {
        [Key]
        public int id { get; set; }

        public string SubName { get; set; }

        public ICollection<SubredditUser> SubredditUsers { get; set; }

    }
}