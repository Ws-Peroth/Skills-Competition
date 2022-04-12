using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(AlpahLoop());
        SoundManager.Instance.BgmPlay(BGM.Title);
    }

    public void OnStartButton()
    {
        SoundManager.Instance.SfxPlay(SFX.ButtonClick);
        SceneManager.LoadScene(1);
    }

    private IEnumerator AlpahLoop()
    {
        while (true)
        {
            FadeEffect.Instance.FadeOut(2);
            yield return new WaitForSeconds(2);
            FadeEffect.Instance.FadeIn(2);
            yield return new WaitForSeconds(3);
        }
    }
}
