using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //基本信息
    //private Rigidbody2D rb;
    //private BoxCollider2D coll;
    public float moveSpeed = 5f;
    public Transform movePoint;

    //碰撞体层:碰撞体层不管遇到的是什么物体，只管这玩意能不能碰撞以及碰撞完能不能通过
    public LayerMask whatStopsMovement;
    public LayerMask whatCanPass;

    //物件交互
    //public bool isOn;
    IGround standingGround;//放地板
    IInteractableObject facingObject;//放面对物体

    //射线检测
    private bool needUpdateObject = false;
    public float triggerDistance = 1.1f;
    private Vector2 dir;
    private Vector2 moveOffset;

    //角色面向
    //private int direction = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        dir = Vector2.down ;
        //rb = GetComponent<Rigidbody2D>();
        //coll = GetComponent<BoxCollider2D>();
    }

    //转向
    /*Vector2 Direction()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) dir = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow)) dir = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) dir = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow)) dir = Vector2.right;
        return dir;
    }*/

    //Tips:上面这个写法返回dir很多余，另外为了下面方便,我顺便让它返回有没有变化方向。
    public bool UpdateDirection()
    {
        //更新方向
        Vector2 vec = dir;
        if (Input.GetKeyDown(KeyCode.UpArrow)) vec = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow)) vec = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) vec = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow)) vec = Vector2.right;
        //检查方向是否改变，并返回true/false
        if(vec == dir)
        {
            return false;
        }
        else
        {
            dir = vec;
            return true;
        }
    }

    //判断前方是否有物体
    void PhisicsCheck()
    {
        //layermask，两个合并
        LayerMask objectLayer = whatCanPass | whatStopsMovement; 
        //检查地面
        Collider2D groundCollider = Physics2D.OverlapCircle(movePoint.position, 0.2f, objectLayer);//理论上只要查whatCanPass,但多查一个也无所谓
        //获取接口
        IGround g;
        if (groundCollider == null)
        {
            g = null;
        }
        else
        {
            g = groundCollider.GetComponent<IGround>();
        }
        //更新接口
        if(g != standingGround) //检查是不是新地面
        {
            if (standingGround != null)
            {
                standingGround.Leave();//调用离开地面时的函数
            }
            standingGround = g; //赋值，更新
            if(standingGround != null)
            {
                standingGround.Enter(); //调用进入地面时函数
            }
        }
        //todo:检查面向物体
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,movePoint.position,moveSpeed * Time.deltaTime);//更新玩家位置
        if(Vector3.Distance(transform.position,movePoint.position)<= .05f)  //如果玩家已经到目的地
        {
            needUpdateObject = false;

            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f) //判断玩家输入
            {
                if (!Physics2D.OverlapCircle
                    (movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), //圆心
                    .2f, //半径
                    whatStopsMovement)) //layer
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);//移动movePoint
                    needUpdateObject = true;//接下来要更新交互物体
                }
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                if (!Physics2D.OverlapCircle
                    (movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f),//圆心
                    .2f, //半径
                    whatStopsMovement)) //layer 
                {
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                    needUpdateObject = true;
                }
            }

            //Tips:你的转向逻辑和移动逻辑不能放一起，移动!=转向，转向!=移动，而且将来还要放人物动画，你放一起根本不能处理视觉嘛
            needUpdateObject = needUpdateObject || UpdateDirection(); //更新方向同时，记录是否要更新面向的物体

            if (needUpdateObject)
            {
                PhisicsCheck();
            }

            //todo:物体交互
        }



        //我把phisicscheck移到了每一次移动后面。之后按照流程图，在Update里面另外加一个按Z判定，判断有没有记录物体，并且把这个tag的物体属性的布尔值打开（类似isOn)
        //物体本身因为类别比较多，想单拉脚本，每个物体一个，如果isOn检定是true就执行各自的效果，前方的和脚底下的都会执行。不过在做关卡的时候会避免同时执行的情况。

    }
}
