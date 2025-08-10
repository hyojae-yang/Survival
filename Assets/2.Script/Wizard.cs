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
        // ������ �⺻ �ɷ�ġ�� GameManager�� ���� ���� �� int�� �� ��ȯ
        maxHealth = 10f * GameManager.Instance.enemyHealthMultiplier; // �⺻ ü�� 10
        currentHealth = maxHealth;
        moveSpeed = 3f; // �̵� �ӵ��� �״��
        damage = (int)(10 * GameManager.Instance.enemyDamageMultiplier); // �⺻ ���ݷ� 10

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
            // �÷��̾ �ʹ� ������ ������ ���� (�������� ����)
        }
        else
        {
            // ������ �Ÿ��� ������ ����
        }

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            ShootMagicBall();
            lastAttackTime = Time.time;
        }
    }

    private void ShootMagicBall()
    {
        // ���ڵ� ���� ȿ���� ���
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