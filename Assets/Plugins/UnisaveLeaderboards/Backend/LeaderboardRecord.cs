using System;
using Unisave.Arango;

namespace Unisave.Leaderboards.Backend
{
    /// <summary>
    /// One record in a leaderboard
    /// </summary>
    public class LeaderboardRecord
    {
        [SerializeAs("_id")]
        public DocumentId id;
        
        public string leaderboard;
        
        public string subject;
        
        public double score;
        
        public DateTime createdAt;
    }
}