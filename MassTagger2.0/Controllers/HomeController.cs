using MassTagger2.Models;
using MassTagger2.Models.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MassTagger2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var vm = new HomeViewModel();
            using (var context = new MassTaggerContext())
            {
                var subs = new List<SubredditViewModel>();
                foreach (var subreddit in context.Subreddits.OrderBy(sub=>sub.SubName))
                {
                    var count = context.SubredditUsers.Count(su => su.SubredditId == subreddit.id);
                    if (count > 0)
                    {
                        var svm = new SubredditViewModel
                        {
                            Name = subreddit.SubName,
                            Ignored = false,
                            TagColor = Util.Enum.TagColor.red
                        };
                        subs.Add(svm);
                    }

                }
                vm.Subreddits = subs;
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult Upload(HomeViewModel vm)
        {
            BinaryReader b = new BinaryReader(vm.JsonFile.InputStream);
            byte[] binData = b.ReadBytes(vm.JsonFile.ContentLength);

            string result = System.Text.Encoding.UTF8.GetString(binData);
            var jobj = JObject.Parse(result);
            return Content(result);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
