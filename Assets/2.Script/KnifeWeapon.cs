using UnityEngine;

public class KnifeWeapon : Weapon
{
    private Transform playerTransform;
    private PlayerMovement playerMovement;

    private float lastAttackTime;

    // Į�� �÷��̾�� ��ġ�� �ʵ��� ������ ��ġ ������
    private float spawnOffset = 2.0f;

    private void Start()
    {
        playerTransform = transform.parent;
        playerMovement = playerTransform.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        // ������ �κ�: ��Ÿ�� ��굵 GetCurrentCooldown() �Լ� ���
        if (Time.time >= lastAttackTime + GetCurrentCooldown())
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    private void Attack()
    {
        GameObject knifeInstance = ObjectPoolManager.Instance.GetKnife();

        if (knifeInstance != null)
        {
            Vector2 direction = playerMovement.lastMoveDirection;
            Vector3 spawnPosition = playerTransform.position + (Vector3)direction.normalized * spawnOffset;

            knifeInstance.transform.position = spawnPosition;

            if (direction != Vector2.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                knifeInstance.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            else
            {
                float angle = (playerTransform.localScale.x > 0) ? -90f : 90f;
                knifeInstance.transform.rotation = Quaternion.Euler(0, 0, angle);
            }

            KnifeProjectile knifeProjectile = knifeInstance.GetComponent<KnifeProjectile>();
            if (knifeProjectile != null)
            {
                // ������ �κ�: GetCurrentDamage() �Լ��� ȣ���Ͽ� ��Ȯ�� ������ ���� ����
                knifeProjectile.SetDamage(GetCurrentDamage());
            }
        }
    }
}