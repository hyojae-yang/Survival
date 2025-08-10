using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // �� ��ȯ�� ���� �߰�

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    // ----- UI ������Ʈ ������ -----
    public Slider healthSlider;
    public Image healthFillImage;
    public TextMeshProUGUI healthText;

    public Slider expSlider;
    public Image expFillImage;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI expText; // ����ġ �ؽ�Ʈ�� �ٽ� �߰�

    public TextMeshProUGUI playTimeText;

    private float elapsedTime = 0f;

    // ----- �߰��� �Ͻ����� UI ���� -----
    public GameObject pausePanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // �� ��ȯ �� �ı����� �ʵ��� ���� (���� ����)
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // ������ �Ͻ����� ���°� �ƴ� ���� �ð� ������Ʈ
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

        // ������ �κ�: ����ġ �ؽ�Ʈ ������Ʈ ����
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

    // ----- �߰��� �Ͻ����� ���� �Լ� -----
    public void ResumeGame()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        Time.timeScale = 1; // ���� �ð� �簳
    }

    

    public void GoToTitle()
    {
        // Ÿ��Ʋ ������ �̵�
        Time.timeScale = 1; // ���� �ε��ϱ� �� �ð� ô���� �ٽ� 1�� ����
        SceneManager.LoadScene("TitleScene");
    }
}