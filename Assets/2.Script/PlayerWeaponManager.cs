using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public static PlayerWeaponManager Instance;

    // 인스펙터에서 무기 프리팹을 할당할 리스트
    public List<GameObject> weaponPrefabs;

    // 현재 활성화된 무기 인스턴스를 담는 리스트
    private List<Weapon> activeWeapons = new List<Weapon>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // =========================================================
        // 수정된 부분: SettingsManager에서 선택된 무기로 게임 시작
        // =========================================================
        if (SettingsManager.Instance != null && weaponPrefabs.Count > 0)
        {
            int initialWeaponIndex = SettingsManager.Instance.selectedWeapon;

            // 인덱스가 유효한지 확인 후 무기 추가
            if (initialWeaponIndex >= 0 && initialWeaponIndex < weaponPrefabs.Count)
            {
                AddNewWeapon(weaponPrefabs[initialWeaponIndex]);
            }
            else
            {
                // 잘못된 인덱스일 경우, 기본 무기(0번 인덱스) 추가
                AddNewWeapon(weaponPrefabs[0]);
            }
        }
        else
        {
            // SettingsManager가 없거나 weaponPrefabs 리스트가 비어있을 경우
            // 기존 로직처럼 첫 번째 무기를 추가
            if (activeWeapons.Count == 0 && weaponPrefabs.Count > 0)
            {
                AddNewWeapon(weaponPrefabs[0]);
            }
        }

        // 모든 무기의 초기 스탯을 설정합니다.
        foreach (Weapon weapon in activeWeapons)
        {
            if (weapon != null)
            {
                weapon.InitializeWeapon();
            }
        }
    }

    // 레벨업 시 호출될 함수
    // PlayerWeaponManager.cs의 LevelUpWeapon 함수 수정
    public void LevelUpWeapon(GameObject weaponPrefab)
    {
        Weapon existingWeapon = GetWeaponInstanceFromPrefab(weaponPrefab);

        if (existingWeapon != null)
        {
            existingWeapon.LevelUp();

            // 수정: 레벨업 시 모든 무기 정보 UI 업데이트 호출
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

    // PlayerWeaponManager.cs의 AddNewWeapon 함수 수정
    public void AddNewWeapon(GameObject newWeaponPrefab)
    {
        GameObject newWeaponObject = Instantiate(newWeaponPrefab, transform.position, Quaternion.identity, transform);
        Weapon newWeapon = newWeaponObject.GetComponent<Weapon>();

        if (newWeapon != null)
        {
            activeWeapons.Add(newWeapon);
            newWeapon.InitializeWeapon();

            // 수정: 새로운 무기 획득 시 모든 무기 정보 UI 업데이트 호출
            if (WeaponInfoUI.Instance != null)
            {
                WeaponInfoUI.Instance.UpdateAllWeaponInfoUI(activeWeapons);
            }
        }
    }

    // 프리팹에 해당하는 무기 인스턴스를 찾는 헬퍼 함수
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