using UnityEngine;

public class Wizard : Enemy
{
    public GameObject magicBallPrefab;
    public float attackCooldown = 2.0f;

    public float minDistance = 5.0f;
    public float maxDistance = 10.0f;

    private float lastAttackTime;

    protected override void Start()
    {
        base.Start();
        // 마법사 기본 능력치에 GameManager의 배율 적용 및 int로 형 변환
        maxHealth = 10f * GameManager.Instance.enemyHealthMultiplier; // 기본 체력 10
        currentHealth = maxHealth;
        moveSpeed = 3f; // 이동 속도는 그대로
        damage = (int)(10 * GameManager.Instance.enemyDamageMultiplier); // 기본 공격력 10

        lastAttackTime = Time.time;
    }

    protected override void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > maxDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
        else if (distanceToPlayer < minDistance)
        {
            // 플레이어가 너무 가까이 있으면 멈춤 (도망가지 않음)
        }
        else
        {
            // 최적의 거리에 있으면 멈춤
        }

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            ShootMagicBall();
            lastAttackTime = Time.time;
        }
    }

    private void ShootMagicBall()
    {
        // 위자드 공격 효과음 재생
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(SoundManager.Instance.wizardAttackSFX);
        }

        if (magicBallPrefab == null) return;

        GameObject magicBall = Instantiate(magicBallPrefab, transform.position, Quaternion.identity);
        MagicBall magicBallScript = magicBall.GetComponent<MagicBall>();

        if (magicBallScript != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            magicBallScript.SetDirection(direction);
        }
    }
}