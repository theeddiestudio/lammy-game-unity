using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // for restarting the scene

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 5f;
    public float gravityScale = 2f;
    public GameObject pipePrefab;
    public float pipeSpawnRate = 2f;
    public float pipeSpeed = 2f;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public GameObject pausePanel;

    private Rigidbody2D rb;
    private int score = 0;
    private bool isGameOver = false;
    private bool hasStarted = false;
    private bool isPaused = false;
    private float pipeSpawnTimer;

    void Start()
    {
        // normalize time
        Time.timeScale = 1f;

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        AudioManager.instance.PauseMusic();
        pipeSpawnTimer = pipeSpawnRate;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);

        UpdateScoreText();
    }

    void Update()
    {
        // Logic for Pause
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            TogglePause();
        }

        // if the game is over or paused, don't run any other game logic.
        if (isGameOver || isPaused)
        {
            return;
        }

        // Logic for Game Starting
        if (!hasStarted)
        {
            if (Input.GetMouseButtonDown(0))
            {
                hasStarted = true;
                rb.gravityScale = gravityScale;
                AudioManager.instance.UnpauseMusic();
                Jump();
            }
            return;
        }

        // Jump
        if (Input.GetMouseButtonDown(0))
        {
            Jump();
        }

        // Spawn Pipes
        pipeSpawnTimer -= Time.deltaTime;
        if (pipeSpawnTimer <= 0)
        {
            SpawnPipe();
            pipeSpawnTimer = pipeSpawnRate;
        }
    }

    // Jump implementation
    void Jump()
    {
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        AudioManager.instance.PlayJumpSound(); // Play jump sound
    }

    // Spawn Implementation
    void SpawnPipe()
    {
        float randomHeight = Random.Range(-1.5f, 2.5f);
        GameObject newPipe = Instantiate(pipePrefab, new Vector3(10, randomHeight, 0), Quaternion.identity);
    }

    // Called when Lammy collides with other object
    void OnCollisionEnter2D(Collision2D collision)
    {
        // If Lammy collides with an object tag "Obstacle" or "Ground"
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Ground"))
        {
            GameOver();
        }
    }

    // Called when Lammy collides with trigger collider object
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("ScorePoint"))
        {
            score++;
            UpdateScoreText();
            AudioManager.instance.PlayScoreSound(); // Play score sound
        }
    }

    // game over state
    void GameOver()
    {
        if (isGameOver) return; // Only run once

        isGameOver = true;
        AudioManager.instance.PlayCrashSound(); // Play crash sound
        AudioManager.instance.PauseMusic();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // pause the time
        Time.timeScale = 0f;
    }

    // update the score
    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    // game restart
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TogglePause()
    {
        if (isGameOver) return; // no pausing if game is over

        isPaused = !isPaused;
        if (isPaused)
        {
            // Pause the game
            Time.timeScale = 0f;
            AudioManager.instance.PauseMusic();
            if (pausePanel != null) pausePanel.SetActive(true);
        }
        else
        {
            // Unpause the game
            Time.timeScale = 1f;
            AudioManager.instance.UnpauseMusic();
            if (pausePanel != null) pausePanel.SetActive(false);
        }
    }
}
