using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public static PlayerWeaponManager Instance;

    // �ν����Ϳ��� ���� �������� �Ҵ��� ����Ʈ
    public List<GameObject> weaponPrefabs;

    // ���� Ȱ��ȭ�� ���� �ν��Ͻ��� ��� ����Ʈ
    private List<Weapon> activeWeapons = new List<Weapon>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // =========================================================
        // ������ �κ�: SettingsManager���� ���õ� ����� ���� ����
        // =========================================================
        if (SettingsManager.Instance != null && weaponPrefabs.Count > 0)
        {
            int initialWeaponIndex = SettingsManager.Instance.selectedWeapon;

            // �ε����� ��ȿ���� Ȯ�� �� ���� �߰�
            if (initialWeaponIndex >= 0 && initialWeaponIndex < weaponPrefabs.Count)
            {
                AddNewWeapon(weaponPrefabs[initialWeaponIndex]);
            }
            else
            {
                // �߸��� �ε����� ���, �⺻ ����(0�� �ε���) �߰�
                AddNewWeapon(weaponPrefabs[0]);
            }
        }
        else
        {
            // SettingsManager�� ���ų� weaponPrefabs ����Ʈ�� ������� ���
            // ���� ����ó�� ù ��° ���⸦ �߰�
            if (activeWeapons.Count == 0 && weaponPrefabs.Count > 0)
            {
                AddNewWeapon(weaponPrefabs[0]);
            }
        }

        // ��� ������ �ʱ� ������ �����մϴ�.
        foreach (Weapon weapon in activeWeapons)
        {
            if (weapon != null)
            {
                weapon.InitializeWeapon();
            }
        }
    }

    // ������ �� ȣ��� �Լ�
    // PlayerWeaponManager.cs�� LevelUpWeapon �Լ� ����
    public void LevelUpWeapon(GameObject weaponPrefab)
    {
        Weapon existingWeapon = GetWeaponInstanceFromPrefab(weaponPrefab);

        if (existingWeapon != null)
        {
            existingWeapon.LevelUp();

            // ����: ������ �� ��� ���� ���� UI ������Ʈ ȣ��
            if (WeaponInfoUI.Instance != null)
            {
                WeaponInfoUI.Instance.UpdateAllWeaponInfoUI(activeWeapons);
            }
        }
        else
        {
            AddNewWeapon(weaponPrefab);
        }
    }

    // PlayerWeaponManager.cs�� AddNewWeapon �Լ� ����
    public void AddNewWeapon(GameObject newWeaponPrefab)
    {
        GameObject newWeaponObject = Instantiate(newWeaponPrefab, transform.position, Quaternion.identity, transform);
        Weapon newWeapon = newWeaponObject.GetComponent<Weapon>();

        if (newWeapon != null)
        {
            activeWeapons.Add(newWeapon);
            newWeapon.InitializeWeapon();

            // ����: ���ο� ���� ȹ�� �� ��� ���� ���� UI ������Ʈ ȣ��
            if (WeaponInfoUI.Instance != null)
            {
                WeaponInfoUI.Instance.UpdateAllWeaponInfoUI(activeWeapons);
            }
        }
    }

    // �����տ� �ش��ϴ� ���� �ν��Ͻ��� ã�� ���� �Լ�
    private Weapon GetWeaponInstanceFromPrefab(GameObject prefab)
    {
        foreach (Weapon weapon in activeWeapons)
        {
            if (weapon.gameObject.name.Contains(prefab.name))
            {
                return weapon;
            }
        }
        return null;
    }
}