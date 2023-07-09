using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    [SerializeField] float boatSpeed = 1f;
    [SerializeField] float rotateSpeed = 1f;

    public int boatHP;
    public int boatMaxHP;

    private Rigidbody2D rb;
    private Vector2 randomDirection;
    private float changeDirectionTime;

    Vector2 avoidanceDirection;
    bool isCollide = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        randomDirection = GetRandomDirection();
        changeDirectionTime = Random.Range(1f, 3f);
    }

    private void Update()
    {
        DyingCheck();
        // Update the timer to change direction
        changeDirectionTime -= Time.deltaTime;

        if (changeDirectionTime <= 0f)
        {
            // Change direction and reset the timer
            if (isCollide)
            {
                randomDirection = avoidanceDirection;
                isCollide = false;
            }
            else if (transform.position.x <= -20 || transform.position.x >= 20 || transform.position.y > 10 || transform.position.y < -10)
            {
                randomDirection = -(transform.position - new Vector3(0, 0, 0));
            }
            else
            {
                randomDirection = GetRandomDirection();
            }

           changeDirectionTime = Random.Range(1f, 3f);
        }

        // Rotate the boat to face the movement direction
        if (randomDirection != Vector2.zero)
        {

                float angle = Mathf.Atan2(randomDirection.y, randomDirection.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }

        rb.velocity = transform.forward * boatSpeed;

        Vector2 movement = rb.transform.up * boatSpeed * Time.deltaTime;
        rb.position += movement;
    }

    private Vector2 GetRandomDirection()
    {
        float angle = Random.Range(0f, 360f);
        Vector2 direction = Quaternion.Euler(0f, 0f, angle) * Vector2.right;
        return direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //DIscover and avoid fish
        if (collision.gameObject.GetComponent<FishController>())
        {
            Debug.Log("Discover!");
            isCollide = true;
            changeDirectionTime = 0;

            Vector2 directionToFish = collision.transform.position - transform.position;
            avoidanceDirection = -directionToFish.normalized;
        }

        //Back to battle field
        if (collision.CompareTag("BoundaryStone"))
        {
            Debug.Log("Boundary!");
            isCollide = true;
            changeDirectionTime = 0;

            avoidanceDirection = -(collision.transform.position - new Vector3(0, 0, 0));
        }
    }

    void DyingCheck()
    {
        if (boatHP <= 0)
        {
            //Winning Code
            GameObject.Find("EventSystem").GetComponent<GameCOntroller>().stage=1;
            Destroy(gameObject);
        }
    }
}