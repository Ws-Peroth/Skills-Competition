using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreItem : Item
{
    public const int Buff = 1500;
    private void Start() => InitializeBaseData();
    
    public override void InitializeBaseData()
    {
        ItemType = PoolCode.PainHealItem;
    }

    protected override void Effect(GameObject player)
    {
        GameManager.Instance.GetScore(Score);
        GameManager.Instance.GetScore(Buff);
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
