using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponInfoUI : MonoBehaviour
{
    public static WeaponInfoUI Instance;

    // 수정된 부분:
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

    public void UpdateAllWeaponInfoUI(List<Weapon> activeWeapons)
    {
        // 기존에 생성된 모든 슬롯을 제거합니다.
        foreach (GameObject slot in spawnedSlots)
        {
            Destroy(slot);
        }
        spawnedSlots.Clear();

        // 현재 보유한 모든 무기에 대한 정보를 표시합니다.
        foreach (Weapon weapon in activeWeapons)
        {
            Transform parentTransform;
            // 무기 타입에 따라 부모 패널을 선택합니다.
            if (weapon.weaponType == WeaponType.Active)
            {
                parentTransform = activeWeaponParent;
            }
            else
            {
                parentTransform = passiveWeaponParent;
            }

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
                nameAndLevelText.text = $"{weapon.weaponName}\nLv. {weapon.currentLevel}";
            }
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