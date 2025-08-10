using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    // 상자 스탯 (인스펙터에서 설정)
    public float maxHealth = 1f;
    public int expDropCountMin = 3;
    public int expDropCountMax = 4;
    public int healthItemDropChance = 50;

    // 내부 변수
    private float currentHealth;
    private Animator boxAnimator;
    private ObjectPoolManager poolManager;

    void Awake()
    {
        boxAnimator = GetComponent<Animator>();
        poolManager = ObjectPoolManager.Instance;
    }

    void OnEnable()
    {
        currentHealth = maxHealth;
        // boxAnimator?.Play("Idle"); // 애니메이션 관련 주석은 그대로 유지합니다.
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // boxAnimator?.SetTrigger("Hit");
        }
    }

    private void Die()
    {
        // 상자가 부서지는 효과음 재생
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(SoundManager.Instance.boxBreakSFX);
        }

        // boxAnimator?.SetTrigger("Destroy"); // 애니메이션 관련 주석은 그대로 유지합니다.
        DropItems();
        poolManager.ReturnTreasureBox(gameObject);
    }

    private void DropItems()
    {
        // 1. 경험치 아이템 드롭
        int expToDrop = Random.Range(expDropCountMin, expDropCountMax + 1);
        for (int i = 0; i < expToDrop; i++)
        {
            GameObject exp = poolManager.GetExp();
            if (exp != null)
            {
                // 수정된 부분: 상자 위치 주변에 무작위로 흩뿌림
                Vector3 randomOffset = Random.insideUnitCircle * 0.5f; // 0.5f 반경 내에서 랜덤 위치
                exp.transform.position = transform.position + randomOffset;
            }
        }

        // 2. 체력 회복 아이템 드롭 (확률 기반)
        if (Random.Range(0, 100) < healthItemDropChance)
        {
            GameObject healthItem = poolManager.GetHealthItem();
            if (healthItem != null)
            {
                // 수정된 부분: 상자 위치 주변에 무작위로 흩뿌림
                Vector3 randomOffset = Random.insideUnitCircle * 0.5f;
                healthItem.transform.position = transform.position + randomOffset;
            }
        }
    }

}