using MassTagger2.Models;
using MassTagger2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassTagger2.Util
{
    public class RESTagBuilder
    {
        public static Dictionary<string, Tag> GetTags(IEnumerable<SubredditViewModel> subreddits, int minimumPosts)
        {
            var tags = new Dictionary<string, Tag>();
            foreach (var subreddit in subreddits)
            {
                using (var context = new MassTaggerContext())
                {
                    var users = context.SubredditUsers
                                        .Include("Posts")
                                        .Include("User")
                                        .Where(su => su.SubredditId == subreddit.Id && su.Posts.Count >= minimumPosts);
                    foreach (var user in users)
                    {
                        if (!tags.ContainsKey("tag."+user.User.Username)){
                            var tag = new Tag
                            {
                                link = "/User/"+user.User.Username,
                                color = subreddit.TagColor.ToString(),
                                ignored = subreddit.Ignored,
                                text = "/r/" + subreddit.Name + " user"
                            };
                            tags.Add("tag."+user.User.Username, tag);
                        }
                    }
                }
            }
            return tags;
        }
    }
}
