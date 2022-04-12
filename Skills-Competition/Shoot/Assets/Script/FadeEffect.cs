using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    public static FadeEffect Instance;

    [SerializeField] private Graphic target;

    private Coroutine _fadeRoutine;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        target.gameObject.SetActive(false);
    }
    
    public void FadeIn(float time)
    {
        CheckRoutine();
        _fadeRoutine = StartCoroutine(Fade(time, true));
    }
    
    public void FadeOut(float time)
    {
        CheckRoutine();
        _fadeRoutine = StartCoroutine(Fade(time, false));
    }

    private void CheckRoutine()
    {
        if (_fadeRoutine != null)
        {
            StopCoroutine(_fadeRoutine);
        }
    }
    
    // 0 -> 1
    // loopCount * delta = value
    // loopCount = TotalTime / deltaTime
    // delta = 1 / (TotalTime / deltaTime)
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

        target.gameObject.SetActive(alphaAdd);
    }
}
