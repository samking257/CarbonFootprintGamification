using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    public GameObject txt1;
    public GameObject txt2;
    public GameObject txt3;
    public GameObject txt4;
    public GameObject txt5;
    public GameObject txt6;
    public GameObject txt7;
    public GameObject txt8;
    public GameObject flashTxt;
    private int increment = 0;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(0)){
            increment = increment + 1;
            switch (increment)
            {
                case 1:
                    txt1.SetActive(false);
                    txt2.SetActive(true);
                    break;
                case 2:
                    txt2.SetActive(false);
                    txt3.SetActive(true);
                    break;
                case 3:
                    txt3.SetActive(false);
                    txt4.SetActive(true);
                    break;
                case 4:
                    txt4.SetActive(false);
                    txt5.SetActive(true);
                    break;
                case 5:
                    txt5.SetActive(false);
                    txt6.SetActive(true);
                    break;
                case 6:
                    txt6.SetActive(false);
                    txt7.SetActive(true);
                    break;
                case 7:
                    txt7.SetActive(false);
                    txt8.SetActive(true);

                    flashTxt.GetComponent<Text>().text = "Click to Start...";
                    InvokeRepeating("FlashText", 0f, 0.5f);
                    break;
                case 8:
                    SceneManager.LoadScene("GameScene");
                    break;
            }
        }
    }

    void FlashText(){
        if(flashTxt.activeInHierarchy){
            flashTxt.SetActive(false);
        } else{
            flashTxt.SetActive(true);
        }
    }
}
