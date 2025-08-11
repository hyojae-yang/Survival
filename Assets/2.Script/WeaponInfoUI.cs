using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponInfoUI : MonoBehaviour
{
    public static WeaponInfoUI Instance;

    // 액티브 무기 슬롯이 배치될 부모 오브젝트
    public Transform activeWeaponParent;
    // 패시브 무기 슬롯이 배치될 부모 오브젝트
    public Transform passiveWeaponParent;

    // 무기 슬롯 프리팹
    public GameObject weaponSlotPrefab;

    private List<GameObject> spawnedSlots = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 매개변수 없이 PlayerWeaponManager에서 직접 데이터를 가져오도록 수정
    public void UpdateAllWeaponInfoUI()
    {
        // 기존에 생성된 모든 슬롯을 제거합니다.
        foreach (GameObject slot in spawnedSlots)
        {
            Destroy(slot);
        }
        spawnedSlots.Clear();

        // PlayerWeaponManager가 존재할 경우에만 UI를 업데이트합니다.
        if (PlayerWeaponManager.Instance == null) return;

        // PlayerWeaponManager에서 모든 활성 무기 및 패시브 스킬 목록을 가져옵니다.
        List<Weapon> activeWeapons = PlayerWeaponManager.Instance.activeWeapons;
        Dictionary<string, HealthBoost> passiveSkills = PlayerWeaponManager.Instance.activePassiveSkills;
        Dictionary<string, SpeedBoost> speedSkills = PlayerWeaponManager.Instance.activeSpeedBoostSkills;

        // 액티브 무기 정보를 표시합니다.
        foreach (Weapon weapon in activeWeapons)
        {
            DisplayWeaponSlot(weapon, activeWeaponParent);
        }

        // 패시브 스킬 정보를 표시합니다.
        foreach (var skill in passiveSkills.Values)
        {
            // HealthBoost는 Weapon을 상속받으므로 동일하게 처리 가능
            DisplayWeaponSlot(skill, passiveWeaponParent);
        }

        foreach (var skill in speedSkills.Values)
        {
            // SpeedBoost도 Weapon을 상속받으므로 동일하게 처리 가능
            DisplayWeaponSlot(skill, passiveWeaponParent);
        }
    }

    private void DisplayWeaponSlot(Weapon weapon, Transform parentTransform)
    {
        if (weapon == null) return;

        GameObject newSlot = Instantiate(weaponSlotPrefab, parentTransform);
        spawnedSlots.Add(newSlot);

        // 무기 아이콘 설정
        Image iconImage = FindChildComponent<Image>(newSlot.transform, "UIcon");
        if (iconImage != null)
        {
            iconImage.sprite = weapon.weaponIcon;
        }

        // 무기 이름과 레벨을 함께 표시합니다.
        TextMeshProUGUI nameAndLevelText = FindChildComponent<TextMeshProUGUI>(newSlot.transform, "DescText");
        if (nameAndLevelText != null)
        {
            int levelToDisplay = 0; // 초기 레벨을 0으로 설정

            // 패시브 스킬의 레벨을 별도로 처리
            if (weapon is HealthBoost healthBoost)
            {
                // HealthBoost 스크립트에서 healthBoostLevel을 가져옵니다.
                levelToDisplay = healthBoost.healthBoostLevel;
            }
            else if (weapon is SpeedBoost speedBoost)
            {
                // SpeedBoost 스크립트에서 speedBoostLevel을 가져옵니다.
                levelToDisplay = speedBoost.speedBoostLevel;
            }
            else
            {
                // 일반 무기일 경우 Weapon 클래스의 currentLevel을 사용
                levelToDisplay = weapon.currentLevel;
            }

            nameAndLevelText.text = $"{weapon.weaponName}\nLv. {levelToDisplay}";
        }
    }

    private T FindChildComponent<T>(Transform parent, string name) where T : Component
    {
        foreach (T component in parent.GetComponentsInChildren<T>(true))
        {
            if (component.gameObject.name == name)
            {
                return component;
            }
        }
        return null;
    }
}