using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public float BulletSpeed { get; set; }
    public float BulletDamage { get; set; }
    public string BulletTarget { get; set; }
    public Vector3 BulletDirection { get; set; }
    public PoolCode BulletType { get; set; }

    public abstract void InitializeBaseData();

    protected abstract void FixedUpdate();
    
    protected abstract void Move();

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("DestroyBorder"))
        {
            PoolManager.Instance.DestroyPrefab(gameObject, BulletType);
        }
    }
}
