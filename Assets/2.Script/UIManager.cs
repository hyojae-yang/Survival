using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // 씬 전환을 위해 추가

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    // ----- UI 컴포넌트 변수들 -----
    public Slider healthSlider;
    public Image healthFillImage;
    public TextMeshProUGUI healthText;

    public Slider expSlider;
    public Image expFillImage;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI expText; // 경험치 텍스트를 다시 추가

    public TextMeshProUGUI playTimeText;

    private float elapsedTime = 0f;

    // ----- 추가된 일시정지 UI 변수 -----
    public GameObject pausePanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // 씬 전환 시 파괴되지 않도록 설정 (선택 사항)
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // 게임이 일시정지 상태가 아닐 때만 시간 업데이트
        if (Time.timeScale > 0)
        {
            elapsedTime += Time.deltaTime;
            UpdatePlayTimeUI();
        }
    }

    private void UpdatePlayTimeUI()
    {
        if (playTimeText != null)
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            playTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (healthText != null)
        {
            healthText.text = $"{currentHealth}/{maxHealth}";
        }

        if (healthFillImage != null)
        {
            healthFillImage.color = Color.Lerp(new Color(1f, 0f, 0f, 0f), Color.red, (float)currentHealth / maxHealth);
        }
    }

    public void UpdateExpUI(int currentExp, int expToNextLevel)
    {
        if (expSlider != null)
        {
            expSlider.maxValue = expToNextLevel;
            expSlider.value = currentExp;
        }

        // 수정된 부분: 경험치 텍스트 업데이트 로직
        if (expText != null)
        {
            expText.text = $"{currentExp}/{expToNextLevel}";
        }

        if (expFillImage != null)
        {
            expFillImage.color = Color.Lerp(Color.white, Color.magenta, (float)currentExp / expToNextLevel);
        }
    }

    public void UpdateLevelUI(int level)
    {
        if (levelText != null)
        {
            levelText.text = "Lv. " + level;
        }
    }

    // ----- 추가된 일시정지 관련 함수 -----
    public void ResumeGame()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        Time.timeScale = 1; // 게임 시간 재개
    }

    

    public void GoToTitle()
    {
        // 타이틀 씬으로 이동
        Time.timeScale = 1; // 씬을 로드하기 전 시간 척도를 다시 1로 설정
        SceneManager.LoadScene("TitleScene");
    }
}