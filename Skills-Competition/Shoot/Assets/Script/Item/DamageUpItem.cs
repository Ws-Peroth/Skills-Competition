using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUpItem : Item
{
    public const int Buff = 5;

    private void Start() => InitializeBaseData();
    

    public override void InitializeBaseData()
    {
        ItemType = PoolCode.DamageUpItem;
    }
    protected override void Effect(GameObject player)
    {
        GameManager.Instance.GetScore(Score);
        player.GetComponent<Player>().GetDamageUpItem(Buff);
        PoolManager.Instance.DestroyPrefab(gameObject, ItemType);
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Effect(col.gameObject);
        }
        base.OnTriggerEnter2D(col);
    }
}
