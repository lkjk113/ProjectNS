using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventStart : MonoBehaviour
{
    public AudioSource BGM;
    public Sprite imgMute;//定义待用的按钮图标
    public Sprite imgSpeak;
    public GameObject btnBGM;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMute()
    {
        if (BGM.isPlaying)
        {
            btnBGM.GetComponent<Image>().sprite = imgMute;
            BGM.Pause();
        }
        else
        {
            btnBGM.GetComponent<Image>().sprite = imgSpeak;
            BGM.Play();
        }
    }

    public void OnQuitGame()
    {
        Application.Quit();
    }

    public void OnStartGame()
    {
        SceneManager.LoadScene("Main");
    }

}
