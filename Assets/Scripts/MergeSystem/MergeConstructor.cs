using System.Linq;
using UnityEngine;

public class MergeConstructor : MonoBehaviour, IMergeConstructor
{
    [SerializeField] private MergeObjectConfiguration[] mergeObjectConfigurations;

    private void OnEnable()
    {
        ServiceLocator.Subscribe<IMergeConstructor>(this);
    }

    private void OnDisable()
    {
        ServiceLocator.Unsubscribe<IMergeConstructor>();
    }

    public MergeObjectConfiguration TryMerge(int level)
    {
        var endValue = mergeObjectConfigurations.Where(t => t.Level == level).ToList();
        return endValue.Count == 0 ? null : endValue[0];
    }
}