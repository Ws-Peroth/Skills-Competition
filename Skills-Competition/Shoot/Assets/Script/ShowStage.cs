using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowStage : MonoBehaviour
{
    [SerializeField] private Text target; 
    public static ShowStage Instance;

    public void ShowStageTitle(string title)
    {
        target.text = title;

        StartCoroutine(ShowEffect());
    }

    private IEnumerator ShowEffect()
    {
        StartCoroutine(Fade(2, true));
        yield return new WaitForSeconds(4);
        StartCoroutine(Fade(2, false));
    }
    
    private const float DeltaTime = 0.04f;
    private IEnumerator Fade(float time, bool alphaAdd)
    {
        var r = target.color.r;
        var b = target.color.b;
        var g = target.color.g;
        target.gameObject.SetActive(true);
        
        var loopCount = (time / DeltaTime);
        var alphaDelta = 1 / loopCount * (alphaAdd ? 1 : -1);
        target.color = alphaAdd ? new Color(r, b, g, 0) : new Color(r, b, g, 1);
        
        while (loopCount --> 0)
        {
            var color = target.color;
            color.a += alphaDelta;
            target.color = color;
            yield return new WaitForSeconds(DeltaTime);
        }

        // target.gameObject.SetActive(alphaAdd);
    }
}
