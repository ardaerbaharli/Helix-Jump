using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Helix : MonoBehaviour
{
    public List<HelixPart> helixParts;
    public PooledObject pooledObject;
    public bool didBlewUp;

    public void Rotate(Vector3 euler)
    {
        transform.Rotate(euler);
    }

    public void BlowUp(bool isSuperSpeed = false)
    {
        if (didBlewUp) return;
        didBlewUp = true;
        helixParts.ForEach(x => x.BlowUp(isSuperSpeed));
    }

    public void SelfDestroy()
    {
        didBlewUp = false;

        helixParts.ForEach(x => x.SelfDestroy());

        ObjectPool.instance.TakeBack(pooledObject);
    }
}