using UnityEngine;

public class Exp : MonoBehaviour
{
    private int baseExpAmount; // Enemy 스크립트로부터 받은 기본 경험치량

    // 외부(Enemy.cs)에서 기본 경험치량을 설정하는 함수
    public void SetExpAmount(int amount)
    {
        baseExpAmount = amount;
    }

    // 플레이어와 충돌하면 경험치를 제공하고 사라집니다.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 오브젝트의 태그가 "Player"인지 확인
        if (other.CompareTag("Player"))
        {
            // 플레이어의 PlayerStats 스크립트를 가져옵니다.
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                // 기본 경험치량에 GameManager의 배율을 곱하고 int로 형 변환하여 최종 경험치 계산
                int finalExp = (int)(baseExpAmount * GameManager.Instance.expMultiplier);
                playerStats.GetExp(finalExp); // 플레이어에게 경험치 전달
            }

            // 경험치 오브젝트를 풀로 반환합니다.
            ObjectPoolManager.Instance.ReturnExp(gameObject);
        }
    }
}