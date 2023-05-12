using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unisave.Leaderboards;
using Unisave.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class MyController : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Button submitButton;

    public string leaderboard = "my-leaderboard";

    void Start()
    {
        submitButton.onClick.AddListener(SubmitScore);
    }

    void DownloadLeaderboard()
    {
        text.text = "Downloading...";

        this.Leaderboard(leaderboard)
            .GetTopScores()
            .Then(records => {
                text.text = Serializer.ToJsonString(records);
            });
    }

    void SubmitScore()
    {
        this.Leaderboard(leaderboard)
            .SubmitScore("John Doe", 42.0)
            .Then(DownloadLeaderboard);
    }
}
