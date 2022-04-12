using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum PoolCode
{
    PlayerBullet,
    Bacteria,
    Germ,
    GermBullet,
    Virus,
    VirusBullet,
    Cancer,
    CancelBullet,
    CovidVirus,
    CovidVirusBullet,
    UpgradeCovidVirus,
    UpgradeCovidVirusBullet,
    RedCell,
    WhiteCell,
    DamageUpItem,
    PowerUpItem,
    SoreUpItem,
    HpHealItem,
    PainHealItem,
    UnbreakableItem,
    
    Max
}

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    public Queue<GameObject>[] objectPool;
    public List<GameObject>[] creatObjectList;
    public List<GameObject> prefabList = new List<GameObject>();

    public static int PoolIndex(PoolCode code) => (int) code;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        InitializeBaseData();
    }

    private void InitializeBaseData()
    {
        var maxIndex = PoolIndex(PoolCode.Max);
        objectPool = new Queue<GameObject>[maxIndex];
        creatObjectList = new List<GameObject>[maxIndex];
        
        for (var i = 0; i < PoolIndex(PoolCode.Max); i++)
        {
            objectPool[i] = new Queue<GameObject>();
            creatObjectList[i] = new List<GameObject>();
        }
    }

    public GameObject CreatPrefab(PoolCode code)
    {
        var index = PoolIndex(code);
        var targetQueue = objectPool[index];
        if (targetQueue.Count > 0)
        {
            return targetQueue.Dequeue();
        }

        var prefab = Instantiate(prefabList[index], transform);
        creatObjectList[index].Add(prefab);
        prefab.SetActive(false);
        return prefab;
    }

    public void DestroyPrefab(GameObject prefab, PoolCode code)
    {
        prefab.SetActive(false);
        objectPool[PoolIndex(code)].Enqueue(prefab);
    }

    public void DestroyAllEntity()
    {
        
    }
    
    public void DestroyAllPrefabs(PoolCode code)
    {
        var index = PoolIndex(code);
        var target = creatObjectList[index];
        foreach (var i in target)
        {
            
            DestroyPrefab(i, code);
        }
    }

    public void DestroyEntirePrefabs()
    {
        for (var i = 0; i < PoolIndex(PoolCode.Max); i++)
        {
            DestroyAllPrefabs((PoolCode) i);
        }
    }
}
