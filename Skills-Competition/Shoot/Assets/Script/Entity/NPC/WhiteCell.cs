using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteCell : Entity
{
    private int dropCount;
    public override void InitializeBaseData()
    {
        dropCount = 0;
        IsDestroyed = false;
        Speed = 0.1f;
        PrefabType = PoolCode.WhiteCell;
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
        if (IsDestroyed)
        {
            return;
        }
    }

    protected override void Move()
    {
        transform.Translate(Vector2.down * Speed);
    }

    public override void Damaged(float damage)
    {
        dropCount++;
        if (dropCount > 1)
        {
            return;
        }

        var randomItem = Random.Range((int) PoolCode.DamageUpItem, (int) PoolCode.Max);
        var item = PoolManager.Instance.CreatPrefab((PoolCode) randomItem);
        item.GetComponent<Item>().InitializeBaseData();
        item.transform.position = transform.position;
        item.SetActive(true);
        
        PoolManager.Instance.DestroyPrefab(gameObject, PrefabType);
    }

    protected override void Attack() { return; }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Damaged(0);
        }
        base.OnTriggerEnter2D(col);
    }
}
