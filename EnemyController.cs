using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    /*
    
    public float speed;
    
    
    public float sideOffset;
    */
    public float upDist;
    public float xaxisDist;
    public Transform ceilingDetect;
    public Transform downDetect;
    public Transform rightDetect;
    public Transform leftDetect;
    private bool playerDetected;
    public float agroRange;
    public float chaseSpeed;
    public float jumpForce;
    public float doubleJumpForce;
    public float rayDist;
    private bool facingLeft;
    public BoxCollider2D box1;  //check platforms
    public BoxCollider2D box2;  //check player
    public BoxCollider2D box3;  //check player
    public Transform groundDetect;
    private Rigidbody2D rb;
    public float health;
    public float damage;
    private Vector2 velocity;
    private Vector2 velocity2;
    public float patrolSpeed;
    public float Yspeed;
    private Animator anim;
    private SpriteRenderer sr;
    private int sign = -1;
    // Start is called before the first frame update
    void Start()
    {

        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerDetected = false;
        anim.SetBool("playerDetected", false);
        anim.SetBool("InAttackRange", false);
        velocity = new Vector2(chaseSpeed, Yspeed);
        velocity2 = new Vector2(-chaseSpeed, Yspeed);
        anim.SetBool("isDead", false);
        sr.flipX = false;
        facingLeft = true;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        print("Patrolspeed = 9, actual speed =" + rb.velocity.x);
        
        //distance to player
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        //print("Dist to player" + distToPlayer + "Player detected" + playerDetected);

        if (distToPlayer < agroRange)
        {
            //chase player
            playerDetected = true;
            anim.SetBool("playerDetected", true);
            //ChasePlayer();
        }
        if (distToPlayer <= 5.3)
        {
            anim.SetBool("InAttackRange", true);
        }
        else if (distToPlayer > 5.3)
        {
            anim.SetBool("InAttackRange", false);
        }
        if (playerDetected == true)
        {
            ChasePlayer();
        }
        else if (playerDetected == false)
        {
            idle();
        }
        
        //idle();
    }
    
    void ChasePlayer()
    {
        RaycastHit2D downCheck = Physics2D.Raycast(downDetect.position, Vector2.down, rayDist);
        RaycastHit2D upCheck = Physics2D.Raycast(ceilingDetect.position, Vector2.up, upDist);
        RaycastHit2D leftCheck = Physics2D.Raycast(leftDetect.position, Vector2.left, xaxisDist);
        RaycastHit2D rightCheck = Physics2D.Raycast(rightDetect.position, Vector2.right, xaxisDist);


        if (transform.position.x < player.position.x)           //if player is right of the enemy
        {
            if( facingLeft == true)
            {
                facingLeft = false;
                sr.flipX = true;
            }
            moveSpeed(1, chaseSpeed);
            cappedVelocity(chaseSpeed);


        }

        else if (transform.position.x > player.position.x)      //if player is to the left of the enemy
        {
            if (facingLeft == false)
            {
                facingLeft = true;
                sr.flipX = false;
            }
            moveSpeed(-1, chaseSpeed);
            cappedVelocity(chaseSpeed);
        }
    }
    
    void moveSpeed(int sign, float speed)
    {
        if( sign < 0) { 
        rb.AddForce(Vector2.left * speed * 2f, ForceMode2D.Impulse);
        rb.AddForce(Vector2.left * speed, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(Vector2.right * speed * 2f, ForceMode2D.Impulse);
            rb.AddForce(Vector2.right * speed, ForceMode2D.Impulse);
        }
    }
    void cappedVelocity(float speed)
    {
        float cappedPatrol = Mathf.Min(Mathf.Abs(rb.velocity.x), speed) * Mathf.Sign(rb.velocity.x);
        //rb.AddForce(Vector2.left * cappedPatrol, ForceMode2D.Impulse);
        rb.velocity = new Vector2(cappedPatrol, rb.velocity.y);
    }
    void idle()
    {
        
        //rb.transform.Translate(Vector2.right * patrolSpeed * Time.deltaTime);
        //RaycastHit2D groundCheck = Physics2D.Raycast(groundDetect.position, Vector2.down, rayDist);
        RaycastHit2D leftCheck = Physics2D.Raycast(leftDetect.position, Vector2.left, xaxisDist);
        RaycastHit2D rightCheck = Physics2D.Raycast(rightDetect.position, Vector2.right, xaxisDist);
        
        if (rb.velocity.x == 0)
        {
            if (leftCheck.collider == true)
            {
                rb.AddForce(Vector2.right * patrolSpeed, ForceMode2D.Impulse);
                facingLeft = false;
            }
            else if (rightCheck.collider == true)
            {
                rb.AddForce(Vector2.left * patrolSpeed, ForceMode2D.Impulse);
                facingLeft = true;
            }
            else
            {
                rb.AddForce(Vector2.left * patrolSpeed, ForceMode2D.Impulse);
                facingLeft = true;
            }
        }
        if (facingLeft == true)
        {
            if (leftCheck.collider == true)
            {
                //print("Left works");
                moveSpeed(1, patrolSpeed);
                cappedVelocity(patrolSpeed);
                sr.flipX = true;
                facingLeft = false;
                /*transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
                */
                //rb.AddForce(Vector2.right * patrolSpeed, ForceMode2D.Impulse);
            }
            else
            {
                sr.flipX = false;
                //transform.eulerAngles = new Vector3(0, 0, 0);
                //movingRight = true;
            }
        }
        if (facingLeft == false)
        {
            if (rightCheck.collider == true)
            {
                //print("right works");
                moveSpeed(-1, patrolSpeed);
                cappedVelocity(patrolSpeed);
                sr.flipX = false;
                facingLeft = true;
                /*transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
                */
                //rb.AddForce(Vector2.right * patrolSpeed, ForceMode2D.Impulse);
            }
            else
            {
                sr.flipX = true;
                //transform.eulerAngles = new Vector3(0, 0, 0);
                //movingRight = true;
            }
        }
        

    }

    
   void OnTriggerEnter2D(Collider2D col)
   {
        switch (col.tag)
        {
            case "Platform":
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                break;
            case "Bullet":
                Destroy(col.gameObject);
                //Destroy(gameObject);
                TakeDamage();
                break;



        }
    }
    
    public void TakeDamage()
    {
        health = health - damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //Instantiate(deathEffect, transform.position, Quaternion.identity);
        anim.SetBool("isDead", true);
        Destroy(gameObject);
    }
    
}
