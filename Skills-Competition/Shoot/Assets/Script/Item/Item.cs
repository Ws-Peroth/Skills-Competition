using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    protected const int Score = 100;
    public PoolCode ItemType { get; set; }

    public abstract void InitializeBaseData();
    protected abstract void Effect(GameObject player);
    
    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("DestroyBorder"))
        {
            print(ItemType);
            PoolManager.Instance.DestroyPrefab(gameObject, ItemType);
        }
    }
}
