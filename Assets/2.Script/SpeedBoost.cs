using UnityEngine;

public class SpeedBoost : Weapon
{
    private PlayerStats playerStats;

    // �̵� �ӵ� ������ (�ν����Ϳ��� ����)
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