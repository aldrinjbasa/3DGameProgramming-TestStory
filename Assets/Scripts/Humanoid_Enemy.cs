using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Humanoid_Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth = 100;
    public Transform sightRadius;
    public GameObject player;
    public float scanRange = 5f;
    public Animator animation;
    public GameObject scoreText;
    

    void Start()
    {
        currentHealth = maxHealth;
    }
    ⌈
    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        Vector3 playerCoord = new Vector3(player.transform.position.x, player.transform.position.y, 0f);
        Vector3 mobX = new Vector3(transform.position.x, transform.position.y, 0f);
        if(distance <= scanRange)
        {
            transform.position = Vector2.MoveTowards(mobX, playerCoord, 1f * Time.deltaTime);
        }

    }

    public void DamageTaken(int damage)
    {
        currentHealth -= damage;
        animation.SetTrigger("onHit");
        FindObjectOfType<AudioManager>().Play("OnHit");
        Debug.Log(damage);

        if(currentHealth < 0)
        {
            Die();
        }
    }

    void Die()
    {
        animation.SetTrigger("onDeath");
        FindObjectOfType<AudioManager>().Play("Death");
        //GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        this.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (sightRadius == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(sightRadius.position, scanRange);
    }


}
