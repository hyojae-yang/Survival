using UnityEngine;

public class Exp : MonoBehaviour
{
    private int baseExpAmount; // Enemy ��ũ��Ʈ�κ��� ���� �⺻ ����ġ��

    // �ܺ�(Enemy.cs)���� �⺻ ����ġ���� �����ϴ� �Լ�
    public void SetExpAmount(int amount)
    {
        baseExpAmount = amount;
    }

    // �÷��̾�� �浹�ϸ� ����ġ�� �����ϰ� ������ϴ�.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // �浹�� ������Ʈ�� �±װ� "Player"���� Ȯ��
        if (other.CompareTag("Player"))
        {
            // �÷��̾��� PlayerStats ��ũ��Ʈ�� �����ɴϴ�.
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                // �⺻ ����ġ���� GameManager�� ������ ���ϰ� int�� �� ��ȯ�Ͽ� ���� ����ġ ���
                int finalExp = (int)(baseExpAmount * GameManager.Instance.expMultiplier);
                playerStats.GetExp(finalExp); // �÷��̾�� ����ġ ����
            }

            // ����ġ ������Ʈ�� Ǯ�� ��ȯ�մϴ�.
            ObjectPoolManager.Instance.ReturnExp(gameObject);
        }
    }
}