using System.Collections.Generic;
using UnityEngine;

public class ParticlePooler : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] ParticleTimeout prefab;
    [SerializeField] int cacheCount = 10;


    readonly Queue<ParticleTimeout> pool = new Queue<ParticleTimeout>();

    void Awake()
    {
        InitializePool();
    }

    void InitializePool()
    {
        for (int i = 0; i < cacheCount; i++)
        {
            ParticleTimeout obj = CreateNewObject();
            ReturnToPool(obj);
        }

    }

    ParticleTimeout CreateNewObject()
    {
        ParticleTimeout obj = Instantiate(prefab, Vector3.zero , Quaternion.identity, this.transform);
        obj.InitializePoolerReference(this);
        obj.gameObject.SetActive(false);
        return obj;
    }

    public ParticleTimeout Get()
    {
        if (pool.Count == 0)
        {
            pool.Enqueue(CreateNewObject());
        }

        ParticleTimeout obj = pool.Dequeue();
        obj.gameObject.SetActive(true);

        return obj;
    }

    public void ReturnToPool(ParticleTimeout obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}