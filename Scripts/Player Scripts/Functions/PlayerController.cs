using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    private float moveInput;
    private bool onGround;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask selectGround;
    public float numExtraJumps;
    private float extraJumps;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    //public GameObject ShootingArm;
    //private SpriteRenderer ShootingArmsr;
    //private Transform shootingArmtr;
    //private bool faceLeft = false;
    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        //ShootingArmsr = ShootingArm.GetComponent<SpriteRenderer>();
        //shootingArmtr = ShootingArm.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(onGround == true)
        {
            extraJumps = numExtraJumps;
        }
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) && (extraJumps > 0))
        {
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) && (extraJumps == 0) && (onGround == true))
        {
            rb.velocity = Vector2.up * jumpForce;
        }
 
    }
    void FixedUpdate()
    {

        onGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, selectGround);
        moveInput = Input.GetAxis("Horizontal");            //get the input for x-scale--- -1 means move left, 1 means move right
        
        
        rb.velocity = new Vector2(moveSpeed * moveInput, rb.velocity.y);
        //sr.flipX = 
        flipCharacter(moveInput);
        //ShootingArmsr.flipY = flipCharacter(moveInput);
        //shootingArmtr.position = new Vector3(shootingArmtr.position.x * -1, shootingArmtr.position.y, shootingArmtr.position.z);


    }
    void flipCharacter(float input)
    {
        if(input > 0)
        {
            //faceLeft = false;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if(input < 0)
        {
            //faceLeft = true;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        /*
        else
        {
            faceLeft = faceLeft;
        }
        */
        //return faceLeft;
        //  Vector3 scaler = transform.localScale;      //scaler to modify the current character's xzy values
        // scaler.x *= -1;                             //set the x scale to reverse of what it currently is
    }
    
}
