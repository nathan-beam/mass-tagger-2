using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassTagger2.Models.ViewModels
{
    public class HomeViewModel
    {
        public HttpPostedFileBase JsonFile { get; set; }

        public IEnumerable<SubredditViewModel> Subreddits { get; set; }

        public int MinimumPosts { get; set; }

        public bool Overwrite { get; set; }
    }
}