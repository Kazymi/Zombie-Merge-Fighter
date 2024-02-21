using System.Collections.Generic;
using UnityEngine;

public class MergeObjectPool : MonoBehaviour, IMergeObjectPool
{
    [SerializeField] private MergeObjectConfiguration[] mergeObjectConfigurations;
    [SerializeField] private MergePoint[] points;

    private Dictionary<int, IPool<MergeObject>> pools = new Dictionary<int, IPool<MergeObject>>();

    private void Awake()
    {
        InitializePool();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnObject(1);
        }
    }

    private void OnEnable()
    {
        ServiceLocator.Subscribe<IMergeObjectPool>(this);
    }

    private void OnDisable()
    {
        ServiceLocator.Unsubscribe<IMergeObjectPool>();
    }

    private void InitializePool()
    {
        foreach (var mergeObjectConfiguration in mergeObjectConfigurations)
        {
            var factory = new FactoryMonoObject<MergeObject>(mergeObjectConfiguration.Prefab.gameObject, transform);
            var pool = new Pool<MergeObject>(factory, 2);
            pools.Add(mergeObjectConfiguration.Level, pool);
        }
    }

    public MergeObject GetMergeObject(int level)
    {
        var pool = pools[level].Pull();
        pool.Initialize(level);
        return pool;
    }

    public void SpawnObject(int spawnLevel)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].IsPointFree)
            {
                var poolObject = GetMergeObject(spawnLevel);
                points[i].AttackAfterPool(poolObject);
                return;
            }
        }
    }
}