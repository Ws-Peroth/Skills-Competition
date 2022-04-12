using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFX
{
    ButtonClick,
    BossAttack,
    EnemyAttack,
    PlayerAttack,
    PlayerHit,
    EnemyDestroy
}

public enum BGM
{
    Title,
    Stage1,
    Stage1Boss,
    Stage2,
    Stage2Boss,
    Ending
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private List<AudioClip> sfxClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> bgmClips = new List<AudioClip>();

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        sfxSource.volume = 1;
        bgmSource.volume = 1;
    }

    public float SfxVolume
    {
        get => sfxSource.volume;
        set => sfxSource.volume = value;
    }
    public float BgmVolume
    {
        get => bgmSource.volume;
        set => bgmSource.volume = value;
    }

    public void BgmPlay(BGM bgm)
    {
        if (bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }

        bgmSource.clip = bgmClips[(int) bgm];
        bgmSource.Play();
    }
    
    public void SfxPlay(SFX sfx)
    {
        sfxSource.PlayOneShot(sfxClips[(int) sfx], SfxVolume);
    }
}
