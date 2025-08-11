using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public static PlayerWeaponManager Instance;

    public List<GameObject> weaponPrefabs;
    public List<Weapon> activeWeapons = new List<Weapon>();

    // 패시브 스킬의 레벨을 관리할 딕셔너리
    public Dictionary<string, HealthBoost> activePassiveSkills = new Dictionary<string, HealthBoost>();
    public Dictionary<string, SpeedBoost> activeSpeedBoostSkills = new Dictionary<string, SpeedBoost>();

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

    void Start()
    {
        if (SettingsManager.Instance != null && weaponPrefabs.Count > 0)
        {
            int initialWeaponIndex = SettingsManager.Instance.selectedWeapon;
            if (initialWeaponIndex >= 0 && initialWeaponIndex < weaponPrefabs.Count)
            {
                AddNewWeapon(weaponPrefabs[initialWeaponIndex]);
            }
            else
            {
                AddNewWeapon(weaponPrefabs[0]);
            }
        }
        else
        {
            if (activeWeapons.Count == 0 && weaponPrefabs.Count > 0)
            {
                AddNewWeapon(weaponPrefabs[0]);
            }
        }

        foreach (Weapon weapon in activeWeapons)
        {
            if (weapon != null)
            {
                weapon.InitializeWeapon();
            }
        }
    }

    public void LevelUpWeapon(GameObject weaponPrefab)
    {

        if (weaponPrefab.GetComponent<HealthBoost>() != null)
        {
            LevelUpPassiveSkill<HealthBoost>(weaponPrefab, activePassiveSkills);
            return;
        }

        if (weaponPrefab.GetComponent<SpeedBoost>() != null)
        {
            LevelUpPassiveSkill<SpeedBoost>(weaponPrefab, activeSpeedBoostSkills);
            return;
        }

        Weapon existingWeapon = GetWeaponInstanceFromPrefab(weaponPrefab);

        if (existingWeapon != null)
        {
            existingWeapon.LevelUp();

            if (WeaponInfoUI.Instance != null)
            {
                WeaponInfoUI.Instance.UpdateAllWeaponInfoUI();
            }
        }
        else
        {
            AddNewWeapon(weaponPrefab);
        }
    }

    private void LevelUpPassiveSkill<T>(GameObject passiveSkillPrefab, Dictionary<string, T> passiveSkillsDictionary) where T : MonoBehaviour
    {
        string skillName = passiveSkillPrefab.name;
        T skillComponent = null;

        if (passiveSkillsDictionary.ContainsKey(skillName))
        {
            skillComponent = passiveSkillsDictionary[skillName];
        }
        else
        {
            GameObject newSkillObject = Instantiate(passiveSkillPrefab, transform.position, Quaternion.identity, transform);
            skillComponent = newSkillObject.GetComponent<T>();
            if (skillComponent != null)
            {
                passiveSkillsDictionary.Add(skillName, skillComponent);
            }
        }

        if (skillComponent is HealthBoost healthBoost)
        {
            healthBoost.OnLevelUp();
        }
        else if (skillComponent is SpeedBoost speedBoost)
        {
            speedBoost.OnLevelUp();
        }

        PlayerStats.Instance.RecalculateStats();

        if (WeaponInfoUI.Instance != null)
        {
            WeaponInfoUI.Instance.UpdateAllWeaponInfoUI();
        }
    }

    public void AddNewWeapon(GameObject newWeaponPrefab)
    {
        GameObject newWeaponObject = Instantiate(newWeaponPrefab, transform.position, Quaternion.identity, transform);
        Weapon newWeapon = newWeaponObject.GetComponent<Weapon>();

        if (newWeapon != null)
        {
            activeWeapons.Add(newWeapon);
            newWeapon.InitializeWeapon();

            if (WeaponInfoUI.Instance != null)
            {
                WeaponInfoUI.Instance.UpdateAllWeaponInfoUI();
            }
        }
    }

    public float GetHealthBonusMultiplier()
    {
        float bonus = 0;
        foreach (var skill in activePassiveSkills.Values)
        {
            bonus += skill.GetHealthMultiplierBonus();
        }
        return bonus;
    }

    public float GetSpeedIncreaseBonus()
    {
        float bonus = 0;
        foreach (var skill in activeSpeedBoostSkills.Values)
        {
            bonus += skill.GetSpeedIncreaseBonus();
        }
        return bonus;
    }

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