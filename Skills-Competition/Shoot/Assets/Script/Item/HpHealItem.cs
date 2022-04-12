using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpHealItem : Item
{
    public const float Heal = 20;
    
    private void Start() => InitializeBaseData();
    
    public override void InitializeBaseData()
    {
        ItemType = PoolCode.HpHealItem;
    }

    protected override void Effect(GameObject player)
    {
        GameManager.Instance.GetScore(Score);
        GameManager.Instance.GetHpHealItem(Heal);
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
