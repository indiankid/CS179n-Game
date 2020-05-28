using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerhealth : MonoBehaviour
{
    public GameManager gameManager;
    public int maxHealth = 100;
    public int currentHealth;
    private Rigidbody2D rb;
    public healthbar healthBar;
    public float fallDeath;
    public float checkRadius;
    public LayerMask selectGround;
    private bool onGround;
    public Transform groundCheck;
    private float fallRange;
    private float lastGroundPos;
    private Animator anim;
    public GameObject player;
    public GameObject gun;
    public PlayerController playcont;
    // Start is called before the first frame update
    void Start()
    {
        playcont = GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        rb = GetComponent<Rigidbody2D>();
        lastGroundPos = groundCheck.position.y - fallDeath;
        anim.SetBool("isDead", false);
    }
    void FixedUpdate()
    {

        onGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, selectGround);
        if (onGround == true)
        {
            float fallRange = groundCheck.position.y - fallDeath;
            lastGroundPos = fallRange;
        }
        else if (onGround == false)
        {
            fallRange = lastGroundPos;
        }
        print("ground pos: " + groundCheck.position.y + " fallDeath: " + fallDeath + " fallRange: " + fallRange + " last pos: " + lastGroundPos);
        
        
        if (transform.position.y < fallRange)
        {
            TakeDamage(10);
            //anim.SetBool("isDead", true);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if(currentHealth <= 0)
        {
            anim.SetBool("isDead", true);
            //Destroy(gameObject, 2f);
            //Destroy(player.transform.Find("Gun").gameObject);
            playcont.enabled = false;
            Destroy(gun);
            //Destroy(player);
            gameManager.GameOver();
        }
    }

    void HealDamage(int damage)
    {
        currentHealth += damage;
        healthBar.SetHealth(currentHealth);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.tag)
        {
            case "mallet":
                TakeDamage(5);
                break;
            case "healthpack":
                HealDamage(10);
                break;

        }
    }
}
