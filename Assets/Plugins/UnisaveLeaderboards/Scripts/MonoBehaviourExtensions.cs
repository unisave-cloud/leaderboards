using UnityEngine;

namespace Unisave.Leaderboards
{
    /// <summary>
    /// Allows you to interact with leaderboards from mono behaviours
    /// </summary>
    public static class MonoBehaviourExtensions
    {
        /// <summary>
        /// Work with the given leaderbaord
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
        /// Represents a specific leaderboad and lets you
        /// perform actions with it
        /// </summary>
        public struct LeaderboardRef
        {
            private MonoBehaviour caller;
            private string leaderboard;

            public LeaderboardRef(MonoBehaviour caller, string leaderboard)
            {
                this.caller = caller;
                this.leaderboard = leaderboard;
            }

            public void SubmitScore(string subject, double score)
            {
                // call dat facet
                
                // What is "better"? Needs to be server-side defined
            }

            public void GetTopScores(int take = 10, int skip = 0)
            {
                // NOTE: sorting order needs to be server-side known
                // otherwise inserts don't work
            }
        }
    }
}