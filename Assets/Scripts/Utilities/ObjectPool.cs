using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;

namespace Utilities
{
    [Serializable]
    public class ObjectToPool
    {
        public string name;
        public GameObject gameObject;
        public int amount;
    }

    [Serializable]
    public class PooledObject
    {
        public string name;
        public GameObject gameObject;
        public Transform transform;
        public Rigidbody rigidbody;
        public string poolName;
    }

    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool instance;
        public List<ObjectToPool> objectToPool;
        public Queue<PooledObject> pooledObjectsQ;
        public Dictionary<string, Queue<PooledObject>> poolDictionary;

        public bool isPoolSet;
        public GameObject parent;


        private void Awake()
        {
            instance = this;
            StartPool();
        }

        private void StartPool()
        {
            poolDictionary = new Dictionary<string, Queue<PooledObject>>();
            foreach (var item in objectToPool)
            {
                pooledObjectsQ = new Queue<PooledObject>();
                for (var i = 0; i < item.amount; i++)
                {
                    var obj = Instantiate(item.gameObject, parent.transform);

                    obj.SetActive(false);

                    Rigidbody rb = null;
                    if (obj.TryGetComponent(out Rigidbody r))
                        rb = r;

                    pooledObjectsQ.Enqueue(new PooledObject()
                    {
                        name = item.name,
                        gameObject = obj,
                        transform = obj.transform,
                        rigidbody = rb,
                        poolName = item.name
                    });
                }

                poolDictionary.Add(item.name, pooledObjectsQ);
            }

            isPoolSet = true;
        }


        public PooledObject GetPooledObject(string objectName)
        {
            if (!poolDictionary.ContainsKey(objectName))
            {
                return null;
            }

            var obj = poolDictionary[objectName].Dequeue();
            if (obj.gameObject.activeSelf)
                return GetPooledObject(objectName);

            obj.transform.rotation = Quaternion.identity;
            if (obj.rigidbody != null)
            {
                obj.rigidbody.useGravity = false;
                obj.rigidbody.velocity = Vector3.zero;
                obj.rigidbody.angularVelocity = Vector3.zero;
                obj.transform.rotation = Quaternion.identity;
            }

            obj.gameObject.SetActive(true);
            return obj;
        }

        public void TakeBack(PooledObject obj)
        {
            if (!gameObject.activeSelf) return;
            if (obj.gameObject == null) return;

            obj.gameObject.SetActive(false);
            var objectName = obj.name;
            poolDictionary[objectName].Enqueue(obj);
        }
    }
}