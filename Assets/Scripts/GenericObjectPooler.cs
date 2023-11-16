using UnityEngine;
using UnityEngine.Pool;

public class GenericObjectPooler<T> where T : Component
{
    private ObjectPool<T> pool;

    public GenericObjectPooler(GameObject prefab, int defaultCapacity = 10, int maxSize = 50)
    {
        pool = new ObjectPool<T>(
            createFunc: () => {
                var newObj = Object.Instantiate(prefab).GetComponent<T>();
                newObj.gameObject.SetActive(false);
                return newObj;
            },
            actionOnGet: (obj) => obj.gameObject.SetActive(true),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Object.Destroy(obj.gameObject),
            collectionCheck: false,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );
    }

    public T Get()
    {
        return pool.Get();
    }

    public void Release(T obj)
    {
        pool.Release(obj);
    }
}