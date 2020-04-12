using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementV2 : MonoBehaviour
{
    GameObject Player;
    public Animator animation;
    public Transform attackHitBox;
    public LayerMask enemyLayers;
    public SpriteRenderer spriteRenderer;
    public float moveSpeed = 6f;
    public float jumpSpeed = 10f;
    public float airDodgeSpeed = 10f;
    public float attackRange = 0.5f;
    public int attackDamage = 20;
    public float attackSpeed = 2f;
    private float nextAttack = 0f;
    public bool grounded = false;
    public bool airCheck = false;
    public bool airDodge = false;
    void Start()
    {
        Player = gameObject.transform.parent.gameObject;
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
        if(Input.GetAxis("Horizontal") > 0 && grounded == true)
        {
            spriteRenderer.flipX = true;
        }
        else if(Input.GetAxis("Horizontal") < 0 && grounded == true)
        {
            spriteRenderer.flipX = false;
        }
    }

    void Jump()
    {
        Vector2 jumpVector = new Vector2(0f, jumpSpeed);
        gameObject.GetComponent<Rigidbody2D>().AddForce(jumpVector, ForceMode2D.Impulse);
        //animation.SetTrigger("Jump");
    }

    void Attack()
    {
        animation.SetTrigger("Attack");
        Collider2D[] onHit = Physics2D.OverlapCircleAll(attackHitBox.position, attackRange, enemyLayers);
        foreach(Collider2D enemy in onHit)
        {
            Debug.Log("We hit" + enemy.name);
            enemy.GetComponent<Humanoid>().DamageTaken(attackDamage);
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

    //Check if player is touching the ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            grounded = true;
            airCheck = false;
            airDodge = false;
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

    private void OnDrawGizmosSelected()
    {
        if(attackHitBox == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackHitBox.position, attackRange);
    }

}
