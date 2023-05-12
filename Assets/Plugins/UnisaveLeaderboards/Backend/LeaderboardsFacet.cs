using System;
using System.Collections.Generic;
using LightJson;
using Unisave;
using Unisave.Facades;
using Unisave.Facets;
using Unisave.Serialization;

namespace Unisave.Leaderboards.Backend
{
    public class LeaderboardsFacet : Facet
    {
        public void SubmitScore(
            string leaderboard,
            string subject,
            double score
        )
        {
            // TODO: handle "collection does not exist exceptions"
            
            // === load configuration ===
            
            // TODO: load config
            string collection = "leaderboards";
            bool descending = true;
            
            // === find better record(s) and give up if they exist ===
            
            int betterRecordCount = DB.Query(@"
                FOR r IN @@collection
                    FILTER r.leaderboard == @leaderboard
                        AND r.subject == @subject
                        AND @sgn * r.score <= @sgn * @score
                    COLLECT WITH COUNT INTO length
                    RETURN length
            ")
                .Bind("@collection", collection)
                .Bind("leaderboard", leaderboard)
                .Bind("subject", subject)
                .Bind("score", score)
                .Bind("sgn", descending ? 1 : -1)
                .FirstAs<int>();

            if (betterRecordCount > 0)
                return;
            
            // === remove existing record(s) ===
            
            DB.Query(@"
                FOR r IN @@collection
                    FILTER r.leaderboard == @leaderboard
                        AND r.subject == @subject
                    REMOVE r._key IN @@collection
            ")
                .Bind("@collection", collection)
                .Bind("leaderboard", leaderboard)
                .Bind("subject", subject)
                .Run();
            
            // === insert the new record ===

            var record = new JsonObject() {
                ["leaderboard"] = leaderboard,
                ["subject"] = subject,
                ["score"] = score,
                ["createdAt"] = Serializer.ToJson(DateTime.UtcNow)
            };
            
            DB.Query(@"INSERT @record INTO @@collection")
                .Bind("@collection", collection)
                .Bind("record", record)
                .Run();
            
            // === send notifications ===
            
            Log.Info("New record set", record);
        }

        public List<LeaderboardRecord> GetTopScores(
            string leaderboard, int skip, int take
        )
        {
            // TODO: handle "collection does not exist exceptions"
            
            // === validate arguments ===

            if (take > 1_000)
                take = 1_000;
            
            // === load configuration ===
            
            // TODO: load config
            string collection = "leaderboards";
            bool descending = true;
            
            // === query the leaderboard ===
            
            return DB.Query(@"
                FOR r IN @@collection
                    FILTER r.leaderboard == @leaderboard
                    SORT @sgn * r.score DESC
                    LIMIT @skip, @take
                    RETURN r
            ")
                .Bind("@collection", collection)
                .Bind("leaderboard", leaderboard)
                .Bind("sgn", descending ? 1 : -1)
                .Bind("skip", skip)
                .Bind("take", take)
                .GetAs<LeaderboardRecord>();
        }
    }
}