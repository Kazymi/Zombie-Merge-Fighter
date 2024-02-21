using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MergePoint : MonoBehaviour
{
    [Header("Merge induced")] [SerializeField]
    private Color inducedColor;

    [SerializeField] private Color defaultColor;
    [SerializeField] private Image inducedImage;

    [Header("Merge position")] [SerializeField]
    private Transform centerPosition;

    private bool isInduced;
    private MergeObject currentMergeObject;

    private IMergeConstructor mergeConstructor;
    private IEffectSpawner effectSpawner;
    private IMergeObjectPool mergeObjectPool;

    public MergeObject CurrentMergeObject => currentMergeObject;

    public bool IsPointFree => currentMergeObject == null;

    public void SetInduced(bool isInduced)
    {
        if (isInduced == this.isInduced)
        {
            return;
        }

        this.isInduced = isInduced;
        inducedImage.DOKill();
        inducedImage.DOColor(isInduced ? inducedColor : defaultColor, 0.4f);
    }

    public void SetOutline(MergeObject mergeObject)
    {
        if (IsPointFree) return;
        if (currentMergeObject == null || currentMergeObject == mergeObject) return;
        if (mergeObject == null)
        {
            currentMergeObject.ChangeOutlineColor(false, 0);
        }
        else
        {
            currentMergeObject.ChangeOutlineColor(true, mergeObject.Level);
        }
    }

    public void ToFree()
    {
        currentMergeObject = null;
    }

    public void AttackAfterPool(MergeObject mergeObject)
    {
        mergeObject.transform.position = centerPosition.position;
        mergeObject.transform.DOShakeScale(0.3f, 0.2f)
            .OnComplete(() => mergeObject.transform.DOScale(Vector3.one, 0.2f));
        currentMergeObject = mergeObject;
        mergeObject.SetAttach(this);
    }

    public void AttachMergeObject(MergeObject mergeObject)
    {
        if (currentMergeObject == mergeObject)
        {
            mergeObject.transform.DOMove(centerPosition.position, 0.2f).SetEase(Ease.InFlash);
            mergeObject.SetAttach(this);
        }
        else
        {
            if (currentMergeObject == null)
            {
                currentMergeObject = mergeObject;
                mergeObject.transform.DOMove(centerPosition.position, 0.2f).SetEase(Ease.InFlash);
                mergeObject.SetAttach(this);
            }
            else
            {
                mergeConstructor ??= ServiceLocator.GetService<IMergeConstructor>();
                if (currentMergeObject.Level == mergeObject.Level)
                {
                    var newObject = mergeConstructor.TryMerge(mergeObject.Level + 1);
                    if (newObject != null)
                    {
                        effectSpawner ??= ServiceLocator.GetService<IEffectSpawner>();
                        effectSpawner.SpawnEffect(EffectType.MergeComplete, centerPosition);
                        currentMergeObject.ReturnToPool();
                        mergeObject.ReturnToPool();
                        mergeObjectPool ??= ServiceLocator.GetService<IMergeObjectPool>();
                        AttackAfterPool(mergeObjectPool.GetMergeObject(newObject.Level));
                    }
                    else
                    {
                        mergeObject.ReturnToAttachPoint();
                    }
                }
                else
                {
                    mergeObject.ReturnToAttachPoint();
                }
            }
        }
    }
}