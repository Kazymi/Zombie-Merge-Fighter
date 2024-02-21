public interface IMergeObjectPool
{
    MergeObject GetMergeObject(int level);
    void SpawnObject(int spawnLevel);
}