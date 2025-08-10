using UnityEngine;

// Weapon ��ũ��Ʈ�� ��ӹ޴� �нú� ��ų
public class HealthBoost : Weapon
{
    // �нú� ȿ���� ������ �÷��̾� ����
    private PlayerStats playerStats;

    // ü�� ���� ���� (�ν����Ϳ��� ����)
    public int healthMultiplier = 2;

    void Start()
    {
        // �÷��̾� ������Ʈ���� PlayerStats ��ũ��Ʈ�� �����ɴϴ�.
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();

        // �нú� ȿ���� ��� �����մϴ�.
        ApplyPassiveEffect();
    }

    // �нú� ȿ�� ���� �Լ�
    private void ApplyPassiveEffect()
    {
        if (playerStats != null)
        {
            // �÷��̾��� �ִ� ü���� 2��� �ø��ϴ�.
            playerStats.maxHealth *= healthMultiplier;
            // ���� ü�µ� �ִ� ü�¿� ���� �÷��ݴϴ�.
            playerStats.currentHealth = playerStats.maxHealth;

            // UI�� ������Ʈ�մϴ�.
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateHealthUI((int)playerStats.currentHealth, (int)playerStats.maxHealth);
            }

        }
    }
}