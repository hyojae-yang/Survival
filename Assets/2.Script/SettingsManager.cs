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
            DontDestroyOnLoad(gameObject); // ���� ����Ǿ �� ������Ʈ�� ����
        }
        else
        {
            Destroy(gameObject);
        }

        // ���� �ε�� �� ������ �⺻��(100%)���� �ʱ�ȭ�ϴ� ����
        SetBGMVolume(1.0f);
        SetSFXVolume(1.0f);
    }

    // UI �����̴� �ʱ�ȭ�� ���� AudioMixer�� ���� ���� ������ ��ȯ�ϴ� �޼���
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

    // UI �����̴� �̺�Ʈ���� ȣ��Ǿ� ������ �����ϴ� �޼���
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

    // UI ��Ӵٿ� �̺�Ʈ���� ȣ��Ǿ� ���̵��� �����ϴ� �޼���
    public void SetDifficulty(int index)
    {
        selectedDifficulty = index;
        Debug.Log("���̵� ����: " + selectedDifficulty);
    }

    // UI ��Ӵٿ� �̺�Ʈ���� ȣ��Ǿ� ���⸦ �����ϴ� �޼���
    public void SetActiveWeapon(int index)
    {
        selectedWeapon = index;
        Debug.Log("���� ���� ����: " + selectedWeapon);
    }
}