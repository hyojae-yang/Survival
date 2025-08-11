using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpPanelUI : MonoBehaviour
{
    public static LevelUpPanelUI Instance;

    // ----- ������ �г� ������ -----
    public GameObject levelUpPanel;
    public Transform[] optionButtonPositions;

    // ����: ���� ������ ��� '���� ���� ������' ����Ʈ
    public GameObject[] weaponPrefabs;
    // �߰�: ��ư �������� ���� '��ư ���ø� ������'
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
        // ���� ��ư ����
        foreach (GameObject button in spawnedButtons)
        {
            Destroy(button);
        }
        spawnedButtons.Clear();
        selectedWeaponPrefabs.Clear();

        if (weaponPrefabs.Length < 3)
        {
            Debug.LogError("������ �гο� ǥ���� ���� �������� 3�� �̸��Դϴ�.");
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
            // ��ư ���ø��� �����ϰ� ��ġ�� �����մϴ�.
            GameObject buttonInstance = Instantiate(buttonTemplatePrefab, optionButtonPositions[i].position, Quaternion.identity, levelUpPanel.transform);
            spawnedButtons.Add(buttonInstance);

            GameObject selectedWeaponPrefab = weaponPrefabs[randomIndices[i]];
            selectedWeaponPrefabs.Add(selectedWeaponPrefab);

            Weapon weapon = selectedWeaponPrefab.GetComponent<Weapon>();
            if (weapon != null)
            {
                // UIcon ������Ʈ�� ã�� ���� �������� �����մϴ�.
                Image iconImage = FindChildComponent<Image>(buttonInstance.transform, "UIcon");
                if (iconImage != null)
                {
                    iconImage.sprite = weapon.weaponIcon;
                }

                // DescText ������Ʈ�� ã�� ���� ������ �����մϴ�.
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
            // ���õ� ���� �������� PlayerWeaponManager�� �����Ͽ� ������/�߰� ������ ó���ϰ� �մϴ�.
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