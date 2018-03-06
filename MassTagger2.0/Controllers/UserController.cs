using MassTagger2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MassTagger2.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index(string userId)
        {
            RedditUser user;
            using (MassTaggerContext context = new MassTaggerContext())
            {
                user = context.RedditUsers.Include("SubredditUsers.Subreddit").Include("SubredditUsers.Posts").FirstOrDefault(u => u.Username == userId);
            }
            if(user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
    }
}