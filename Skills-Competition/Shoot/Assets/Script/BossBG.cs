using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBG : MonoBehaviour
{
    public static BossBG Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;
    }

    private void Start()
    {
        gameObject.transform.position = new Vector3(0, 10, 0);
    }

    public void BossAppend()
    {
        gameObject.transform.position = new Vector3(0, 10, 0);
        StartCoroutine(BGStart());
    }

    public void BossDestroyed()
    {
        StartCoroutine(BgEnd());
    }

    // loopCount * delta = Value
    // Value / loopCount
    
    private IEnumerator BGStart()
    {
        yield return new WaitForSeconds(2);
        const float delta = 7 / ((GameManager.Stage2Time - 1.6f) / 0.01f);
        while (gameObject.transform.position.y > 2.5f)
        {
            // 10 -> 0 : N
            gameObject.transform.Translate(Vector3.down * delta);
            yield return new WaitForSeconds(0.01f);
        }
        
        yield break;
    }
    
    private IEnumerator BgEnd()
    {
        while (gameObject.transform.position.y < -12)
        {
            gameObject.transform.Translate(Vector3.down * 0.05f);
        }
        
        yield break;
    }
}
