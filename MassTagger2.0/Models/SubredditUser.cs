using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MassTagger2.Models
{
    public class SubredditUser
    {
        [Key]
        public int id { get; set; }

        public int SubredditId { get; set; }

        public int UserId { get; set; }

        public Subreddit Subreddit { get; set; }

        public RedditUser User { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}