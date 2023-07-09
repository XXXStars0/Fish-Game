using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Transform fish;
    public GameObject bulletPrefab;

    public float rotationSpeed = 5f;
    public float fireInterval = 1f;
    //public float bulletSpeed = 5f;

    [SerializeField] float fireTimer = 0f;
    void Start()
    {
        fireTimer = fireInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (fish != null)
        {
            Vector3 targetDirection = fish.position - transform.position;

                Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, targetDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
                {
                    Debug.Log("Shoot");
                    fireTimer = fireInterval; 

                    GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                    //bullet.GetComponent<Bullet>().speed = bulletSpeed;
                }
        }
        else
        {
            fireTimer = fireInterval;
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<FishController>()) {
            fish = collision.transform;
        }
        else
        {
            fish = null;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<FishController>())
        {
            fish = null;
        }
    }
}
