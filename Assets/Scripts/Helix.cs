using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Helix : MonoBehaviour
{
    public PooledObject pooledObject;
    public List<Hoop> hoops;
    public bool isInitialLevel;

    public void Rotate(Vector3 euler)
    {
        transform.Rotate(euler);
    }

    public void SelfDestroy()
    {
        if (isInitialLevel)
            Destroy(gameObject);
        else
        {
            hoops.ForEach(x => x.SelfDestroy());
            ObjectPool.instance.TakeBack(pooledObject);
        }
    }

    public void DeactivateColliders()
    {
        return;
            hoops.ForEach(x => x.DeactivateColliders());
    }
}