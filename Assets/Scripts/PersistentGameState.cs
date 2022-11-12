using Persistence.Data;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Persistence
{
    public class PersistentGameState : MonoBehaviour
    {
        public const string FILENAME = "HighScore.json";
        public const int TOTAL_TRACKED_HIGH_SCORES = 5;

        public static PersistentGameState Instance;

        [HideInInspector] public string Username;
        [HideInInspector] public HighScoreData HighScoreData = new();

        private string _filePath;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _filePath = $"{Application.persistentDataPath}/{FILENAME}";

            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadHighScore();
        }

        public void AddNewScore(int score)
        {
            // Find the index to insert the new data
            int index = HighScoreData.Scores.FindLastIndex(i => i >= score);
            if (index == -1)
            {
                HighScoreData.Names.Insert(0, Username);
                HighScoreData.Scores.Insert(0, score);
            }
            else
            {
                HighScoreData.Names.Insert(index + 1, Username);
                HighScoreData.Scores.Insert(index + 1, score);
            }

            // Remove the last item in the lists if the lists are too long
            if (HighScoreData.Names.Count > TOTAL_TRACKED_HIGH_SCORES)
            {
                HighScoreData.Names.RemoveAt(TOTAL_TRACKED_HIGH_SCORES);
                HighScoreData.Scores.RemoveAt(TOTAL_TRACKED_HIGH_SCORES);
            }

            string json = JsonUtility.ToJson(HighScoreData);
            File.WriteAllText(_filePath, json);
        }

        private void LoadHighScore()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                HighScoreData = JsonUtility.FromJson<HighScoreData>(json);
            }
        }

        public string HighScoreString => $"Best score: {HighScoreName} : {HighScore}";

        private int HighScore => HighScoreData?.Scores.FirstOrDefault() ?? 0;
        private string HighScoreName => HighScoreData?.Names.FirstOrDefault() ?? string.Empty;
    }
}
