using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public DamageSpriteHandler[] damageNumbers;
    ArrayList damageArray = new ArrayList();
    float digitToDisplay = 0;

    void Awake()
    {

    }

    public void ShowDamage(int damage, Transform enemy)
    {
        int damageDigit;
        int currentDivision = damage;
        Vector3 displayVector = new Vector3(enemy.position.x + 0.5f, enemy.position.y + 0.5f);
        Vector3 resetVector = new Vector3(enemy.position.x + 0.5f, enemy.position.y + 0.5f);
        //Create Array of Damage Digits
        while (currentDivision >= 1)
        {
            damageDigit = currentDivision % 10;
            //Make an array based off of the modulo
            damageArray.Add(damageDigit);
            currentDivision = currentDivision / 10;
        }
        foreach(int digitInArray in damageArray)
        {
            //digitToDisplay = damageArray.Count - .1f;
            GameObject digitDisplay = new GameObject(digitInArray.ToString());
            digitDisplay.AddComponent<SpriteRenderer>();
            digitDisplay.AddComponent<Rigidbody2D>();
            digitDisplay.GetComponent<SpriteRenderer>().name = digitInArray.ToString();
            digitDisplay.GetComponent<SpriteRenderer>().sprite = damageNumbers[digitInArray].damageSprite;
            digitDisplay.GetComponent<SpriteRenderer>().sortingOrder = 2; //Plays on top of enemy
            digitDisplay.GetComponent<Rigidbody2D>().gravityScale = -0.1f;
            digitDisplay.transform.SetParent(enemy);
            digitDisplay.transform.position = displayVector;
            displayVector.x += -.385f;
            //for(int i = 255; i == 0; i--)
            StartCoroutine(waitTilDelete(3,digitDisplay));
            //digitToDisplay--;
        }
        //Display Damage to scene
        displayVector.x = resetVector.x;
        damageArray.Clear();
        //Attach sprite to damage

    }

    IEnumerator waitTilDelete(int seconds, GameObject digit)
    {

        yield return new WaitForSeconds(seconds);
        Destroy(digit);
    }

}
