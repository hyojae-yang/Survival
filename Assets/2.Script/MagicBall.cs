using UnityEngine;

public class MagicBall : MonoBehaviour
{
    // ����ü �ӵ� (�÷��̾� �̵� �ӵ� 5���� ���� ������)
    public float speed = 7.0f;
    // ����ü ������
    public int damage = 5;
    // ����ü�� ����
    public float lifetime = 5.0f;

    private Vector3 direction;

    void Start()
    {
        // ���� �ð� �� ����ü �ı�
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // ������ �������� ����ü �̵�
        transform.position += direction * speed * Time.deltaTime;
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �÷��̾�� �浹 ��
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
            }

            // ����ü �ı�
            Destroy(gameObject);
        }
    }
}