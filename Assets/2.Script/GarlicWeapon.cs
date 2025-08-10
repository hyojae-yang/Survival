using UnityEngine;

public class GarlicWeapon : Weapon
{
    private float lastAttackTime;

    public float initialGarlicRadius = 1f;

    void OnEnable()
    {
        UpdateWeaponStats();
    }

    private void Update()
    {
        if (Time.time >= lastAttackTime + GetCurrentCooldown())
        {
            AttackEnemiesInArea();
            lastAttackTime = Time.time;
        }
    }

    private void AttackEnemiesInArea()
    {
        float currentGarlicRadius = initialGarlicRadius + GetCurrentRangeIncrease();

        // 수정된 부분: 반경을 절반으로 줄여서 사용
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, currentGarlicRadius / 2f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(GetCurrentDamage());
                }
            }
            else if (collider.CompareTag("TreasureBox"))
            {
                TreasureBox treasureBox = collider.GetComponent<TreasureBox>();
                if (treasureBox != null)
                {
                    treasureBox.TakeDamage(GetCurrentDamage());
                }
            }
        }
    }

    public override void LevelUp()
    {
        base.LevelUp();
        UpdateWeaponStats();
    }

    private void UpdateWeaponStats()
    {
        float currentGarlicRadius = initialGarlicRadius + GetCurrentRangeIncrease();
        transform.localScale = new Vector3(currentGarlicRadius, currentGarlicRadius, 1f);
    }
}