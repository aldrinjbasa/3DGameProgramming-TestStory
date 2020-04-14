using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeUp : MonoBehaviour
{
    Vector3 startPosition = new Vector3(-0.0160647f, -2.685229f, -0.1269531f);
    private GameObject player;
    public GameObject gameOverCanvas;
    // Start is called before the first frame update
    void Awake()
    {
        GameObject.Find("Player").transform.position = startPosition;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(player.GetComponent<Humanoid_Player>().score == 14)
        {
            gameOverCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
