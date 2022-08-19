using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

public class Hoop : MonoBehaviour
{
    public PooledObject pooledObject;
    private List<Collider> colliders;

    private void Awake()
    {
        colliders = GetComponentsInChildren<Collider>().ToList();
    }

    public void SelfDestroy()
    {
        ActivateColliders();
        ObjectPool.instance.TakeBack(pooledObject);
    }

    public void DeactivateColliders()
    {
        colliders.ForEach(x => x.enabled = false);
    }

    private void ActivateColliders()
    {
        colliders.ForEach(x => x.enabled = true);
    }
}