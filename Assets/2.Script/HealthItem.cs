using UnityEngine;

public class HealthItem : MonoBehaviour
{
    private PlayerStats playerStats;

    private void Start()
    {
        // 플레이어 오브젝트를 찾아서 PlayerStats 컴포넌트를 가져옵니다.
        // 이 방법은 게임 시작 시 한 번만 호출되므로 효율적입니다.
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerStats = playerObject.GetComponent<PlayerStats>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 오브젝트가 플레이어인지 확인합니다.
        if (other.CompareTag("Player"))
        {
            // 플레이어 스탯이 null이 아닌지 확인합니다.
            if (playerStats != null)
            {
                // 플레이어 최대 체력의 절반만큼 회복합니다.
                float healAmount = playerStats.maxHealth * 0.5f;
                playerStats.Heal(healAmount);
            }

            // 아이템을 오브젝트 풀로 반환합니다.
            ObjectPoolManager.Instance.ReturnHealthItem(gameObject);
        }
    }
}