using UnityEngine;

public class KnifeProjectile : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float lifeTime = 2f;

    private float projectileDamage;

    public void SetDamage(float damage)
    {
        projectileDamage = damage;
    }

    private void OnEnable()
    {
        Invoke("Deactivate", lifeTime);
    }

    private void Update()
    {
        // Į�� ���� Y��(����) �������� ���ư��� ��
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(projectileDamage);
            }
            Deactivate();
        }
        // ������ �κ�: ���� �浹 ���� �߰�
        else if (other.CompareTag("TreasureBox"))
        {
            TreasureBox treasureBox = other.GetComponent<TreasureBox>();
            if (treasureBox != null)
            {
                treasureBox.TakeDamage(projectileDamage);
            }
            Deactivate();
        }
        
    }

    private void Deactivate()
    {
        ObjectPoolManager.Instance.ReturnKnife(gameObject);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}