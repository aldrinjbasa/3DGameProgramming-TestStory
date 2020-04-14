using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid_Player : MonoBehaviour
{
    public float maxHealth = 100;
    private float currentHealth = 100;
    public float knockback = 5f;
    public Animator animation;
    public GameObject healthBar;
    public int score;

    void Start()
    {
        currentHealth = maxHealth;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DamageTaken(int damage)
    {
        currentHealth -= damage;
        //animation.SetTrigger("onHit");
        FindObjectOfType<AudioManager>().Play("OnHit");
        Vector3 healthUpdateVector = new Vector3(currentHealth / maxHealth, 1f, 1f);
        //Vector3 healthSubtractVector = new Vector3((currentHealth / maxHealth), 0f, 0f);
        healthBar.transform.localScale = healthUpdateVector;
        if (currentHealth < 0)
        {
            Die();
        }
        else if(currentHealth > 0)
        {
            Knockback();
        }
    }

    void Die()
    {
        animation.SetTrigger("onDeath");
        FindObjectOfType<AudioManager>().Play("Death");
        GetComponent<BoxCollider2D>().enabled = false;
        //GetComponent<CircleCollider2D>().enabled = false;
        this.enabled = false;
    }

    void Knockback()
    {
        Vector2 knockbackVectorRight = new Vector2(knockback - 0.5f, knockback);
        Vector2 knockbackVectorLeft = new Vector2(-knockback + 0.5f, knockback);
        //Hit is on the left side of player
        if (transform.localScale.x == 1) 
        {
            transform.GetComponent<Rigidbody2D>().AddForce(knockbackVectorRight, ForceMode2D.Impulse);
        }
        else if(transform.localScale.x == -1)
        {
            transform.GetComponent<Rigidbody2D>().AddForce(knockbackVectorLeft, ForceMode2D.Impulse);
        }
    }

}
