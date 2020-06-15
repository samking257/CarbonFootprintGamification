using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public void LoadGameScene(){
        SceneManager.LoadScene("GameScene");
    }

    public void LoadIntroScene(){
        SceneManager.LoadScene("IntroScene");
    }
}
