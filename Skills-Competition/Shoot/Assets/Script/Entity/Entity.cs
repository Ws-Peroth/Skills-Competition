using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public float AttackDelay { get; set; }
    public bool IsDestroyed { get; set; }
    public float Speed { get; set; }
    [field:SerializeField] public float Damage { get; set; }
    public float Hp { get; set; }
    
    public int Score { get; set; }
    public PoolCode PrefabType { get; set; }
    public PoolCode BulletType { get; set; }

    protected abstract void FixedUpdate();
    protected abstract void Killed();
    protected abstract void Move();
    public abstract void Damaged(float damage);
    protected abstract void Attack();

    public abstract void InitializeBaseData();

    protected abstract void OnBecameInvisible();

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("DestroyBorder"))
        {
            PoolManager.Instance.DestroyPrefab(gameObject, PrefabType);
        }
    }
}
