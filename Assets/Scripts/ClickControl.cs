using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickControl : MonoBehaviour
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

    public void OnClick(string objType)
    {
        switch (objType)
        {
            case "StartGame":

                break;
            case "QuitGame":
                Application.Quit();
                break;

            case "Mute":
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
                break;
        }


    }
}
