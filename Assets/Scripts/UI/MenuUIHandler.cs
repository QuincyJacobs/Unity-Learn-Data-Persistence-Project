using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Persistence.UI
{
    // Sets the script to be executed later than all default scripts
    // This is helpful for UI, since other things may need to be initialized before setting the UI
    [DefaultExecutionOrder(1000)]
    public class MenuUIHandler : MonoBehaviour
    {
        private const int MAX_USERNAME_LENGTH = 7;

        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TMP_InputField _username;

        private void Awake()
        {
            _scoreText.text = PersistentGameState.Instance.HighScoreString;
            _username.text = PersistentGameState.Instance.Username;
        }

        public void OnUsernameChange(string username)
        {
            if (!string.IsNullOrEmpty(username) && !Regex.IsMatch(username, @"^[a-zA-Z]+$"))
            {
                _username.text = PersistentGameState.Instance.Username;
            }

            if(username.Count() > MAX_USERNAME_LENGTH)
            {
                username = username[..MAX_USERNAME_LENGTH];
                _username.text = username;
            }

            PersistentGameState.Instance.Username = _username.text;
        }

        public void StartGame()
        {
            SceneManager.LoadScene("main");
        }

        public void HighScores()
        {
            SceneManager.LoadScene("highscores");
        }

        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        }
    }
}
