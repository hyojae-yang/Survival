using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("BGM")]
    public AudioClip titleBGM;
    public AudioClip mainBGM;

    [Header("SFX")]
    public AudioClip buttonSFX;
    public AudioClip monsterDieSFX;
    public AudioClip wizardAttackSFX;
    public AudioClip playerGetItemSFX;
    public AudioClip boxBreakSFX;
    public AudioClip playerDieSFX;
    public AudioClip playerLevelUpSFX;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "TitleScene")
        {
            PlayBGM(titleBGM);
        }
        else if (currentSceneName == "MainScene")
        {
            PlayBGM(mainBGM);
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip == clip && bgmSource.isPlaying)
        {
            return;
        }

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}