using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    public GameManager gm;

    Rigidbody2D rb;
    Animator animator;
    [SerializeField] Transform groundCheckCollider;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform respawnLocation;

    [SerializeField] float speed = 2;
    float runSpeed = 1.75f;
    float horizontalValue;
    [SerializeField] float jumpPower;
    [SerializeField] int totalJumps;
    int availableJumps = 0;

    bool rightDirection = true;
    bool isRunning = false;
    bool isGrounded = false;
    bool multipleJump;
    
    //Constant
    const float groundCheckRadius = 0.2f;

    private void Awake()
    {
        availableJumps = totalJumps;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        //Thay doi gia tri yVelocity cua animator, neu dang nhay len thi nhan gia tri duong, roi xuong thi nhan gia tri am
        animator.SetFloat("yVelocity", rb.velocity.y);
        //bam nut sang trai thi nhan gia tri -1, sang phai 1, khong bam gi thi la 0
        horizontalValue = Input.GetAxisRaw("Horizontal");
        //Giu shift de chay nhanh
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }
        //Neu nhan nut space thi set cho jump = true va nguoc lai, bien jump luu trang thai da nhay hay chua
        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        GroundCheck();
        move(horizontalValue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Coin":
                AudioManager.instance.playMusic("coin");
                Destroy(collision.gameObject);
                gm.updateScore(10);
                break;
            case "Trap":
                AudioManager.instance.playMusic("hurt");
                gm.LoseLive();
                transform.position = respawnLocation.position;
                break;
            case "Bottom":
                AudioManager.instance.playMusic("hurt");
                gm.LoseLive();
                transform.position = respawnLocation.position;
                break;
            case "Finish":

                gm.NextLevel();
                break;
            case "Cherry":
                AudioManager.instance.playMusic("coin");
                Destroy(collision.gameObject);
                gm.addLive();
                break;
            case "Win":
                gm.victory();
                break;
        }
    }

    void GroundCheck()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;
        //Kiem tra xem GroundCheck trong Player co va cham voi cac collider 2d khac
        //co layer la ground hay khong
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        //Neu coliders co so phan tu > 0 thi dang cham dat
        if (colliders.Length > 0)
        {
            isGrounded = true;
            //Neu vua nay khong cham dat (vua nhay len xong)
            if(!wasGrounded)
            {
                availableJumps = totalJumps;
                multipleJump = false;
            }
            //Neu co tag platform
            foreach(var c in colliders)
            {
                if(c.tag == "Platform")
                {
                    transform.parent = c.transform;
                }
            }
        }
        else
        {
            transform.parent = null;
        }
        //Neu dang cham dat thi set bien jump cua animator la false va nguoc lai
        animator.SetBool("Jump", !isGrounded);

    }
    void move(float direction)
    {    
        //gia tri * toc do * gia tri fix cung deltatime
        float xValue = direction * speed * 100 * Time.fixedDeltaTime;
        //Neu dang trong trang thai chay thi nhan xvalue voi he so chay
        if (isRunning)
            xValue *= runSpeed;
        //Update vi tri moi
        Vector2 taggetVelocity = new Vector2(xValue, rb.velocity.y);

        rb.velocity = taggetVelocity;

        //Neu dang ben trai va di sang phai thi quay mat va nguoc lai
        if(rightDirection && direction < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            rightDirection = false;
        }
        else if(!rightDirection && direction > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            rightDirection = true;
        }

        //Dat gia tri xVelocity trong Moving Tree dua theo gia tri velocity cua rigidbody
        //Voi 0 la idle, 4 la walk, 7 la run
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
    }
    void Jump()
    {
        //Neu nhan vat dang chamd dat thi co the nhay duoc
        if (isGrounded)
        {
            multipleJump = true;
            //Giam so lan multiple jump di 1
            availableJumps --;
            AudioManager.instance.playMusic("jump");
            rb.velocity = Vector2.up * jumpPower;
            
        }
        else
        {
            if (multipleJump && availableJumps > 0)
            {
                availableJumps --;
                AudioManager.instance.playMusic("jump");
                rb.velocity = Vector2.up * jumpPower;
               
            }
        }
    }
}
