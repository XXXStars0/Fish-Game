using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class FishController : MonoBehaviour
{
    [Header("Fish State")]
    [SerializeField] float fishSpeed = 1f;
    [SerializeField] float rotateSpeed = 1f;

    float baseSpeed;
    float baseRotate;
    float basePow;

    public int fishHP;
    public int fishMaxHP;
    public int fishAP;

    private Rigidbody2D rb;

    float horizontalInput;
    float verticalInput;

    //Camera
    [Header("Shaking Camera")]
    public float shakeDuration = 2f; 
    public float shakeAmplitude = 1f;
    public float shakeFrequency = 1f; 
    private float shakeElapsedTime = 0f;
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    bool startShake;

    [Header("Booster")]
    public float spdBoost;
    public float powBoost;
    public float rotBoost;

    [Header("UI Control")]
    [SerializeField] public Slider HPBar;
    [SerializeField] TMP_Text SLV;
    [SerializeField] TMP_Text RLV;
    [SerializeField] TMP_Text PLV;
    [SerializeField] Animator screenFlash;

    int speedLV = 0;
    int rotateLV = 0;
    int attackLV = 0;
    void Start()
    {
        //get RIgid body
        rb = GetComponent<Rigidbody2D>();

        //Camera Control
        virtualCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        startShake = false;
        //Reset Timer
        spdBoost = 0f;
        powBoost = 0f;
        rotBoost = 0f;

        //Base State
        baseSpeed = fishSpeed;
        basePow = fishAP;
        baseRotate = rotateSpeed;

        //UI Connection
        HPBar = GameObject.Find("Fish_HP").GetComponent<Slider>();

        HPBar.maxValue = fishMaxHP;
        HPBar.value = fishHP;

        speedLV = 0;
        rotateLV = 0;
        attackLV = 0;

        SLV = GameObject.Find("Text_SpeedLV").GetComponent<TMP_Text>();
        RLV = GameObject.Find("Text_RotLV").GetComponent<TMP_Text>();
        PLV = GameObject.Find("Text_AtkLV").GetComponent<TMP_Text>();
        setUITexts();

        screenFlash = GameObject.Find("Panel_MainUI").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        controller();
        Shake();
        DyingCheck();
        setUITexts();
        //abilityBoost();
        HPBar.value = fishHP;
    }

    private void setUITexts()
    {
        SLV.text = "SPD LV:" + speedLV.ToString();
        RLV.text = "ROT LV:" + rotateLV.ToString();
        PLV.text = "ATK LV:" + attackLV.ToString();
    }

    void abilityBoost()
    {
        if (spdBoost <= 0f)
        {
            fishSpeed = baseSpeed;
        }
        else
        {
            spdBoost -=Time.deltaTime;
        }

        if(rotBoost <= 0f)
        {
            rotateSpeed = baseRotate;
        }
        else
        {
            rotBoost -=Time.deltaTime;
        }

        if(powBoost < 0f)
        {
            fishAP = (int)basePow;
        }
        else
        {
            powBoost -=Time.deltaTime;
        }
    }

    private void controller()
    {
        // Read Input
        horizontalInput = Input.GetAxis("Horizontal");
        //verticalInput = Input.GetAxis("Vertical");

        // Cannot control fish front movement
        verticalInput = 1f;

        Vector2 movement = rb.transform.up * verticalInput * fishSpeed * Time.deltaTime;
        rb.position += movement;

        //Rotation COntrol
        transform.Rotate(0f, 0f, horizontalInput * rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Collide Boat
        if (collision.CompareTag("BoatCheck"))
        {
            //Check Boat
            GameObject.Find("Boat").GetComponent<BoatController>().boatHP -= fishAP;
            startShake = true;
        }
        //Collide Stone
        if (collision.CompareTag("BoundaryStone"))
        {
            //TO DO: FIsh cannot reach boundary
            fishHP = 0;
            //fishHP = 0;
        }
        //Boost Item
        if (collision.CompareTag("PickableItem"))
        {
            switch (collision.gameObject.GetComponent<Pickable>().type)
            {
                case 1:
                    speedLV += 1;
                    spdBoost = collision.gameObject.GetComponent<Pickable>().boosterTime;
                    fishSpeed += collision.gameObject.GetComponent<Pickable>().boosterAmount;
                    break;

                case 2:
                    rotateLV += 1;
                    rotBoost = collision.gameObject.GetComponent<Pickable>().boosterTime;
                    rotateSpeed += collision.gameObject.GetComponent<Pickable>().boosterAmount;
                    break;

                case 3:
                    attackLV += 1;
                    powBoost = collision.gameObject.GetComponent<Pickable>().boosterTime;
                    fishAP += (int)collision.gameObject.GetComponent<Pickable>().boosterAmount;
                    break;

                case 4:
                    int heal = (int)collision.gameObject.GetComponent<Pickable>().boosterAmount;
                    if(fishHP + heal > fishMaxHP)
                    {
                        fishHP = fishMaxHP;
                    }
                    else
                    {
                        fishHP += heal;
                    }
                    break;
            }
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Web"))
        {
            fishHP -= collision.GetComponent<Web>().Damage;
            Destroy(collision.gameObject);
            screenFlash.SetTrigger("FlashRed");
        }
    }

    void Shake()
    {
        if (startShake)
        {
            if (shakeElapsedTime == 0)
            {
                virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
                virtualCameraNoise.m_FrequencyGain = shakeFrequency;
            }

            shakeElapsedTime += Time.deltaTime;

            if (shakeElapsedTime > shakeDuration)
            {
                startShake = false;
                virtualCameraNoise.m_AmplitudeGain = 1f;
                virtualCameraNoise.m_FrequencyGain = 1f;
                shakeElapsedTime = 0f;
            }
        }
    }

    void DyingCheck()
    {
        if (fishHP <= 0)
        {
            //Game Over Code
            GameObject.Find("EventSystem").GetComponent<GameCOntroller>().stage = 2;
            Destroy(gameObject);
        }
    }

}