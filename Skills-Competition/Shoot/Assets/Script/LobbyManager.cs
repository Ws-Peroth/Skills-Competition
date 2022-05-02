using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private GameObject help;
    [SerializeField] private Text sliderValueText;
    [SerializeField] private Text sliderSfxValueText;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        SoundManager.Instance.SfxPlay(SFX.ButtonClick);
        help.SetActive(false);
    }
    public void OnStartButton()
    {
        SoundManager.Instance.SfxPlay(SFX.ButtonClick);
        StartCoroutine(GameStart());
    }
    public void OnHelpButton()
    {
        SoundManager.Instance.SfxPlay(SFX.ButtonClick);
        
        bgmSlider.value = SoundManager.Instance.BgmVolume * 100;
        sfxSlider.value = SoundManager.Instance.SfxVolume * 100;
        
        sliderValueText.text = $"{bgmSlider.value:0}";
        sliderSfxValueText.text = $"{sfxSlider.value:0}";
        
        help.SetActive(true);
    }
    public void OnQuitButton()
    {
        SoundManager.Instance.SfxPlay(SFX.ButtonClick);
        print("Quit");
        Application.Quit();
    }

    private IEnumerator GameStart()
    {
        SceneManager.LoadScene(2);
        yield break;
    }

    public void SetSoundVolume()
    {
        SoundManager.Instance.BgmVolume = bgmSlider.value / 100;
        sliderValueText.text = $"{bgmSlider.value:0}";
    }
    
    public void SetSFXVolume()
    {
        SoundManager.Instance.SfxVolume = sfxSlider.value / 100;
        sliderSfxValueText.text = $"{sfxSlider.value:0}";
    }

    public void CloseHelp(GameObject screen)
    {
        SoundManager.Instance.SfxPlay(SFX.ButtonClick);
        screen.SetActive(false);
    }
}
