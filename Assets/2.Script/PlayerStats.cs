// PlayerStats.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public float maxHealth = 100f;
    public float currentHealth;
    public float moveSpeed = 5f;

    public float baseHealth;
    public float baseMoveSpeed;

    public int level = 1;
    public int currentExp = 0;
    public int requiredExp = 10;

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

        baseHealth = maxHealth;
        baseMoveSpeed = moveSpeed;

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
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
            }
        }
    }

    public void GetExp(int expAmount)
    {
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

    private void LevelUp()
    {
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

    public void RecalculateStats()
    {
        float newMaxHealth = baseHealth;
        float newMoveSpeed = baseMoveSpeed;

        if (PlayerWeaponManager.Instance != null)
        {
            float healthBonus = PlayerWeaponManager.Instance.GetHealthBonusMultiplier();
            float speedBonus = PlayerWeaponManager.Instance.GetSpeedIncreaseBonus();

            newMaxHealth = baseHealth * (1 + healthBonus);
            newMoveSpeed = baseMoveSpeed + speedBonus;

        }

        float healthRatio = currentHealth / maxHealth;
        maxHealth = newMaxHealth;
        currentHealth = maxHealth * healthRatio;

        moveSpeed = newMoveSpeed;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealthUI((int)currentHealth, (int)maxHealth);
        }
    }

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

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealthUI((int)currentHealth, (int)maxHealth);
        }
    }

    private void Die()
    {
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