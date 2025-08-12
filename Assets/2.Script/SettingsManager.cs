using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    public AudioMixer audioMixer;
    public int selectedDifficulty = 0;
    public int selectedWeapon = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 변경되어도 이 오브젝트를 유지
        }
        else
        {
            Destroy(gameObject);
        }

        // 씬이 로드될 때 볼륨을 기본값(100%)으로 초기화하는 로직
        SetBGMVolume(1.0f);
        SetSFXVolume(1.0f);
    }

    // UI 슬라이더 초기화를 위해 AudioMixer의 볼륨 값을 가져와 반환하는 메서드
    public float GetBGMVolume()
    {
        float volume;
        if (audioMixer != null && audioMixer.GetFloat("BGMVolume", out volume))
        {
            return Mathf.Pow(10, volume / 20);
        }
        return 1.0f;
    }

    public float GetSFXVolume()
    {
        float volume;
        if (audioMixer != null && audioMixer.GetFloat("SFXVolume", out volume))
        {
            return Mathf.Pow(10, volume / 20);
        }
        return 1.0f;
    }

    // UI 슬라이더 이벤트에서 호출되어 볼륨을 설정하는 메서드
    public void SetBGMVolume(float volume)
    {
        if (audioMixer == null) return;
        if (volume == 0) audioMixer.SetFloat("BGMVolume", -80f);
        else audioMixer.SetFloat("BGMVolume", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        if (audioMixer == null) return;
        if (volume == 0) audioMixer.SetFloat("SFXVolume", -80f);
        else audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }

    // UI 드롭다운 이벤트에서 호출되어 난이도를 설정하는 메서드
    public void SetDifficulty(int index)
    {
        selectedDifficulty = index;
        Debug.Log("난이도 설정: " + selectedDifficulty);
    }

    // UI 드롭다운 이벤트에서 호출되어 무기를 설정하는 메서드
    public void SetActiveWeapon(int index)
    {
        selectedWeapon = index;
        Debug.Log("시작 무기 설정: " + selectedWeapon);
    }
}