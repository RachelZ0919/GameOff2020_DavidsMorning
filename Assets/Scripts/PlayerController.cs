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

    //碰撞体层
    public LayerMask whatStopsMovement;
    public LayerMask Item;

    //物件交互
    public bool isOn;

    //射线检测
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
    Vector2 Direction()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) dir = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow)) dir = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) dir = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow)) dir = Vector2.right;
        return dir;
    }

    //判断前方是否有物体
    void PhisicsCheck()
    {

        //调用方向
        Vector2 vec = Direction();
        Vector2 pos = transform.position;
        RaycastHit2D frontCheck = Physics2D.Raycast(pos + vec, vec, triggerDistance, whatStopsMovement);
        if (frontCheck) isOn = true;
        else isOn = false;

        //这里本来还有一个布尔值，但后来发现和isOn没什么区别就删掉了，思路就是if(frontcheck)记录前方物体的tag，但我不知道怎么写代码确定这个物体在前方。
        //在frontcheck的同时也会做一个groundcheck，检测角色现在踩的这块地板有没有需要记录的tag。如果是未割就先变成已割再记录。

        //调试部分
        Color color = frontCheck ? Color.red : Color.green;
        Debug.DrawRay(pos + vec, vec, color, 1.1f);
        print(isOn);
    }

    // Update is called once per frame
    void Update()
    {

    //角色按格移动,遇到物体则被阻挡
        transform.position = Vector3.MoveTowards(transform.position,movePoint.position,moveSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position,movePoint.position)<= .05f){

            if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f){
                if(!Physics2D.OverlapCircle(movePoint.position +  new Vector3(Input.GetAxisRaw("Horizontal"),0f,0f),.2f,whatStopsMovement)){
                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"),0f,0f);
                PhisicsCheck();
                }
            }
            else if(Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f){
                 if(!Physics2D.OverlapCircle(movePoint.position +  new Vector3(0f,Input.GetAxisRaw("Vertical"),0f),.2f,whatStopsMovement)){              
                 movePoint.position += new Vector3(0f,Input.GetAxisRaw("Vertical"),0f);
                 PhisicsCheck();
                }
             }
         
         }

        //我把phisicscheck移到了每一次移动后面。之后按照流程图，在Update里面另外加一个按Z判定，判断有没有记录物体，并且把这个tag的物体属性的布尔值打开（类似isOn)
        //物体本身因为类别比较多，想单拉脚本，每个物体一个，如果isOn检定是true就执行各自的效果，前方的和脚底下的都会执行。不过在做关卡的时候会避免同时执行的情况。

    }
}
