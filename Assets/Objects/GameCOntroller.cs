using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameCOntroller : MonoBehaviour
{ 
    [SerializeField]GameObject Win;
    [SerializeField] GameObject Lose;
    [SerializeField] GameObject Pause;
    [SerializeField] GameObject MainUI;

    TMP_Text Btn_Pause_Text;

    public float stage = 0;

    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        Win = GameObject.Find("Panel_Win");
        Win.SetActive(false);
        Lose = GameObject.Find("Panel_Over");
        Lose.SetActive(false);
        Pause = GameObject.Find("Panel_Pause");
        Pause.SetActive(false);
        MainUI = GameObject.Find("Panel_MainUI");
        MainUI.SetActive(true);
        Btn_Pause_Text = GameObject.Find("Btn_Pause_Text").GetComponent<TMP_Text>();
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
                MainUI.SetActive(false);
                break;
            case 2:
                //lose
                Lose.SetActive(true);
                MainUI.SetActive(false);
                break;
            default: break; 
        }
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Btn_Pause_Text.text = "RESUME";
            Time.timeScale = 0f;
            Pause.SetActive(true);
        }
        else
        {
            Btn_Pause_Text.text = "PAUSE";
            Time.timeScale = 1f;
            Pause.SetActive(false);
        }
    }

    public void BackToMainMenu()
    {
        //Destroy A lot
        cleanClones();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void Retry()
    {
        cleanClones();
        SceneManager.LoadScene("BattleField");
    }

    void cleanClones()
    {
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in objects)
        {
            if (obj.GetComponent<Bullet>() || obj.GetComponent<Web>() || obj.CompareTag("PickableItem"))
            {
                Destroy(obj);
            }
        }
    }
}
