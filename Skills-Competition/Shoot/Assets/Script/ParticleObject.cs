using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleObject : MonoBehaviour
{
    public ParticlePool type;
    [SerializeField] private ParticleSystem particle;

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
    public void Play()
    {
        print($"play {type}");
        gameObject.layer = 5;
        particle.Stop();
        if (particle.isStopped)
        {
            particle.Play();
        }
        Invoke(nameof(EffectEnd), 3);
    }

    private void EffectEnd()
    {
        print($"end {type}");
        ParticleManager.Instance.DestroyPrefab(this, type);
    }
}
