using UnityEngine;

public class HealthItem : MonoBehaviour
{
    private PlayerStats playerStats;

    private void Start()
    {
        // �÷��̾� ������Ʈ�� ã�Ƽ� PlayerStats ������Ʈ�� �����ɴϴ�.
        // �� ����� ���� ���� �� �� ���� ȣ��ǹǷ� ȿ�����Դϴ�.
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerStats = playerObject.GetComponent<PlayerStats>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �浹�� ������Ʈ�� �÷��̾����� Ȯ���մϴ�.
        if (other.CompareTag("Player"))
        {
            // �÷��̾� ������ null�� �ƴ��� Ȯ���մϴ�.
            if (playerStats != null)
            {
                // �÷��̾� �ִ� ü���� ���ݸ�ŭ ȸ���մϴ�.
                float healAmount = playerStats.maxHealth * 0.5f;
                playerStats.Heal(healAmount);
            }

            // �������� ������Ʈ Ǯ�� ��ȯ�մϴ�.
            ObjectPoolManager.Instance.ReturnHealthItem(gameObject);
        }
    }
}