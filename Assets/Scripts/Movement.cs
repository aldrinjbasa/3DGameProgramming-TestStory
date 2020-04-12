using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    GameObject Player;
    public Animator animation;
    public float moveSpeed = 6f;
    public float jumpSpeed = 10f;
    public float airDodgeSpeed = 10f;
    public bool grounded = false;
    public bool airCheck = false;
    // Start is called before the first frame update
    void Start()
    {
        Player = gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalMovement();
        JumpState();
        WaveDash();
    }

    void HorizontalMovement()
    {
        Vector3 characterMovement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += characterMovement * Time.deltaTime * moveSpeed;
        animation.SetFloat("horizontalMoveSpeed", Mathf.Abs(characterMovement.x));
    }

    void JumpSquatState()
    {
        if(Input.GetButton("Jump") && grounded == true)
        {
            animation.SetBool("jumpButtonPressed", true);
        }
    }
    void JumpState()
    {
        if (Input.GetButton("Jump") && grounded == true)
        {
            animation.SetBool("jumpButtonPressed", true);
            animation.SetFloat("verticalMoveSpeed", jumpSpeed);
            Vector2 verticalMovement = new Vector2(0f, jumpSpeed);
            gameObject.GetComponent<Rigidbody2D>().AddForce(verticalMovement, ForceMode2D.Impulse);
        }
        
    }
    void animSetGrounded() //Called using animator
    {
        animation.SetFloat("verticalMoveSpeed", 0);
        animation.SetBool("jumpButtonPressed", false);
    }

    void animLand() //Called using animator
    {
        animation.SetBool("animGrounded", true);
        animation.SetBool("airDodge", false);
    }

    void animDone()
    {
        animation.SetBool("animGrounded", false);
        animation.SetBool("airDodge", false);
        animation.SetBool("jumpButtonPressed", false);
    }

    void WaveDash()
    {
        //Downright Wavedash
        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.DownArrow) && airCheck == true)
        {
            animation.SetBool("airDodge", true);
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(airDodgeSpeed, -airDodgeSpeed), ForceMode2D.Impulse);
        }
        //Downleft Wavedash
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.DownArrow) && airCheck == true)
        {
            animation.SetBool("airDodge", true);
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-airDodgeSpeed, -airDodgeSpeed), ForceMode2D.Impulse);
        }
    }

    //Check if player is touching the ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            grounded = true;
            airCheck = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            grounded = false;
            airCheck = true;
        }
    }
}
