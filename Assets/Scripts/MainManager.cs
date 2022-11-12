using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public int LineCount = 6;
    public Brick BrickPrefab;
    public Rigidbody Ball;

    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _highScoreText;
    [SerializeField] private GameObject _gameOverText;
    
    private bool m_Started = false;
    private bool m_GameOver = false;
    private int m_Points;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
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
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        _gameOverText.SetActive(true);
        PersistentGameState.Instance.AddNewScore(m_Points);
        UpdateHighScore();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("menu");
    }

    private void AddPoint(int point)
    {
        m_Points += point;
        _scoreText.text = $"Score: {m_Points}";
    }

    private void UpdateHighScore()
    {
        _highScoreText.text = PersistentGameState.Instance.HighScoreString;
    }

}
