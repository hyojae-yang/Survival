using UnityEngine;

// Weapon 스크립트를 상속받는 패시브 스킬
public class SpeedBoost : Weapon
{
    // 레벨당 이동 속도 증가량 (인스펙터에서 설정)
    public float speedIncreasePerLevel = 1.0f;

    // 패시브 스킬의 현재 레벨을 추적하는 변수
    public int speedBoostLevel = 0;

    void Start()
    {
        // Start에서는 아무 작업도 하지 않습니다.
        // 이 스크립트는 PlayerWeaponManager를 통해 OnLevelUp()이 호출될 때만 작동합니다.
    }

    // PlayerWeaponManager에서 호출할 레벨업 함수
    public void OnLevelUp()
    {
        // 레벨을 1 증가시킵니다.
        speedBoostLevel++;
    }

    // PlayerWeaponManager가 이동 속도 보너스 값을 요청할 때 호출되는 함수
    public float GetSpeedIncreaseBonus()
    {
        // 현재 레벨에 speedIncreasePerLevel를 곱하여 총 보너스 값을 반환합니다.
        return speedBoostLevel * speedIncreasePerLevel;
    }
}