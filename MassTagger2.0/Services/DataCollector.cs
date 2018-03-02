using MassTagger2.Models;
using RedditSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace MassTagger2.Services
{
    public class DataCollector
    {
        public static void Start()
        {
            Start:
                try
                {
                    RegisterThreads();
                }
                catch(Exception e)
                {
                    //I know this is terrible but I don't want to recursively 
                    //call Start(), filling up my stack with tons of method calls
                    goto Start; 
                }
        }

        private static void RegisterThreads()
        {
            var concurrentQueue = new ConcurrentQueue<PostRecord>();
            var reddit = new Reddit();
            //var subredditNames = new List<string>();
            var sb = new StringBuilder();
            using (MassTaggerContext context = new MassTaggerContext())
            {
                foreach (var subreddit in context.Subreddits)
                {
                    sb.Append(subreddit.SubName + "+");
                }

            }
            try
            {
                var sub = sb.ToString().Substring(0, sb.ToString().Length - 1);
                var subreddit = reddit.GetCombinedSubreddit(sub);
                if (subreddit != null)
                {
                    void commentStart() => MakeCommentListener(subreddit, concurrentQueue);
                    void postStart() => MakePostListener(subreddit, concurrentQueue);
                    var commentThread = new Thread(commentStart);
                    var postThread = new Thread(postStart);
                    commentThread.Start();
                    postThread.Start();
                }
            }
            catch (Exception e) { }

            void addRecords() => AddRecords(concurrentQueue);
            var addThread = new Thread(addRecords);
            addThread.Start();
        }

        private static void MakeCommentListener(CombinedSubreddit subreddit, ConcurrentQueue<PostRecord> concurrentQueue)
        {
            try
            {
                var sub = subreddit;
                var queue = concurrentQueue;

                foreach (var comment in sub.Comments.GetListingStream())
                {
                    try
                    {
                        var postRecord = new PostRecord
                        {
                            Subreddit = comment.Subreddit.ToLower(),
                            Username = comment.AuthorName.ToLower(),
                            Url = comment.Shortlink
                        };
                        queue.Enqueue(postRecord);
                    }
                    catch { }
                }
            }
            catch { }
        }

        private static void MakePostListener(CombinedSubreddit subreddit, ConcurrentQueue<PostRecord> concurrentQueue)
        {
            try
            {
                var sub = subreddit;
                var queue = concurrentQueue;

                foreach (var post in sub.Posts.GetListingStream())
                {
                    var postRecord = new PostRecord
                    {
                        Subreddit = post.SubredditName.ToLower(),
                        Username = post.AuthorName.ToLower(),
                        Url = post.Shortlink.Substring(15)
                    };
                    queue.Enqueue(postRecord);
                }
            }
            catch { }
        }

        private static void AddRecords(ConcurrentQueue<PostRecord> concurrentQueue)
        {
            while (true)
            {
                try
                {
                    if (concurrentQueue.TryDequeue(out PostRecord pr))
                    {
                        var user = GetUser(pr.Username);
                        var subreddit = GetSubreddit(pr.Subreddit);
                        var subredditUser = GetSubredditUser(subreddit, user);
                        var post = new Post { SubredditUserID = subredditUser.id, Url = pr.Url };
                        using (var context = new MassTaggerContext())
                        {
                            context.Posts.Add(post);
                            context.SaveChanges();
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }
        }

        private static RedditUser GetUser(string username)
        {
            using (var context = new MassTaggerContext())
            {
                var user = context.RedditUsers.FirstOrDefault(u => u.Username == username);
                if (user == null)
                {
                    user = new RedditUser { Username = username };
                    context.RedditUsers.Add(user);
                    context.SaveChanges();
                }
                return user;
            }
        }

        private static Subreddit GetSubreddit(string subName)
        {
            using (var context = new MassTaggerContext())
            {
                var subreddit = context.Subreddits.FirstOrDefault(s => s.SubName == subName);
                if (subreddit == null)
                {
                    subreddit = new Subreddit { SubName = subName };
                    context.Subreddits.Add(subreddit);
                    context.SaveChanges();
                }
                return subreddit;
            }
        }

        private static SubredditUser GetSubredditUser(Subreddit subreddit, RedditUser user)
        {
            using (var context = new MassTaggerContext())
            {
                var subUser = context.SubredditUsers.FirstOrDefault(su => su.UserId == user.id && su.SubredditId == subreddit.id);
                if (subUser == null)
                {
                    subUser = new SubredditUser { UserId = user.id, SubredditId = subreddit.id };
                    context.SubredditUsers.Add(subUser);
                    context.SaveChanges();
                }
                return subUser;
            }
        }
    }
}