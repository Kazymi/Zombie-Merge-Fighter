using UnityEngine;

[CreateAssetMenu(menuName = "Configurations/Create MergeObjectConfiguration", fileName = "MergeObjectConfiguration",
    order = 0)]
public class MergeObjectConfiguration : ScriptableObject
{
    [SerializeField] private MergeObject prefab;
    [SerializeField] private int level;

    public MergeObject Prefab => prefab;

    public int Level => level;
}