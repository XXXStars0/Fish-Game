using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1f;
    public float existTime = 5f;
    public GameObject WebPrefab;

    private void Start()
    {
        Destroy(gameObject, existTime);
    }
    void Update()
    {
        // 控制子弹向前移动
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameObject web = Instantiate(WebPrefab, transform.position, transform.rotation);
    }
}
