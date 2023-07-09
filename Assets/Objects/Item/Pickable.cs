using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    public float type;
    public float boosterTime;
    public float boosterAmount;
    public float existTime = 10f;

    private void Start()
    {
        Destroy(gameObject, existTime);
    }

}
