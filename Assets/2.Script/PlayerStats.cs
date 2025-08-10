using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    // 플레이어의 능력치
    public float maxHealth = 100f;
    public float currentHealth;
    public float moveSpeed = 5f;

    // 경험치 및 레벨
    public int level = 1;
    public int currentExp = 0;
    public int requiredExp = 10;

    // 무적 시간 관련 변수
    public bool isInvincible;
    public float invincibilityTime = 2f;
    private float invincibilityTimer;

    public GameObject diePanel;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        currentHealth = maxHealth;
    }

    void Start()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealthUI((int)currentHealth, (int)maxHealth);
            UIManager.Instance.UpdateExpUI(currentExp, requiredExp);
            UIManager.Instance.UpdateLevelUI(level);
        }
    }

    void Update()
    {
        // 무적 시간 타이머
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
            }
        }
    }

    // 경험치 획득 함수
    public void GetExp(int expAmount)
    {
        // 경험치 획득 시 효과음은 플레이어와 아이템의 충돌을 감지하는 스크립트에서 재생하는 것이 더 적절합니다.
        currentExp += expAmount;
        if (currentExp >= requiredExp)
        {
            LevelUp();
        }
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateExpUI(currentExp, requiredExp);
        }
    }

    // 레벨업 함수
    private void LevelUp()
    {
        // 레벨업 효과음 재생
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(SoundManager.Instance.playerLevelUpSFX);
        }

        currentExp -= requiredExp;
        level++;
        requiredExp = (int)(requiredExp * 1.5f);

        currentHealth = maxHealth;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateLevelUI(level);
            UIManager.Instance.UpdateExpUI(currentExp, requiredExp);
            UIManager.Instance.UpdateHealthUI((int)currentHealth, (int)maxHealth);
        }

        if (LevelUpPanelUI.Instance != null)
        {
            LevelUpPanelUI.Instance.ShowLevelUpPanel();
        }

    }

    // 플레이어에게 데미지를 주는 함수
    public void TakeDamage(float damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;
            isInvincible = true;
            invincibilityTimer = invincibilityTime;

            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateHealthUI((int)currentHealth, (int)maxHealth);
            }

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    // 추가된 함수: 체력을 회복합니다.
    public void Heal(float amount)
    {
        // 현재 체력에 회복량을 더하고, 최대 체력을 넘지 않도록 합니다.
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);

        // 아이템 획득 효과음은 여기서 재생하는 것보다, 아이템 오브젝트와의 충돌을 감지하는 스크립트에서 호출하는 것이 더 적절합니다.
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealthUI((int)currentHealth, (int)maxHealth);
        }

    }

    private void Die()
    {
        // 플레이어 사망 효과음 재생
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(SoundManager.Instance.playerDieSFX);
        }
        if (diePanel != null)
        {
            diePanel.SetActive(true);
        }

        Time.timeScale = 0;
    }
}