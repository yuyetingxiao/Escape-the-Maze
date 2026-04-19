using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("��ǰ�ؿ�����")]
    public int currentLevelIndex = 0; // 0=��1�أ�1=��2�أ��Դ�����
    public int targetBeanCount = 10;
    public float gameTime = 60f;

    [Header("UI")]
    public Text scoreText;
    public Text timeText;
    public GameObject failPanel;
    public GameObject successPanel;
    public GameObject menuPanel;

    [Header("��������")]
    public float warningTime = 10f;
    public float flashSpeed = 0.2f;

    [Header("��Ч")]
    public AudioClip eatBeanClip;
    [Range(0, 1)] public float beanVolume = 0.7f;

    private int currentScore;
    private float currentTime;
    private bool isGameOver;
    private bool isPaused;

    private float flashTimer;
    private bool timeTextIsVisible = true;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = beanVolume;
    }

    void Start()
    {
        // ���عؿ�ʱ��ͬ��Ŀ�����ݵ�ȫ�ֹ�����
        if (LevelProgressManager.Instance != null)
        {
            LevelProgressManager.Instance.SetLevelTarget(currentLevelIndex, targetBeanCount, gameTime);
        }

        currentTime = gameTime;
        isGameOver = false;
        isPaused = false;
        UpdateScoreUI();
        UpdateTimeUI();

        if (failPanel != null) failPanel.SetActive(false);
        if (successPanel != null) successPanel.SetActive(false);
        if (menuPanel != null) menuPanel.SetActive(false);
    }

    void Update()
    {
        if (isGameOver || isPaused) return;

        currentTime -= Time.deltaTime;
        UpdateTimeUI();

        // ʵʱͬ�����ȵ�ȫ�ֹ�����
        if (LevelProgressManager.Instance != null)
        {
            LevelProgressManager.Instance.UpdateLevelProgress(currentLevelIndex, currentScore, currentTime);
        }

        if (currentTime <= warningTime)
        {
            FlashTimeText();
        }
        else
        {
            if (timeText != null)
            {
                timeText.enabled = true;
                timeText.color = Color.white;
            }
        }

        if (currentTime <= 0)
        {
            currentTime = 0;
            GameFail();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        if (isGameOver) return;

        isPaused = !isPaused;
        menuPanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;

        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (PlayerControl.Instance != null)
                PlayerControl.Instance.StopFootstepAudio();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void FlashTimeText()
    {
        if (timeText == null) return;

        flashTimer += Time.deltaTime;
        if (flashTimer >= flashSpeed)
        {
            timeTextIsVisible = !timeTextIsVisible;
            timeText.enabled = timeTextIsVisible;
            flashTimer = 0;
        }
        timeText.color = Color.red;
    }

    public void AddScore(int value = 1)
    {
        if (isGameOver) return;
        currentScore += value;
        UpdateScoreUI();

        if (eatBeanClip != null)
        {
            audioSource.PlayOneShot(eatBeanClip);
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Bean��" + currentScore + " / " + targetBeanCount;
    }

    void UpdateTimeUI()
    {
        if (timeText != null)
            timeText.text = "Time��" + Mathf.Ceil(currentTime).ToString();
    }

    public bool IsBeanCollectedEnough()
    {
        return currentScore >= targetBeanCount;
    }

    public void StopAllSounds()
    {
        AudioSource[] allAudio = FindObjectsOfType<AudioSource>();
        foreach (AudioSource a in allAudio)
        {
            a.Stop();
        }
    }

    public void GameFail()
    {
        isGameOver = true;
        isPaused = true;
        if (failPanel != null) failPanel.SetActive(true);
        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StopAllSounds();

        if (PlayerControl.Instance != null)
            PlayerControl.Instance.OnGameOver();
    }

    public void GameSuccess()
    {
        isGameOver = true;
        isPaused = true;
        if (successPanel != null) successPanel.SetActive(true);
        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StopAllSounds();

        if (PlayerControl.Instance != null)
            PlayerControl.Instance.OnGameOver();
    }

    // �˵���ť���������˵�
    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("start"); // ���˵�������
    }



    public void LoadNextLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}