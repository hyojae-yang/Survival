using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum WeaponType { Active, Passive }

public class Weapon : MonoBehaviour
{
    // 무기 정보
    public string weaponName;
    public string weaponDescription;
    public Sprite weaponIcon;
    public WeaponType weaponType;
    public int maxLevel = 5;

    // 무기 스탯
    public float baseDamage;
    public float damageIncreasePerLevel;
    public float baseCooldown;
    public float cooldownDecreasePerLevel;

    // 추가된 변수: 범위 관련
    public float baseRangeIncrease;          // 레벨업으로 얻는 추가 범위의 현재 값
    public float rangeIncreasePerLevel;      // 레벨업 당 추가 범위가 얼마나 증가할지

    // 현재 레벨
    public int currentLevel;

    // 초기 기본 공격력 (초기화에 사용)
    private float initialBaseDamage;
    // 초기 기본 쿨타임 (초기화에 사용)
    private float initialBaseCooldown;
    // 초기 기본 추가 범위 (초기화에 사용)
    private float initialBaseRangeIncrease;

    private void Start()
    {
        InitializeWeapon();
    }

    public void InitializeWeapon()
    {
        initialBaseDamage = baseDamage;
        initialBaseCooldown = baseCooldown;
        initialBaseRangeIncrease = baseRangeIncrease; // 초기 추가 범위 저장
        currentLevel = 1;
    }

    public float GetCurrentDamage()
    {
        return initialBaseDamage + (damageIncreasePerLevel * (currentLevel - 1));
    }

    public float GetCurrentCooldown()
    {
        return Mathf.Max(0, initialBaseCooldown - (cooldownDecreasePerLevel * (currentLevel - 1)));
    }

    // 추가된 함수: 현재 레벨에 따른 추가 범위 값을 가져옴
    public float GetCurrentRangeIncrease()
    {
        return initialBaseRangeIncrease + (rangeIncreasePerLevel * (currentLevel - 1));
    }

    public virtual void LevelUp() // virtual 키워드 추가: 자식 클래스에서 오버라이드 가능하도록
    {
        if (currentLevel >= maxLevel)
        {
            return;
        }

        currentLevel++;
        // 레벨업 시 baseRangeIncrease도 함께 증가시킴
        // baseDamage와 baseCooldown은 GetCurrent~ 함수에서 계산되므로 여기서는 직접 변경하지 않습니다.
    }
}