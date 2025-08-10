using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponInfoUI : MonoBehaviour
{
    public static WeaponInfoUI Instance;

    // ������ �κ�:
    // ��Ƽ�� ���� ������ ��ġ�� �θ� ������Ʈ
    public Transform activeWeaponParent;
    // �нú� ���� ������ ��ġ�� �θ� ������Ʈ
    public Transform passiveWeaponParent;

    // ���� ���� ������
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
        // ������ ������ ��� ������ �����մϴ�.
        foreach (GameObject slot in spawnedSlots)
        {
            Destroy(slot);
        }
        spawnedSlots.Clear();

        // ���� ������ ��� ���⿡ ���� ������ ǥ���մϴ�.
        foreach (Weapon weapon in activeWeapons)
        {
            Transform parentTransform;
            // ���� Ÿ�Կ� ���� �θ� �г��� �����մϴ�.
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

            // ���� ������ ����
            Image iconImage = FindChildComponent<Image>(newSlot.transform, "UIcon");
            if (iconImage != null)
            {
                iconImage.sprite = weapon.weaponIcon;
            }

            // ���� �̸��� ������ �Բ� ǥ���մϴ�.
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