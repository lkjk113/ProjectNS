using Assets.Enums;
using Assets.Scripts;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class CharactorController2d : MonoBehaviour
{

    public LayerMask groundLayer;

    SpriteRenderer spriteRenderer;
    Animator animator;
    float moveSpeed;
    Rigidbody2D rigidbody;
    Collider2D collider;
    float jumpTime;

    /// <summary>
    /// 是否蹲下
    /// </summary>
    bool Crouching;

    /// <summary>
    /// 是否正在上跳
    /// </summary>
    bool Jumping;

    /// <summary>
    /// 是否正在攻击
    /// </summary>
    bool Hitting;

    /// <summary>
    /// 即将落地
    /// </summary>
    bool Landing
    {
        get
        {
            return CheckIsGrounded(8);
        }
    }

    /// <summary>
    /// 已落地
    /// </summary>
    bool Landed
    {
        get
        {
            return CheckIsGrounded(3);
        }
    }

    /// <summary>
    /// 与地面距离检测
    /// </summary>
    /// <param name="distance">射线检测距离</param>
    /// <returns></returns>
    bool CheckIsGrounded(float distance)
    {
        Vector2 position = rigidbody.transform.position;
        Vector2 direction = Vector2.down;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        Jumping = false;
        jumpTime = 0.1f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        animator.SetBool("landing", Landing);
        animator.SetBool("landed", Landed);
        bool CrouchPress = false;
        var act = InputActions.None;
        foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(keyCode))
            {
                act = GetAction(keyCode);
                switch (act)
                {

                    case InputActions.MoveUp://上移
                        break;
                    case InputActions.MoveDown://下移
                        //gameObject.transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
                        break;
                    case InputActions.Jump://跳跃
                        if (Landed && !Jumping && !Crouching)
                        {
                            //播放上跳动画
                            animator.Play("PrincessJump");
                            Jumping = true;
                            StartCoroutine(Jump());
                        }
                        break;
                    case InputActions.Crouch://蹲下
                        CrouchPress = true;
                        if (Landed)
                        {
                            if (!Crouching)
                                //播放下蹲动画
                                animator.Play("PrincessCrouch");
                            Crouching = true;
                        }
                        break;
                    case InputActions.Hit://攻击1
                        if (!Landing)//空中攻击
                        {

                            StartCoroutine(Airit());
                        }
                        else if (Crouching)//下蹲攻击
                        {

                        }
                        else//普通攻击
                        {
                            if (!Hitting)
                            {
                                Hitting = true;
                                StartCoroutine(Hit());
                            }
                        }

                        break;
                    case InputActions.MoveLeft://左移
                    case InputActions.MoveRight://右移
                        StartCoroutine(Move(act == InputActions.MoveLeft));
                        break;
                    default:
                        break;
                }
            }
        }
        if (!CrouchPress)
        {
            Crouching = false;
        }
        if (act != InputActions.MoveLeft && act != InputActions.MoveRight && act != InputActions.Hit)//没有按左右键时
        {
            animator.SetFloat("speed", 0);
            if (Landed && !Jumping && !Crouching && !Hitting)//复位
                animator.Play("PrincessIdle");
        }
    }

    /// <summary>
    /// 移动
    /// </summary>
    /// <returns></returns>
    IEnumerator Move(bool moveLeft)
    {
        spriteRenderer.flipX = moveLeft; //转身

        if (Hitting)//移动攻击 
        {
            animator.Play("PrincessHit");
            moveSpeed = 6;
            transform.Translate((moveLeft ? Vector2.left : Vector2.right) * moveSpeed * Time.deltaTime);
        }

        if (!Crouching && !Hitting)//站立攻击
        {
            rigidbody.gravityScale = 11;
            if (!Landed)
            {
                moveSpeed = 21;
                //if (!Jumping)//坠落
                //    animator.Play("PrincessFall");
            }
            else
            {
                moveSpeed = 25;
                animator.Play("PrincessRun");
            }
            animator.SetFloat("speed", moveSpeed);
            transform.Translate((moveLeft ? Vector2.left : Vector2.right) * moveSpeed * Time.deltaTime);
        }

        yield return null;
    }

    /// <summary>
    /// 普通攻击
    /// </summary>
    IEnumerator Hit()
    {
        animator.Play("PrincessHit");
        yield return new WaitForSeconds(0.2f);
        Hitting = false;
    }

    IEnumerator Airit()
    {
        animator.Play("PrincessAirhit");
        yield return new WaitForSeconds(0.2f);
    }

    /// <summary>
    /// 上跳
    /// </summary>
    /// <param name="i"></param>
    IEnumerator Jump(int i = 0)
    {
        float startGravity = rigidbody.gravityScale;
        //重力设为0使其上升不受影响
        rigidbody.gravityScale = 0;

        //手动给与上升速度
        rigidbody.velocity = Vector2.up * 40;

        float timer = 0f;

        while (timer < jumpTime) //判断上跳时间是否已完
        {

            timer += Time.deltaTime;
            yield return null;

        }

        //切换下落
        StartCoroutine(Fall());

        //重力改回默认值
        rigidbody.gravityScale = startGravity;

    }

    /// <summary>
    /// 下落
    /// </summary>
    /// <returns></returns>
    IEnumerator Fall()
    {
        yield return new WaitForSeconds(0.3f);//等待上升到最高时刻再下降
        animator.Play("PrincessFall");
        Jumping = false;
        rigidbody.gravityScale = 10;
    }


    /// <summary>
    /// 根据按键获取动作
    /// </summary>
    /// <param name="keyCode"></param>
    /// <returns></returns>
    InputActions GetAction(KeyCode keyCode)
    {
        return InputSetting.Default.FirstOrDefault(r => r.Value == keyCode).Key;
    }


}
