using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Persistence.Game
{
    public class MainManager : MonoBehaviour
    {
        public int LineCount = 6;
        public Brick BrickPrefab;
        public Rigidbody Ball;

        [SerializeField] private Text _scoreText;
        [SerializeField] private Text _highScoreText;
        [SerializeField] private GameObject _gameOverText;

        private bool _started = false;
        private bool _gameOver = false;
        private int _points;

        private void Start()
        {
            const float step = 0.6f;
            int perLine = Mathf.FloorToInt(4.0f / step);

            int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
            for (int i = 0; i < LineCount; ++i)
            {
                for (int x = 0; x < perLine; ++x)
                {
                    Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                    var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                    brick.PointValue = pointCountArray[i];
                    brick.onDestroyed.AddListener(AddPoint);
                }
            }

            UpdateHighScore();
        }

        private void Update()
        {
            if (!_started)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _started = true;
                    float randomDirection = Random.Range(-1.0f, 1.0f);
                    Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                    forceDir.Normalize();

                    Ball.transform.SetParent(null);
                    Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
                }
            }
            else if (_gameOver)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
        }

        public void GameOver()
        {
            _gameOver = true;
            _gameOverText.SetActive(true);
            PersistentGameState.Instance.AddNewScore(_points);
            UpdateHighScore();
        }

        public void BackToMenu()
        {
            SceneManager.LoadScene("menu");
        }

        private void AddPoint(int point)
        {
            _points += point;
            _scoreText.text = $"Score: {_points}";
        }

        private void UpdateHighScore()
        {
            _highScoreText.text = PersistentGameState.Instance.HighScoreString;
        }
    }
}
