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

    public void SetSoundVolume(Slider slider)
    {
        SoundManager.Instance.BgmVolume = slider.value / 100;
        sliderValueText.text = $"{slider.value:0}";
    }
    
    public void SetSFXVolume(Slider slider)
    {
        SoundManager.Instance.SfxVolume = slider.value / 100;
        sliderSfxValueText.text = $"{slider.value:0}";
    }

    public void CloseHelp(GameObject screen)
    {
        SoundManager.Instance.SfxPlay(SFX.ButtonClick);
        screen.SetActive(false);
    }
}
