using MassTagger2.Models;
using MassTagger2.Models.ViewModels;
using MassTagger2.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MassTagger2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var vm = new HomeViewModel();
            vm.MinimumPosts = 3;
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
                            Id = subreddit.id,
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

            var tags = RESTagBuilder.GetTags(vm.Subreddits, vm.MinimumPosts);
            string result = Encoding.UTF8.GetString(binData);
            var merged = JSonMerger.GetJson(tags, result, vm.Overwrite);
            var contentType = "application/json";
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "MassTagger-" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".resbackup",
                Inline = false
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(Encoding.UTF8.GetBytes(merged), contentType);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
