using UnityEngine;

public class MagicBall : MonoBehaviour
{
    // 투사체 속도 (플레이어 이동 속도 5보다 조금 빠르게)
    public float speed = 7.0f;
    // 투사체 데미지
    public int damage = 5;
    // 투사체의 수명
    public float lifetime = 5.0f;

    private Vector3 direction;

    void Start()
    {
        // 일정 시간 후 투사체 파괴
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // 설정된 방향으로 투사체 이동
        transform.position += direction * speed * Time.deltaTime;
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어와 충돌 시
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
            }

            // 투사체 파괴
            Destroy(gameObject);
        }
    }
}