using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public const int MaxRank = 5;
    public static ScoreManager Instance;
    
    [field:SerializeField] public List<(string name, int score)> LeaderBoardData = new List<(string name, int score)>();
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool IsNewRecord(int score)
    {
        if (LeaderBoardData.Count < MaxRank)
        {
            return true;
        }

        return LeaderBoardData[LeaderBoardData.Count - 1].score <= score;
    }

    public void AddScore(string userName, int score)
    {
        var data = (userName, score);
        LeaderBoardData.Add(data);
        // 내림차순 정렬
        LeaderBoardData.Sort((a, b) => b.score.CompareTo(a.score));

        // if (LeaderBoardData.Count > MaxRank) LeaderBoardData.RemoveAt(MaxRank);
    }
}
