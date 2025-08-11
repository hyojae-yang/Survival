using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpPanelUI : MonoBehaviour
{
    public static LevelUpPanelUI Instance;

    // ----- 레벨업 패널 변수들 -----
    public GameObject levelUpPanel;
    public Transform[] optionButtonPositions;

    // 수정: 무기 정보가 담긴 '실제 무기 프리팹' 리스트
    public GameObject[] weaponPrefabs;
    // 추가: 버튼 디자인을 위한 '버튼 템플릿 프리팹'
    public GameObject buttonTemplatePrefab;

    private List<GameObject> spawnedButtons = new List<GameObject>();
    private List<GameObject> selectedWeaponPrefabs = new List<GameObject>();

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

    public void ShowLevelUpPanel()
    {
        Time.timeScale = 0;
        levelUpPanel.SetActive(true);
        SetRandomOptions();
    }

    private void SetRandomOptions()
    {
        // 기존 버튼 정리
        foreach (GameObject button in spawnedButtons)
        {
            Destroy(button);
        }
        spawnedButtons.Clear();
        selectedWeaponPrefabs.Clear();

        if (weaponPrefabs.Length < 3)
        {
            Debug.LogError("레벨업 패널에 표시할 무기 프리팹이 3개 미만입니다.");
            return;
        }

        List<int> randomIndices = new List<int>();
        while (randomIndices.Count < 3)
        {
            int randomIndex = Random.Range(0, weaponPrefabs.Length);
            if (!randomIndices.Contains(randomIndex))
            {
                randomIndices.Add(randomIndex);
            }
        }

        for (int i = 0; i < randomIndices.Count; i++)
        {
            // 버튼 템플릿을 생성하고 위치를 지정합니다.
            GameObject buttonInstance = Instantiate(buttonTemplatePrefab, optionButtonPositions[i].position, Quaternion.identity, levelUpPanel.transform);
            spawnedButtons.Add(buttonInstance);

            GameObject selectedWeaponPrefab = weaponPrefabs[randomIndices[i]];
            selectedWeaponPrefabs.Add(selectedWeaponPrefab);

            Weapon weapon = selectedWeaponPrefab.GetComponent<Weapon>();
            if (weapon != null)
            {
                // UIcon 컴포넌트를 찾아 무기 아이콘을 설정합니다.
                Image iconImage = FindChildComponent<Image>(buttonInstance.transform, "UIcon");
                if (iconImage != null)
                {
                    iconImage.sprite = weapon.weaponIcon;
                }

                // DescText 컴포넌트를 찾아 무기 설명을 설정합니다.
                TextMeshProUGUI descText = FindChildComponent<TextMeshProUGUI>(buttonInstance.transform, "DescText");
                if (descText != null)
                {
                    descText.text = weapon.weaponDescription;
                }
            }

            Button buttonComponent = buttonInstance.GetComponent<Button>();
            if (buttonComponent != null)
            {
                int index = i;
                buttonComponent.onClick.AddListener(() => SelectWeapon(index));
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

    public void SelectWeapon(int optionIndex)
    {
        if (PlayerWeaponManager.Instance != null && selectedWeaponPrefabs.Count > optionIndex)
        {
            // 선택된 무기 프리팹을 PlayerWeaponManager에 전달하여 레벨업/추가 로직을 처리하게 합니다.
            PlayerWeaponManager.Instance.LevelUpWeapon(selectedWeaponPrefabs[optionIndex]);
        }

        levelUpPanel.SetActive(false);
        Time.timeScale = 1;

        foreach (GameObject button in spawnedButtons)
        {
            Destroy(button);
        }
        spawnedButtons.Clear();
        selectedWeaponPrefabs.Clear();
    }
}