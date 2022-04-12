using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCell : Entity
{
    public override void InitializeBaseData()
    {
        IsDestroyed = false;
        Speed = 0.1f;
        PrefabType = PoolCode.RedCell;
    }

    protected override void OnBecameInvisible()
    {
        return;
    }

    protected override void FixedUpdate()
    {
        Move();
    }

    protected override void Killed()
    {
        return;
    }

    protected override void Move()
    {
        transform.Translate(Vector2.down * Speed);
    }

    public override void Damaged(float damage)
    {
        if(IsDestroyed) return;
        IsDestroyed = true;
        
        GameManager.Instance.GetPainDamage(10);
        PoolManager.Instance.DestroyPrefab(gameObject, PrefabType);
    }

    protected override void Attack() { return; }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bullet"))
        {
            Damaged(0);
        }
        if (col.CompareTag("Player"))
        {
            Damaged(0);
        }
        base.OnTriggerEnter2D(col);
    }
}
