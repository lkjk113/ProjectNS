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
    /// 虚拟摇杆动作
    /// </summary>
    InputActions VirtualAction;
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

        //虚拟摇杆
        //获取border对象的transform组件  
        initPosition = stick.transform.localPosition;
        r = Vector3.Distance(stick.transform.localPosition, border.transform.localPosition);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        animator.SetBool("landing", Landing);
        animator.SetBool("landed", Landed);
        var action = InputActions.None;
        foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(keyCode))
            {
                action = GetAction(keyCode);
                DoAction(action);
            }
        }
        DoAction(VirtualAction);

        if (action != InputActions.MoveLeft && action != InputActions.MoveRight && action != InputActions.Hit)
        {
            if ((VirtualAction & (InputActions.MoveLeft | InputActions.MoveRight | InputActions.Hit)) == 0)
            {
                //没有按左右键时
                animator.SetFloat("speed", 0);
                if (Landed && !Jumping && !Crouching && !Hitting)//复位
                    animator.Play("Idle");
            }
        }
    }

    void DoAction(InputActions action)
    {
        bool CrouchPress = false;
        if ((action & InputActions.Jump) == InputActions.Jump)//跳跃
        {
            if (Landed && !Jumping && !Crouching)
            {
                //播放上跳动画
                animator.Play("Jump");
                Jumping = true;
                StartCoroutine(Jump());
            }
        }
        if ((action & InputActions.Crouch) == InputActions.Crouch)//蹲下
        {
            CrouchPress = true;
            if (Landed)
            {
                if (!Crouching)
                    //播放下蹲动画
                    animator.Play("Crouch");
                Crouching = true;
            }
        }

        if ((action & InputActions.Hit) == InputActions.Hit)//攻击1
        {
            if (!Landing)//空中攻击
            {
                StartCoroutine(Jumphit());
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
        }

        if ((action & InputActions.MoveLeft) == InputActions.MoveLeft)//左移
            StartCoroutine(Move(true));
        if ((action & InputActions.MoveRight) == InputActions.MoveRight)//右移
            StartCoroutine(Move(false));

        if (!CrouchPress)
        {
            Crouching = false;
        }
    }

    /// <summary>
    /// 移动
    /// </summary>
    /// <returns></returns>
    IEnumerator Move(bool towardsLeft)
    {
        spriteRenderer.flipX = towardsLeft; //转身

        if (Hitting)//移动攻击 
        {
            animator.Play("Hit");
            moveSpeed = 6;
            transform.Translate((towardsLeft ? Vector2.left : Vector2.right) * moveSpeed * Time.deltaTime);
        }

        if (!Crouching && !Hitting)//站立攻击
        {
            rigidbody.gravityScale = 11;
            if (!Landed)
            {
                moveSpeed = 21;
                //if (!Jumping)//坠落
                //    animator.Play("Fall");
            }
            else
            {
                moveSpeed = 25;
                animator.Play("Run");
            }
            animator.SetFloat("speed", moveSpeed);
            transform.Translate((towardsLeft ? Vector2.left : Vector2.right) * moveSpeed * Time.deltaTime);
        }

        yield return null;
    }

    /// <summary>
    /// 普通攻击
    /// </summary>
    IEnumerator Hit()
    {
        animator.Play("Hit");
        yield return new WaitForSeconds(0.2f);
        Hitting = false;
    }

    IEnumerator Jumphit()
    {
        animator.Play("Jumphit");
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
        animator.Play("Fall");
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

















    //虚拟方向按钮初始位置  
    Vector3 initPosition;
    //虚拟方向按钮可移动的半径  
    float r;
    //border对象  
    public GameObject border;

    public GameObject stick;

    //鼠标拖拽  
    public void OnDragIng()
    {

        stick.transform.localPosition = Input.mousePosition;

        if (Vector3.Distance(Input.mousePosition, initPosition) > 64)        //如果鼠标到虚拟键盘原点的位置 > 半径r  
        {
            //计算出鼠标和原点之间的向量  
            Vector3 dir = Input.mousePosition - initPosition;
            //这里dir.normalized是向量归一化的意思，实在不理解你可以理解成这就是一个方向，就是原点到鼠标的方向，乘以半径你可以理解成在原点到鼠标的方向上加上半径的距离  
            stick.transform.localPosition = initPosition + dir.normalized * 64;
        }

        var moveRange = stick.transform.localPosition - initPosition;


        if (stick.transform.localPosition.x < 0f)//左移
        {
            ResetAction();
            AddAction(InputActions.MoveLeft);
        }

        if (stick.transform.localPosition.x > 0f)//右移
        {
            ResetAction();
            AddAction(InputActions.MoveRight);
        }

        if (stick.transform.localPosition.y > r * 0.5 && !Jumping)//跳跃
            AddAction(InputActions.Jump);
        else
            RemoveAction(InputActions.Jump);

        if (stick.transform.localPosition.y < -r * 0.5)//下蹲
        {
            ResetAction();
            AddAction(InputActions.Crouch);
        }


    }
    //鼠标松开  
    public void OnDragEnd()
    {
        //松开鼠标虚拟摇杆回到原点  
        stick.transform.localPosition = initPosition;

        ResetAction();
    }



    void AddAction(InputActions another)
    {
        if ((VirtualAction & another) != another)
            VirtualAction = (InputActions)((int)VirtualAction + (int)another);
    }

    void RemoveAction(InputActions another)
    {
        if ((VirtualAction & another) == another)
            VirtualAction = (InputActions)((int)VirtualAction - (int)another);
    }

    void ResetAction()
    {
        VirtualAction = InputActions.None;
    }
}
