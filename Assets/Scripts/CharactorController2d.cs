using Assets.Enums;
using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharactorController2d : MonoBehaviour
{

    CharacterController ctrl = new CharacterController();


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(keyCode))
            {
                switch (GetAction(keyCode))
                {
                    case InputActions.MoveLeft://左移
                        gameObject.transform.Translate(Vector3.left * 5 * Time.deltaTime);
                        break;
                    case InputActions.MoveRight://右移
                        gameObject.transform.Translate(Vector3.right * 5 * Time.deltaTime);
                        break;
                    case InputActions.MoveUp://上移
                        break;
                    case InputActions.MoveDown://下移
                        break;
                    case InputActions.Jump://跳跃
                        if (ctrl.isGrounded)
                            gameObject.transform.Translate(Vector3.up * 10 * Time.deltaTime);
                        break;
                    case InputActions.Crouch://蹲下
                        gameObject.transform.Translate(Vector3.down * 5 * Time.deltaTime);
                        break;
                    case InputActions.Hit://攻击1
                        break;
                    default:
                        break;
                }
            }
        }
    }


    //void OnGUI()
    //{
    //    if (Input.anyKeyDown)
    //    {
    //        Event e = Event.current;
    //        if (e.isKey)
    //        {
    //            switch (GetAction(e.keyCode))
    //            {
    //                case InputActions.MoveLeft://左移
    //                    gameObject.transform.Translate(Vector3.left * 5 * Time.deltaTime);
    //                    break;
    //                case InputActions.MoveRight://右移
    //                    gameObject.transform.Translate(Vector3.right * 5 * Time.deltaTime);
    //                    break;
    //                case InputActions.MoveUp://上移
    //                    break;
    //                case InputActions.MoveDown://下移
    //                    break;
    //                case InputActions.Jump://跳跃
    //                    gameObject.transform.Translate(Vector3.up * 5 * Time.deltaTime);
    //                    break;
    //                case InputActions.Crouch://蹲下
    //                    gameObject.transform.Translate(Vector3.down * 5 * Time.deltaTime);
    //                    break;
    //                case InputActions.Hit://攻击1
    //                    break;
    //                default:
    //                    break;
    //            }
    //        }
    //    }
    //}


    public InputActions GetAction(KeyCode keyCode)
    {
        return InputSetting.Default.FirstOrDefault(r => r.Value == keyCode).Key;
    }
}
