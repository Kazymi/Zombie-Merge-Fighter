using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MergeObjectAnimatorController : MonoBehaviour
{
    private Dictionary<MergeObjectAnimationType, int> hashes;
    private Animator animator;

    protected enum MergeObjectAnimationType
    {
        Idle,
        Fly
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        InitializeHash();
    }

    private void InitializeHash()
    {
        hashes = new Dictionary<MergeObjectAnimationType, int>();
        foreach (MergeObjectAnimationType mergeObjectAnimationType in Enum.GetValues(typeof(MergeObjectAnimationType)))
        {
            hashes.Add(mergeObjectAnimationType, Animator.StringToHash(mergeObjectAnimationType.ToString()));
        }
    }

    public void ToFly(bool isFly)
    {
        animator.SetBool(hashes[MergeObjectAnimationType.Fly], isFly);
    }
}