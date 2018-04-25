using Assets.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualpadController : MonoBehaviour
{

    //虚拟方向按钮初始位置  
    Vector3 initPosition;
    //虚拟方向按钮可移动的半径  
    float r;
    //border对象  
    public GameObject border;

    public GameObject button;



    void Start()
    {
        //获取border对象的transform组件  
        initPosition = button.transform.position;
        r = Vector3.Distance(button.transform.position, border.transform.position);
    }
    //鼠标拖拽  
    public void OnDragIng()
    {
        //如果鼠标到虚拟键盘原点的位置 < 半径r  
        if (Vector3.Distance(Input.mousePosition, initPosition) < r)
        {
            //虚拟键跟随鼠标  
            button.transform.position = Input.mousePosition;
        }
        else
        {
            //计算出鼠标和原点之间的向量  
            Vector3 dir = Input.mousePosition - initPosition;
            //这里dir.normalized是向量归一化的意思，实在不理解你可以理解成这就是一个方向，就是原点到鼠标的方向，乘以半径你可以理解成在原点到鼠标的方向上加上半径的距离  
            button.transform.position = initPosition + dir.normalized * r;
        }

        var moveRange = button.transform.position - initPosition;

        if (moveRange.x > 0)//右移
        {

        }

    }
    //鼠标松开  
    public void OnDragEnd()
    {
        //松开鼠标虚拟摇杆回到原点  
        button.transform.position = initPosition;
    }
}