using System.Collections.Generic;
using UnityEngine;
using Unisave.Facets;
using Unisave.Leaderboards.Backend;

namespace Unisave.Leaderboards
{
    /// <summary>
    /// Allows you to interact with leaderboards from mono behaviours
    /// </summary>
    public static class MonoBehaviourExtensions
    {
        /// <summary>
        /// Work with the given leaderboard
        /// </summary>
        /// <param name="caller">
        /// The MonoBehaviour instance calling this API
        /// </param>
        /// <param name="leaderboard">Leaderboard name</param>
        /// <returns>
        /// Leaderboard reference with which actions can be performed
        /// </returns>
        public static LeaderboardRef Leaderboard(
            this MonoBehaviour caller,
            string leaderboard
        ) => new LeaderboardRef(caller, leaderboard);
        
        /// <summary>
        /// Represents a specific leaderboard and lets you
        /// perform actions with it
        /// </summary>
        public class LeaderboardRef
        {
            private readonly MonoBehaviour caller;
            private readonly string leaderboard;

            public LeaderboardRef(MonoBehaviour caller, string leaderboard)
            {
                this.caller = caller;
                this.leaderboard = leaderboard;
            }

            public FacetCall SubmitScore(string subject, double score)
            {
                return caller.CallFacet(
                    (LeaderboardsFacet f) => f.SubmitScore(
                        leaderboard, subject, score
                    )
                );
            }

            public FacetCall<List<LeaderboardRecord>> GetTopScores(
                int take = 10,
                int skip = 0
            )
            {
                return caller.CallFacet(
                    (LeaderboardsFacet f) => f.GetTopScores(
                        leaderboard, skip, take
                    )
                );
            }
        }
    }
}