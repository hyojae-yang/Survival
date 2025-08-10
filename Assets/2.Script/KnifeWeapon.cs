using UnityEngine;

public class KnifeWeapon : Weapon
{
    private Transform playerTransform;
    private PlayerMovement playerMovement;

    private float lastAttackTime;

    // 칼이 플레이어와 겹치지 않도록 생성될 위치 오프셋
    private float spawnOffset = 2.0f;

    private void Start()
    {
        playerTransform = transform.parent;
        playerMovement = playerTransform.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        // 수정된 부분: 쿨타임 계산도 GetCurrentCooldown() 함수 사용
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
                // 수정된 부분: GetCurrentDamage() 함수를 호출하여 정확한 데미지 값을 전달
                knifeProjectile.SetDamage(GetCurrentDamage());
            }
        }
    }
}