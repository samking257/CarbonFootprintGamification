using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AvatarManager : MonoBehaviour
{

    public GameObject avatarPanel;
    public GameObject AvatarButton1;
    public GameObject AvatarButton2;
    public GameObject AvatarButton3;
    public GameObject AvatarButton4;
    public GameObject AvatarButton5;

    public Sprite female2;
    public Sprite male1;
    public Sprite male2;
    public Sprite robot;

    public RuntimeAnimatorController anim1;
    public RuntimeAnimatorController anim2;
    public RuntimeAnimatorController anim3;
    public RuntimeAnimatorController anim4;

    [SerializeField]
    private int health;

    void Start()
    {
        AvatarButton1.GetComponent<Button>().onClick.AddListener(delegate{Avatar1();} );
        AvatarButton2.GetComponent<Button>().onClick.AddListener(delegate{Avatar3();} );
        AvatarButton3.GetComponent<Button>().onClick.AddListener(delegate{Avatar2();} );
        AvatarButton4.GetComponent<Button>().onClick.AddListener(delegate{Avatar4();} );
        AvatarButton5.GetComponent<Button>().onClick.AddListener(delegate{Avatar5();} );

        if(PlayerPrefs.HasKey("AvatarImg")){
            switch (PlayerPrefs.GetString("AvatarImg"))
            {
                case "female1":
                    avatarPanel.SetActive(false);
                    break;
                case "female2":
                    GetComponent<SpriteRenderer>().sprite = female2;
                    GetComponent<Animator>().runtimeAnimatorController = anim1 as RuntimeAnimatorController;
                    avatarPanel.SetActive(false);
                    break;
                case "male1":
                    GetComponent<SpriteRenderer>().sprite = male1;
                    GetComponent<Animator>().runtimeAnimatorController = anim2 as RuntimeAnimatorController;
                    avatarPanel.SetActive(false);
                    break;
                case "male2":
                    GetComponent<SpriteRenderer>().sprite = male2;
                    GetComponent<Animator>().runtimeAnimatorController = anim3 as RuntimeAnimatorController;
                    avatarPanel.SetActive(false);
                    break;
                case "robot":
                    GetComponent<SpriteRenderer>().sprite = robot;
                    GetComponent<Animator>().runtimeAnimatorController = anim4 as RuntimeAnimatorController;
                    avatarPanel.SetActive(false);
                    break;
            }
        }
        updateStatus();
    }

    void Update()
    {
        GetComponent<Animator>().SetBool("Cheer", gameObject.transform.position.y > -1.35f);
        if(gameObject.transform.position.y > -1.45f && gameObject.transform.position.y < -1.3f){
            GetComponent<Animator>().SetBool("Hit", true);
        } else{
            GetComponent<Animator>().SetBool("Hit", false);
        }

        if(Input.GetMouseButtonDown (0)){
            Vector2 vector = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(vector), Vector2.zero);
            if(hit){
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 9000f));
                
            }
        }
    }

    void updateStatus(){
        if(!PlayerPrefs.HasKey("health")){
            health = 500;
            PlayerPrefs.SetInt("health", health);
        } else {
            health = PlayerPrefs.GetInt("health");
        }

        if(!PlayerPrefs.HasKey("then")){
            PlayerPrefs.SetString("then", getStringTime());
        }

        TimeSpan ts = getTimeSpan();
        int points = 5;
        if(PlayerPrefs.HasKey("hourPoints")){
            points = PlayerPrefs.GetInt("hourPoints");
        }
        
        health -= (int)(ts.TotalHours * points);
        if(health < 0){
            health = 0;
        }
        PlayerPrefs.SetInt("health", health);

        InvokeRepeating("updateDevice", 0f, 30f);
    } 

    void updateDevice(){
        PlayerPrefs.SetString("then", getStringTime());
    }

    TimeSpan getTimeSpan(){
        return DateTime.Now - Convert.ToDateTime(PlayerPrefs.GetString("then"));
    }

    string getStringTime(){
        DateTime now = DateTime.Now;
        return now.Day + "/" + now.Month + "/" + now.Year + " " + now.Hour + ":" + now.Minute + ":" + now.Second;
    }

    public void Avatar1(){
        PlayerPrefs.SetString("AvatarImg", "female1");
        avatarPanel.SetActive(false);
    }

    public void Avatar2(){
        PlayerPrefs.SetString("AvatarImg", "female2");
        GetComponent<SpriteRenderer>().sprite = female2;
        GetComponent<Animator>().runtimeAnimatorController = anim1 as RuntimeAnimatorController;
        avatarPanel.SetActive(false);
    }

    public void Avatar3(){
        PlayerPrefs.SetString("AvatarImg", "male1");
        GetComponent<SpriteRenderer>().sprite = male1;
        GetComponent<Animator>().runtimeAnimatorController = anim2 as RuntimeAnimatorController;
        avatarPanel.SetActive(false);
    }

    public void Avatar4(){
        PlayerPrefs.SetString("AvatarImg", "male2");
        GetComponent<SpriteRenderer>().sprite = male2;
        GetComponent<Animator>().runtimeAnimatorController = anim3 as RuntimeAnimatorController;
        avatarPanel.SetActive(false);
    }

    public void Avatar5(){
        PlayerPrefs.SetString("AvatarImg", "robot");
        GetComponent<SpriteRenderer>().sprite = robot;
        GetComponent<Animator>().runtimeAnimatorController = anim4 as RuntimeAnimatorController;
        avatarPanel.SetActive(false);
    }
}
