using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(1000)]
public class HighscoreUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scores;

    private void Awake()
    {
        StringBuilder stringBuilder = new();
        for(int i = 0; i < PersistentGameState.Instance.HighScoreData?.Scores.Count; i++)
        {
            stringBuilder.Append(string.Format("{0,-3} {1,-5} {2}\r\n", i+1, PersistentGameState.Instance.HighScoreData.Scores[i], PersistentGameState.Instance.HighScoreData.Names[i]));
        }
        _scores.text = stringBuilder.ToString();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("menu");
    }
}
