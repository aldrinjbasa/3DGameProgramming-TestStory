using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth = 100;
    public Animator animation;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageTaken(int damage)
    {
        currentHealth -= damage;
        animation.SetTrigger("onHit");
        FindObjectOfType<AudioManager>().Play("OnHit");

        if(currentHealth < 0)
        {
            Die();
        }
    }

    void Die()
    {
        animation.SetTrigger("onDeath");
        FindObjectOfType<AudioManager>().Play("Death");
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        this.enabled = false;
    }
}
