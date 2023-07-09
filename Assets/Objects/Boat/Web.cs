using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web : MonoBehaviour
{
    public int Damage = 1;
    public float existTime = 3f;
    public float growthRate = 0.1f;
    public float maxSize = 1;

    void Start()
    {
        Destroy(gameObject, existTime);
    }

    void Update()
    {
        Vector3 newScale = transform.localScale + Vector3.one * growthRate * Time.deltaTime;
        if (newScale.x <= maxSize && newScale.y <= maxSize)
        {
            transform.localScale = newScale;
        }
    }


}
