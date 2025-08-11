using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponInfoUI : MonoBehaviour
{
    public static WeaponInfoUI Instance;

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

    // �Ű����� ���� PlayerWeaponManager���� ���� �����͸� ���������� ����
    public void UpdateAllWeaponInfoUI()
    {
        // ������ ������ ��� ������ �����մϴ�.
        foreach (GameObject slot in spawnedSlots)
        {
            Destroy(slot);
        }
        spawnedSlots.Clear();

        // PlayerWeaponManager�� ������ ��쿡�� UI�� ������Ʈ�մϴ�.
        if (PlayerWeaponManager.Instance == null) return;

        // PlayerWeaponManager���� ��� Ȱ�� ���� �� �нú� ��ų ����� �����ɴϴ�.
        List<Weapon> activeWeapons = PlayerWeaponManager.Instance.activeWeapons;
        Dictionary<string, HealthBoost> passiveSkills = PlayerWeaponManager.Instance.activePassiveSkills;
        Dictionary<string, SpeedBoost> speedSkills = PlayerWeaponManager.Instance.activeSpeedBoostSkills;

        // ��Ƽ�� ���� ������ ǥ���մϴ�.
        foreach (Weapon weapon in activeWeapons)
        {
            DisplayWeaponSlot(weapon, activeWeaponParent);
        }

        // �нú� ��ų ������ ǥ���մϴ�.
        foreach (var skill in passiveSkills.Values)
        {
            // HealthBoost�� Weapon�� ��ӹ����Ƿ� �����ϰ� ó�� ����
            DisplayWeaponSlot(skill, passiveWeaponParent);
        }

        foreach (var skill in speedSkills.Values)
        {
            // SpeedBoost�� Weapon�� ��ӹ����Ƿ� �����ϰ� ó�� ����
            DisplayWeaponSlot(skill, passiveWeaponParent);
        }
    }

    private void DisplayWeaponSlot(Weapon weapon, Transform parentTransform)
    {
        if (weapon == null) return;

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
            int levelToDisplay = 0; // �ʱ� ������ 0���� ����

            // �нú� ��ų�� ������ ������ ó��
            if (weapon is HealthBoost healthBoost)
            {
                // HealthBoost ��ũ��Ʈ���� healthBoostLevel�� �����ɴϴ�.
                levelToDisplay = healthBoost.healthBoostLevel;
            }
            else if (weapon is SpeedBoost speedBoost)
            {
                // SpeedBoost ��ũ��Ʈ���� speedBoostLevel�� �����ɴϴ�.
                levelToDisplay = speedBoost.speedBoostLevel;
            }
            else
            {
                // �Ϲ� ������ ��� Weapon Ŭ������ currentLevel�� ���
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