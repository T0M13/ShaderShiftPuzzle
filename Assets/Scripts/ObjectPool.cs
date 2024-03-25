using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private readonly T prefab;
    private readonly int amount;
    private readonly Stack<T> objects = new Stack<T>();

    public ObjectPool(T prefab, int amount)
    {
        this.prefab = prefab;
        this.amount = amount;
    }

    public T Get()
    {
        if (objects.Count == 0)
            AddObjects(amount);
        return objects.Pop();
    }

    public void ReturnToPool(T objectToReturn)
    {
        objectToReturn.gameObject.SetActive(false);
        objects.Push(objectToReturn);
    }

    private void AddObjects(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var newObject = Object.Instantiate(prefab);
            newObject.gameObject.SetActive(false);
            objects.Push(newObject);
        }
    }
}
