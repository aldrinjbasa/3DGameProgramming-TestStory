using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Humanoid_Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth = 100;
    public int positionInArray;
    public Transform sightRadius;
    public GameObject player;
    public float scanRange = 5f;
    public Animator animation;
    public Text scoreText;
    public int scoreToAdd;
    public float moveSpeed;
    

    void Start()
    {
        currentHealth = maxHealth;
        scoreText = GameObject.Find("Score").GetComponentInChildren<Text>();
    }
    // Update is called once per frame
    void Update()
    {
        player = GameObject.Find("Player");
        float distance = Vector3.Distance(player.transform.position, transform.position);
        Vector3 playerCoord = new Vector3(player.transform.position.x, player.transform.position.y, 0f);
        Vector3 mobX = new Vector3(transform.position.x, transform.position.y, 0f);
        scoreToAdd = GameObject.Find("Player").GetComponent<Humanoid_Player>().score;
        if (distance <= scanRange)
        {
            transform.position = Vector2.MoveTowards(mobX, playerCoord, moveSpeed * Time.deltaTime);
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
        scoreToAdd++;
        GameObject.Find("Player").GetComponent<Humanoid_Player>().score += 1;
        animation.SetTrigger("onDeath");
        FindObjectOfType<AudioManager>().Play("Death");
        scoreText.text = "Score: " + scoreToAdd;
        //GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine(waitTilDestroy(1));
    }

    IEnumerator waitTilDestroy(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("OutOfBounds"))
        {
            Vector3 startPosition = new Vector3(-0.0160647f, -2.685229f, -0.1269531f);
            gameObject.transform.position = startPosition;
        }
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
