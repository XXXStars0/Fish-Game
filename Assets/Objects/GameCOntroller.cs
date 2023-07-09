using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCOntroller : MonoBehaviour
{ 
    [SerializeField]GameObject Win;
    [SerializeField] GameObject Lose;
    public float stage = 0;

    // Start is called before the first frame update
    void Start()
    {
        Win = GameObject.Find("Panel_Win");
        Win.SetActive(false);
        Lose = GameObject.Find("Panel_Over");
        Lose.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(stage != 0)
        {
            Time.timeScale = 0f;
        }
        switch (stage)
        {
            case 1:
                //win
                Win.SetActive(true);
                break;
            case 2:
                //lose
                Lose.SetActive(true);
                break;
            default: break; 
        }
    }
}
