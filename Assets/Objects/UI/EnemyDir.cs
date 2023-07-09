using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Transform = UnityEngine.Transform;
using Cinemachine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyDir : MonoBehaviour
{
    public Transform fish;
    public Transform boat;
    public GameObject pointer;

    public CinemachineVirtualCamera virtualCamera;
    public float hideDistance = 10f;
    public float pointerOffset = 2f; 
    void Start()
    {
        fish = GameObject.Find("Fish").transform;
        boat = GameObject.Find("Boat").transform;
        pointer = GameObject.Find("EnemySearch");
        virtualCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        pointer.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (boat!=null && fish!=null)
        {
            Vector2 direction = boat.position - fish.position;

            direction.Normalize();

            float distance = Vector2.Distance(boat.position, fish.position);

            if (distance <= hideDistance)
            {
                pointer.SetActive(false);
            }
            else
            {
                pointer.SetActive(true);
                float angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
                pointer.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            }

        }
    }

}
