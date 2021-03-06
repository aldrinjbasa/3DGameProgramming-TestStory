﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementV2 : MonoBehaviour
{
    GameObject Player;
    public Animator animation;
    public Transform attackHitBox;
    public LayerMask enemyLayers;
    public LayerMask platformLayers;
    public SpriteRenderer spriteRenderer;
    public GameObject ceilingCheck;
    public GameObject groundCheck;
    public float moveSpeed = 6f;
    public float jumpSpeed = 10f;
    public float airDodgeSpeed = 10f;
    public float attackRange = 0.5f;
    public int attackDamage = 20;
    public float attackSpeed = 2f;
    private float nextAttack = 0f;
    public float knockback = 2f;
    public bool grounded = false;
    public bool airCheck = false;
    public bool airDodge = false;
    void Start()
    {
        //Player = gameObject.transform.parent.gameObject;
    }

    void Update()
    {
        HorizontalMovement();

        if(Input.GetKeyDown(KeyCode.LeftAlt) && grounded == true)
        {
            Jump();
        }

        if(Time.time >= nextAttack) //implement attack speed
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Attack();
                nextAttack = Time.time + 1f / attackSpeed;
            }
        }

        WaveDash();
        Rotate();
        SuperJump();
    }
    void HorizontalMovement()
    {
        Vector3 characterMovement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += characterMovement * Time.deltaTime * moveSpeed;
        //animation.SetFloat("horizontalMoveSpeed", Mathf.Abs(characterMovement.x));
        if(Input.GetAxis("Horizontal") != 0)
        {
            animation.SetFloat("HorizontalMovement", Mathf.Abs(characterMovement.x));
        }
        else if(Input.GetAxis("Horizontal") == 0)
        {
            animation.SetFloat("HorizontalMovement", 0);
        }
    }

    void Rotate()
    {
        //Rotate Right
        if(Input.GetAxis("Horizontal") > 0 && grounded == true)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        //Rotate Left
        else if(Input.GetAxis("Horizontal") < 0 && grounded == true)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    void Jump()
    {
        Vector2 jumpVector = new Vector2(0f, jumpSpeed);
        gameObject.GetComponent<Rigidbody2D>().AddForce(jumpVector, ForceMode2D.Impulse);
        FindObjectOfType<AudioManager>().Play("Jump");
        //animation.SetTrigger("Jump");
    }

    void Attack()
    {
        animation.SetTrigger("Attack");
        FindObjectOfType<AudioManager>().Play("SwordSlash_01"); //Play Sword Slash
        Collider2D[] onHit = Physics2D.OverlapCircleAll(attackHitBox.position, attackRange, enemyLayers);
        Vector2 knockbackVector = new Vector2(knockback, 0f);
        foreach(Collider2D enemy in onHit)
        {
            int damageCalculation = attackDamage + Random.Range(1, 80);
            Debug.Log("We hit" + enemy.name);
            enemy.GetComponent<Humanoid_Enemy>().DamageTaken(damageCalculation);
            FindObjectOfType<DamageManager>().ShowDamage(damageCalculation, enemy.transform);
            damageCalculation = attackDamage;
            //Knockback
            // Facing Left (Attacking from Right side of enemy)
            if(transform.localScale.x == 1)
            {
                enemy.GetComponent<Rigidbody2D>().AddForce(-knockbackVector, ForceMode2D.Impulse);
            }
            // Facing Right (Attacking from Left side of enemy)
            else if(transform.localScale.x == -1)
            {
                enemy.GetComponent<Rigidbody2D>().AddForce(knockbackVector, ForceMode2D.Impulse);
            }
        }
    }

    void WaveDash()
    {
        //Down-Right Wavedash
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.DownArrow) && airCheck == true && airDodge == false)
        {
            airDodge = true;
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(airDodgeSpeed, -airDodgeSpeed), ForceMode2D.Impulse);
            Debug.Log("Wavedash Downright");
        }
        //Down-Left Wavedash
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.DownArrow) && airCheck == true && airDodge == false)
        {
            airDodge = true;
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-airDodgeSpeed, -airDodgeSpeed), ForceMode2D.Impulse);
        }
    }

    void SuperJump()
    {
        if(Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftAlt) && grounded == true)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 5f), ForceMode2D.Impulse);
            //gameObject.GetComponent<BoxCollider2D>().enabled = false;
            //StartCoroutine(waitEnableBoxCollider(1));
        }
        /*Collider2D[] onCeilingCollide = Physics2D.OverlapCircleAll(ceilingCheck.transform.position, 1f, platformLayers);
        foreach(Collider2D platforms in onCeilingCollide)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }*/
    }

    IEnumerator waitEnableBoxCollider(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }

    //Check if player is touching the ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.collider.tag == "Ground")
        {
            grounded = true;
            airCheck = false;
            airDodge = false;
            /*if(gameObject.GetComponent<BoxCollider2D>().enabled == false)
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
            }*/
        }
        else if(collision.collider.tag == "Enemy")
        {
            GetComponent<Humanoid_Player>().DamageTaken(attackDamage);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            grounded = false;
            airCheck = true;
            animation.SetTrigger("Jump");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            Physics2D.IgnoreCollision(GameObject.Find("Player").GetComponent<Collider2D>(), collision, true);
        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("OutOfBounds"))
        {
            Vector3 startPosition = new Vector3(-0.0160647f, -2.685229f, -0.1269531f);
            GameObject.Find("Player").transform.position = startPosition;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Physics2D.IgnoreCollision(GameObject.Find("Player").GetComponent<Collider2D>(), collision, false);
    }

    private void OnDrawGizmosSelected()
    {
        if(attackHitBox == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackHitBox.position, attackRange);
    }

}
