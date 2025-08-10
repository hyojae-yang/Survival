using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TitleSceneManager : MonoBehaviour
{
    // ----- 인스펙터에 할당할 UI 패널 및 컴포넌트들 -----
    public GameObject descriptionPanel;
    public GameObject settingsPanel;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public TMP_Dropdown difficultyDropdown;
    public TMP_Dropdown activeWeaponDropdown;

    private void Start()
    {
        if (SoundManager.Instance != null && (!SoundManager.Instance.bgmSource.isPlaying || SoundManager.Instance.bgmSource.clip != SoundManager.Instance.titleBGM))
        {
            SoundManager.Instance.PlayBGM(SoundManager.Instance.titleBGM);
        }
        InitializeSettingsUI();
    }

    private void InitializeSettingsUI()
    {
        if (SettingsManager.Instance != null)
        {
            if (bgmSlider != null) { bgmSlider.onValueChanged.RemoveAllListeners(); }
            if (sfxSlider != null) { sfxSlider.onValueChanged.RemoveAllListeners(); }
            if (difficultyDropdown != null) { difficultyDropdown.onValueChanged.RemoveAllListeners(); }
            if (activeWeaponDropdown != null) { activeWeaponDropdown.onValueChanged.RemoveAllListeners(); }

            if (bgmSlider != null)
            {
                bgmSlider.value = SettingsManager.Instance.GetBGMVolume();
                bgmSlider.onValueChanged.AddListener(SettingsManager.Instance.SetBGMVolume);
            }
            if (sfxSlider != null)
            {
                sfxSlider.value = SettingsManager.Instance.GetSFXVolume();
                sfxSlider.onValueChanged.AddListener(SettingsManager.Instance.SetSFXVolume);
            }
            if (difficultyDropdown != null)
            {
                difficultyDropdown.value = SettingsManager.Instance.selectedDifficulty;
                difficultyDropdown.onValueChanged.AddListener(delegate { SettingsManager.Instance.SetDifficulty(difficultyDropdown.value); });
            }
            if (activeWeaponDropdown != null)
            {
                activeWeaponDropdown.value = SettingsManager.Instance.selectedWeapon;
                activeWeaponDropdown.onValueChanged.AddListener(delegate { SettingsManager.Instance.SetActiveWeapon(activeWeaponDropdown.value); });
            }
        }
    }

    public void SaveSettings()
    {
        if (settingsPanel != null)
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySFX(SoundManager.Instance.buttonSFX);
            }
            if (SettingsManager.Instance != null)
            {
                if (difficultyDropdown != null)
                {
                    SettingsManager.Instance.selectedDifficulty = difficultyDropdown.value;
                }
                if (activeWeaponDropdown != null)
                {
                    SettingsManager.Instance.selectedWeapon = activeWeaponDropdown.value;
                }

            }
            settingsPanel.SetActive(false);
        }
    }

    public void StartGame()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(SoundManager.Instance.buttonSFX);
        }
        SceneManager.LoadScene("MainScene");
    }

    public void ShowDescription()
    {
        if (descriptionPanel != null)
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySFX(SoundManager.Instance.buttonSFX);
            }
            descriptionPanel.SetActive(true);
        }
    }

    public void HideDescription()
    {
        if (descriptionPanel != null)
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySFX(SoundManager.Instance.buttonSFX);
            }
            descriptionPanel.SetActive(false);
        }
    }

    public void ShowSettings()
    {
        if (settingsPanel != null)
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySFX(SoundManager.Instance.buttonSFX);
            }
            settingsPanel.SetActive(true);
        }
    }

    public void HideSettings()
    {
        if (settingsPanel != null)
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySFX(SoundManager.Instance.buttonSFX);
            }
            settingsPanel.SetActive(false);
        }
    }

    public void QuitGame()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(SoundManager.Instance.buttonSFX);
        }
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}