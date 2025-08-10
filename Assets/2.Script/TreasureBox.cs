using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    // ���� ���� (�ν����Ϳ��� ����)
    public float maxHealth = 1f;
    public int expDropCountMin = 3;
    public int expDropCountMax = 4;
    public int healthItemDropChance = 50;

    // ���� ����
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
        // boxAnimator?.Play("Idle"); // �ִϸ��̼� ���� �ּ��� �״�� �����մϴ�.
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
        // ���ڰ� �μ����� ȿ���� ���
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(SoundManager.Instance.boxBreakSFX);
        }

        // boxAnimator?.SetTrigger("Destroy"); // �ִϸ��̼� ���� �ּ��� �״�� �����մϴ�.
        DropItems();
        poolManager.ReturnTreasureBox(gameObject);
    }

    private void DropItems()
    {
        // 1. ����ġ ������ ���
        int expToDrop = Random.Range(expDropCountMin, expDropCountMax + 1);
        for (int i = 0; i < expToDrop; i++)
        {
            GameObject exp = poolManager.GetExp();
            if (exp != null)
            {
                // ������ �κ�: ���� ��ġ �ֺ��� �������� ��Ѹ�
                Vector3 randomOffset = Random.insideUnitCircle * 0.5f; // 0.5f �ݰ� ������ ���� ��ġ
                exp.transform.position = transform.position + randomOffset;
            }
        }

        // 2. ü�� ȸ�� ������ ��� (Ȯ�� ���)
        if (Random.Range(0, 100) < healthItemDropChance)
        {
            GameObject healthItem = poolManager.GetHealthItem();
            if (healthItem != null)
            {
                // ������ �κ�: ���� ��ġ �ֺ��� �������� ��Ѹ�
                Vector3 randomOffset = Random.insideUnitCircle * 0.5f;
                healthItem.transform.position = transform.position + randomOffset;
            }
        }
    }

}