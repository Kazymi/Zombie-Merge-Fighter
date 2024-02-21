using UnityEngine;

[RequireComponent(typeof(MergeObjectAnimatorController), typeof(Outline))]
public class MergeObject : MonoPooled
{
    [Header("Outline Color Settong")] [SerializeField]
    private Color canBeMerge;

    [SerializeField] private Color cantBeMerge;

    private MergeObjectAnimatorController mergeObjectAnimatorController;
    private int level;
    private Outline outline;

    public MergePoint attachPoint;

    public int Level => level;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
        mergeObjectAnimatorController = GetComponent<MergeObjectAnimatorController>();
    }

    public override void ReturnToPool()
    {
        base.ReturnToPool();
        attachPoint.ToFree();
    }

    public override void Initialize()
    {
        base.Initialize();
        attachPoint = null;
    }

    public void SetAttach(MergePoint mergePoint)
    {
        if (attachPoint != null && attachPoint != mergePoint)
        {
            attachPoint.ToFree();
        }

        attachPoint = mergePoint;
    }

    public void SetFly(bool isFly)
    {
        mergeObjectAnimatorController.ToFly(isFly);
    }

    public void ChangeOutlineColor(bool isOutlineActive, int intCheckLevel)
    {
        outline.enabled = isOutlineActive;
        outline.OutlineColor = intCheckLevel == level ? canBeMerge : cantBeMerge;
    }

    public void ReturnToAttachPoint()
    {
        attachPoint.AttachMergeObject(this);
    }

    public void Initialize(int level)
    {
        this.level = level;
    }
}