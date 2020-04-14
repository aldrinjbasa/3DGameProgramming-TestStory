using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public GameObject portal;
    GameObject player;
    bool teleportable;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(teleportable == true && Input.GetKeyDown(KeyCode.UpArrow))
        {
            Object.DontDestroyOnLoad(player);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            teleportable = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        teleportable = false;
    }
}
