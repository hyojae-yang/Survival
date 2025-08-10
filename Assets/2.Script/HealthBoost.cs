using UnityEngine;

// Weapon 스크립트를 상속받는 패시브 스킬
public class HealthBoost : Weapon
{
    // 패시브 효과를 적용할 플레이어 스탯
    private PlayerStats playerStats;

    // 체력 증가 배율 (인스펙터에서 설정)
    public int healthMultiplier = 2;

    void Start()
    {
        // 플레이어 오브젝트에서 PlayerStats 스크립트를 가져옵니다.
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();

        // 패시브 효과를 즉시 적용합니다.
        ApplyPassiveEffect();
    }

    // 패시브 효과 적용 함수
    private void ApplyPassiveEffect()
    {
        if (playerStats != null)
        {
            // 플레이어의 최대 체력을 2배로 늘립니다.
            playerStats.maxHealth *= healthMultiplier;
            // 현재 체력도 최대 체력에 맞춰 늘려줍니다.
            playerStats.currentHealth = playerStats.maxHealth;

            // UI도 업데이트합니다.
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateHealthUI((int)playerStats.currentHealth, (int)playerStats.maxHealth);
            }

        }
    }
}