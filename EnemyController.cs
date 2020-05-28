using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float xaxisDist;
    public Transform rightDetect;
    public Transform leftDetect;
    private bool playerDetected;
    public float agroRange;
    public float chaseSpeed;
    public float jumpForce;
    //public float doubleJumpForce;
    private bool facingLeft;
    private Rigidbody2D rb;
    public float health;
    public float damage;
    public float patrolSpeed;
    private Animator anim;
    private bool enemydead;
    public float attackRange;
    public float enemyPlayerDist;
    // Start is called before the first frame update
    void Start()
    {
        enemydead = false;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerDetected = false;
        anim.SetBool("playerDetected", false);
        anim.SetBool("InAttackRange", false);
        anim.SetBool("isDead", false);
        facingLeft = true;
        
    }

    // Update is called once per frame
    void Update()
    {

        //distance to player
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        //print("Dist to player" + distToPlayer + "Player detected" + playerDetected);
        if(enemydead == true)
        {
            anim.SetBool("InAttackRange", false);
            Die();
        }
        else if(enemydead == false){ 
            if (Mathf.Abs(distToPlayer) < agroRange )
            {
                //chase player
                playerDetected = true;
                anim.SetBool("playerDetected", true);
                //ChasePlayer();
            }
            if (distToPlayer <= attackRange)
            {
                anim.SetBool("InAttackRange", true);
            }
            else if (distToPlayer > attackRange)
            {
                anim.SetBool("InAttackRange", false);
            }
        
        }
        
    }
    void FixedUpdate()
    {
        
        if (playerDetected == true && enemydead == false)
        {
            ChasePlayer();
        }
        else if (playerDetected == false && enemydead == false)
        {
            idle();
        }
        
    }
    
    void ChasePlayer()
    {

        if (transform.position.x < (player.position.x - enemyPlayerDist))           //if player is right of the enemy
        {
            transform.localScale = new Vector3(-1, 1, 1);
            moveSpeed(1, chaseSpeed);
            cappedVelocity(chaseSpeed);
        }

        else if (transform.position.x > (player.position.x + enemyPlayerDist))      //if player is to the left of the enemy
        {
            transform.localScale = new Vector3(1, 1, 1);
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
    void Jump(float speed)
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
    void cappedVelocity(float speed)
    {
        float cappedPatrol = Mathf.Min(Mathf.Abs(rb.velocity.x), speed) * Mathf.Sign(rb.velocity.x);
        //rb.AddForce(Vector2.left * cappedPatrol, ForceMode2D.Impulse);
        rb.velocity = new Vector2(cappedPatrol, rb.velocity.y);
    }
    void cappedJump(float speed)
    {
        float capJump = Mathf.Min(Mathf.Abs(rb.velocity.y), speed);
        //rb.AddForce(Vector2.left * cappedPatrol, ForceMode2D.Impulse);
        rb.velocity = new Vector2(rb.velocity.x, capJump);
    }
    void idle()
    {
        facingLeft = true;
        RaycastHit2D leftCheck = Physics2D.Raycast(leftDetect.position, Vector2.left, xaxisDist);
        RaycastHit2D rightCheck = Physics2D.Raycast(rightDetect.position, Vector2.right, xaxisDist);
       

        if(rb.velocity.x == 0)              //move left initially
        {
            facingLeft = true;
            moveSpeed(-1, patrolSpeed);
            cappedVelocity(patrolSpeed);
        }
        if(leftCheck.collider == true)      //if something on the left
        {
            if(facingLeft == true)          //if facing left, then face right and move right
            {   
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                facingLeft = false;
                moveSpeed(1, patrolSpeed);
                cappedVelocity(patrolSpeed);   
            }
        }
        else if(rightCheck.collider == true)
        {
            if (facingLeft == false)          //if facing right, then face left and move left
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
                facingLeft = true;
                moveSpeed(-1, patrolSpeed);
                cappedVelocity(patrolSpeed);               
            }
        }
        

    }

    
   void OnTriggerEnter2D(Collider2D col)
   {
        if (enemydead == false)
        {
            switch (col.tag)
            {
                case "Platform":
                    Jump(jumpForce);
                    cappedJump(jumpForce);
                    break;
                case "Enemy":
                    Jump(jumpForce);
                    cappedJump(jumpForce);
                    break;
                case "Bullet":
                    Destroy(col.gameObject);
                    playerDetected = true;
                    //Destroy(gameObject);
                    TakeDamage();
                    break;



            }
        }
    }
    
    public void TakeDamage()
    {
        health = health - damage;

        if (health <= 0)
        {
            //Die();
            enemydead = true;
        }
    }

    void Die()
    {
        anim.SetBool("isDead", true);
        moveSpeed(-1, 0f);
        cappedVelocity(0f);
        Destroy(gameObject, 10f);
    }
    
}
