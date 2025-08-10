using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    // �÷��̾��� �ɷ�ġ
    public float maxHealth = 100f;
    public float currentHealth;
    public float moveSpeed = 5f;

    // ����ġ �� ����
    public int level = 1;
    public int currentExp = 0;
    public int requiredExp = 10;

    // ���� �ð� ���� ����
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
        // ���� �ð� Ÿ�̸�
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
            }
        }
    }

    // ����ġ ȹ�� �Լ�
    public void GetExp(int expAmount)
    {
        // ����ġ ȹ�� �� ȿ������ �÷��̾�� �������� �浹�� �����ϴ� ��ũ��Ʈ���� ����ϴ� ���� �� �����մϴ�.
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

    // ������ �Լ�
    private void LevelUp()
    {
        // ������ ȿ���� ���
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

    // �÷��̾�� �������� �ִ� �Լ�
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

    // �߰��� �Լ�: ü���� ȸ���մϴ�.
    public void Heal(float amount)
    {
        // ���� ü�¿� ȸ������ ���ϰ�, �ִ� ü���� ���� �ʵ��� �մϴ�.
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);

        // ������ ȹ�� ȿ������ ���⼭ ����ϴ� �ͺ���, ������ ������Ʈ���� �浹�� �����ϴ� ��ũ��Ʈ���� ȣ���ϴ� ���� �� �����մϴ�.
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealthUI((int)currentHealth, (int)maxHealth);
        }

    }

    private void Die()
    {
        // �÷��̾� ��� ȿ���� ���
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