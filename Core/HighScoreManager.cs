using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace AsteroidsClone.Core;

public class HighScoreEntry
{
    public string Initials { get; set; } = "AAA";
    public int Score { get; set; }
}

public class HighScoreData
{
    public int AllTimeHigh { get; set; }
    public List<HighScoreEntry> Leaderboard { get; set; } = new();
}

public static class HighScoreManager
{
    private const string FileName = "highscores.json";
    private const int MaxLeaderboardEntries = 10;

    public static int AllTimeHigh { get; private set; }
    public static List<HighScoreEntry> Leaderboard { get; private set; } = new();

    private static string FilePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName);

    public static void Load()
    {
        try
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                var data = JsonConvert.DeserializeObject<HighScoreData>(json);
                if (data != null)
                {
                    AllTimeHigh = data.AllTimeHigh;
                    Leaderboard = data.Leaderboard ?? new List<HighScoreEntry>();
                }
            }
        }
        catch
        {
            // If load fails, start fresh
            AllTimeHigh = 0;
            Leaderboard = new List<HighScoreEntry>();
        }
    }

    public static void Save()
    {
        try
        {
            var data = new HighScoreData
            {
                AllTimeHigh = AllTimeHigh,
                Leaderboard = Leaderboard
            };
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }
        catch
        {
            // Silently fail if save doesn't work
        }
    }

    public static bool QualifiesForLeaderboard(int score)
    {
        if (score <= 0) return false;
        if (Leaderboard.Count < MaxLeaderboardEntries) return true;
        return score > Leaderboard.Min(e => e.Score);
    }

    public static void AddEntry(string initials, int score)
    {
        // Update all-time high
        if (score > AllTimeHigh)
        {
            AllTimeHigh = score;
        }

        // Add to leaderboard
        Leaderboard.Add(new HighScoreEntry
        {
            Initials = initials.ToUpper(),
            Score = score
        });

        // Sort by score descending and keep top 10
        Leaderboard = Leaderboard
            .OrderByDescending(e => e.Score)
            .Take(MaxLeaderboardEntries)
            .ToList();

        Save();
    }

    public static void UpdateAllTimeHigh(int score)
    {
        if (score > AllTimeHigh)
        {
            AllTimeHigh = score;
            Save();
        }
    }
}
