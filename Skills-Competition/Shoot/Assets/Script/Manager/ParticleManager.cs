using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParticlePool
{
    Boss,
    Enemy,
    Max
}
public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance;

    public Queue<ParticleObject>[] particlePool;
    public List<ParticleObject> prefabList = new List<ParticleObject>();

    public static int PoolIndex(ParticlePool code) => (int) code;

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
        var maxIndex = PoolIndex(ParticlePool.Max);
        particlePool = new Queue<ParticleObject>[maxIndex];
        
        for (var i = 0; i < PoolIndex(ParticlePool.Max); i++)
        {
            particlePool[i] = new Queue<ParticleObject>();
        }
    }

    public ParticleObject CreatPrefab(ParticlePool code)
    {
        print($"Creat : {code}");
        var index = PoolIndex(code);
        var targetQueue = particlePool[index];
        if (targetQueue.Count > 0)
        {
            return targetQueue.Dequeue();
        }

        var prefab = Instantiate(prefabList[index], transform);
        prefab.type = code;
        prefab.gameObject.SetActive(true);
        return prefab;
    }

    public void DestroyPrefab(ParticleObject prefab, ParticlePool code)
    {
        prefab.gameObject.SetActive(true);
        particlePool[PoolIndex(code)].Enqueue(prefab);
    }
}
