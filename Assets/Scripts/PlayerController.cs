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


    //角色面向
    //private int direction = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        //rb = GetComponent<Rigidbody2D>();
        //coll = GetComponent<BoxCollider2D>();
    }

    //转向
    Vector2 Direction()
    {
        Vector2 dir = new Vector2(0,-1);
        if (Input.GetKeyDown(KeyCode.UpArrow)) dir = new Vector2(0, 1);
        else if (Input.GetKeyDown(KeyCode.DownArrow)) dir = new Vector2(0, -1);
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) dir = new Vector2(-1, 0);
        else if (Input.GetKeyDown(KeyCode.RightArrow)) dir = new Vector2(1, 0);
        return dir;
    }

    //判断前方是否有物体
    void PhisicsCheck()
    {

        //调用方向
        Vector2 vec = Direction();

        Vector2 pos = transform.position;
        RaycastHit2D frontCheck = Physics2D.Raycast(pos, vec, triggerDistance, Item);
        if (frontCheck && Input.GetKeyDown(KeyCode.Z)) isOn = true;
        else isOn = false;
        Color color = frontCheck ? Color.red : Color.green;
        Debug.DrawRay(pos, vec, color, 1.1f);

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
                }
            }
            else if(Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f){
                 if(!Physics2D.OverlapCircle(movePoint.position +  new Vector3(0f,Input.GetAxisRaw("Vertical"),0f),.2f,whatStopsMovement)){              
                movePoint.position += new Vector3(0f,Input.GetAxisRaw("Vertical"),0f);
                }
             }
         
         }

        //调用射线
        PhisicsCheck();



         //角色与物体交互
         //输出角色朝向
         //if(Input.GetKeyDown(KeyCode.UpArrow))direction = 0;
         //else if(Input.GetKeyDown(KeyCode.DownArrow))direction = 1;
         //else if(Input.GetKeyDown(KeyCode.LeftArrow))direction = 2;
         //else if(Input.GetKeyDown(KeyCode.RightArrow))direction = 3;


    }
}
