using MassTagger2.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassTagger2.Util
{
    public class JSonMerger
    {
        public static string GetJson(Dictionary<string, Tag> tags, string resFile, bool overwrite)
        {
            var schema = JObject.Parse(resFile);
            var data = (JObject)schema.SelectToken("data");

            var existingTags = new List<string>();
            foreach (var pair in data)
            {
                if (pair.Key.StartsWith("tag") && tags.Keys.Contains(pair.Key.ToLower()))
                {
                    existingTags.Add(pair.Key);
                }
            }

            if (overwrite)
            {
                foreach (var tag in existingTags)
                {
                    data.Remove(tag);
                }
            }
            else
            {
                foreach (var tag in existingTags)
                {
                    tags.Remove(tag);
                }
            }

            var jTags = JObject.FromObject(tags);
            foreach (var child in jTags.Children())
            {
                data.Add(child);
            }
            return schema.ToString();
        }
    }
}