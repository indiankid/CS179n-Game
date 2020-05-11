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
    private bool movingRight;
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
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerDetected = false;
        velocity = new Vector2(chaseSpeed, Yspeed);
        velocity2 = new Vector2(-chaseSpeed, Yspeed);
}

    // Update is called once per frame
    void Update()
    {
        
        //distance to player
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        print("Dist to player" + distToPlayer + "Player detected" + playerDetected);

        if (distToPlayer < agroRange)
        {
            //chase player
            playerDetected = true;
            //ChasePlayer();
        }
        if (playerDetected == true)
        {
            ChasePlayer();
        }
        else if (playerDetected == false)
        {
            idle();
        }
    }
    
    void ChasePlayer()
    {
        RaycastHit2D downCheck = Physics2D.Raycast(downDetect.position, Vector2.down, rayDist);
        RaycastHit2D upCheck = Physics2D.Raycast(ceilingDetect.position, Vector2.up, upDist);
        RaycastHit2D leftCheck = Physics2D.Raycast(leftDetect.position, Vector2.left, xaxisDist);
        RaycastHit2D rightCheck = Physics2D.Raycast(rightDetect.position, Vector2.right, xaxisDist);


        if (transform.position.x < player.position.x)           //if player is right of the enemy
        {

            //rb.velocity = Vector2.right * chaseSpeed;
            //rb.velocity = new Vector2(chaseSpeed, Yspeed);
            //rb.MovePosition((Vector2)transform.position + (velocity * Time.deltaTime));
            rb.AddForce(Vector2.right * chaseSpeed, ForceMode2D.Impulse);
            /*if (downCheck.collider == false)
            {
                if ((leftCheck.collider == true) || (rightCheck.collider == true))           //if there is no more platform area left:
                {
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

                }
            }
            */
        }

        else if (transform.position.x > player.position.x)      //if player is to the left of the enemy
        {
            //rb.velocity = new Vector2(-chaseSpeed, Yspeed);
            rb.AddForce(Vector2.left * chaseSpeed, ForceMode2D.Impulse);
           /* if (downCheck.collider == false)
            {
                if ((leftCheck.collider == true) || (rightCheck.collider == true))           //if there is no more platform area left:
                {
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                }
            }
            */
        }
    }
    
    void idle()
    {
        rb.transform.Translate(Vector2.right * patrolSpeed * Time.deltaTime);
        RaycastHit2D groundCheck = Physics2D.Raycast(groundDetect.position, Vector2.down, rayDist);

        if (groundCheck.collider == false)
        {
            if (movingRight)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
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
        /*
        if(col == box1)
        {
            switch (col.tag)
            {
            case "Platform":
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                break;
            }

        }
        if ((col == box2) || (col == box3))
        {
            switch (col.tag)
            {
                case "Player":
                    rb.AddForce(Vector2.up * doubleJumpForce, ForceMode2D.Impulse);
                    break;
            }

        }
 */
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
        Destroy(gameObject);
    }
    
}


/* void idle()
    {
        //rb.MovePosition(rb.position * velocity * Time.deltaTime);
        //rb.MovePosition((Vector2)transform.position + (velocity * Time.deltaTime));
        rb.velocity = new Vector2(patrolSpeed, Yspeed);
        RaycastHit2D groundCheck = Physics2D.Raycast(groundDetect.position, Vector2.down, rayDist);

        if (groundCheck.collider == false)
        {
            if (movingRight)
            {

                //rb.MovePosition((Vector2)transform.position +  (velocity2 * Time.deltaTime));
                rb.AddForce(Vector2.up * jumpForce);
                movingRight = false;
            }
            else
            {
                // rb.MovePosition((Vector2)transform.position + (velocity * Time.deltaTime));
                rb.AddForce(Vector2.up * jumpForce);
                movingRight = true;
            }
        }
    } */
