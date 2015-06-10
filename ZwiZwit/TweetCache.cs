using System;
using System.Collections.Generic;
using System.Text;

namespace ZwiZwit
{
    class TweetCache
    {
        public Dictionary<long, TwitterAccess.StatusInfo> StatusHash { get; protected set; }
        public List<TwitterAccess.StatusInfo> StatusList { get; protected set; }

        public TweetCache()
        {
            StatusHash = new Dictionary<long, TwitterAccess.StatusInfo>();
            StatusList = new List<TwitterAccess.StatusInfo>();
        }

        public bool AddTweet(TwitterAccess.StatusInfo tweet)
        {
            if (StatusHash.ContainsKey(tweet.id))
            {
                return false;
            }
            StatusList.Add(tweet);
            StatusHash.Add(tweet.id, tweet);
            return true;
        }

        public bool RemoveTweet(TwitterAccess.StatusInfo tweet)
        {
            if (!StatusHash.ContainsKey(tweet.id))
            {
                return false;
            }
            StatusHash.Remove(tweet.id);
            StatusList.Remove(tweet);
            return true;
        }

        public void Clear()
        {
            StatusHash.Clear();
            StatusList.Clear();
        }

        public void Load()
        {
        }

    }
}
