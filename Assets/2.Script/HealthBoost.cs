using UnityEngine;

// Weapon 스크립트를 상속받는 패시브 스킬
public class HealthBoost : Weapon
{
    // 레벨당 체력 보너스 배율 (인스펙터에서 설정)
    public float healthMultiplierBonus = 1.0f;

    // 패시브 스킬의 현재 레벨을 추적하는 변수
    public int healthBoostLevel = 0;

    void Start()
    {
        // Start에서는 아무 작업도 하지 않습니다.
        // 이 스크립트는 PlayerWeaponManager를 통해 OnLevelUp()이 호출될 때만 작동합니다.
    }

    // PlayerWeaponManager에서 호출할 레벨업 함수
    public void OnLevelUp()
    {
        // 레벨을 1 증가시킵니다.
        healthBoostLevel++;
    }

    // PlayerWeaponManager가 체력 보너스 값을 요청할 때 호출되는 함수
    public float GetHealthMultiplierBonus()
    {
        // 현재 레벨에 healthMultiplierBonus를 곱하여 총 보너스 값을 반환합니다.
        // 예를 들어, 1레벨이면 100% (1.0f), 2레벨이면 200% (2.0f) 보너스.
        return healthBoostLevel * healthMultiplierBonus;
    }
}