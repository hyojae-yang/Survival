using UnityEngine;

public class SpeedBoost : Weapon
{
    private PlayerStats playerStats;

    // 이동 속도 증가량 (인스펙터에서 설정)
    public float speedIncrease = 1f;

    void Start()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        ApplyPassiveEffect();
    }

    private void ApplyPassiveEffect()
    {
        if (playerStats != null)
        {
            playerStats.moveSpeed += speedIncrease;
        }
    }
}