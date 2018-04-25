using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Created by Myles Okorn
public class MO_RainOfArrows : MonoBehaviour {

    public float rainTimer = 10.0f;
    public float resetTime = 10f;
    public GameObject arrow;
    public float numberOfArrows;
    public float numberOfRows;
    public GameObject player;
    public Image warning;

    private bool isRaining = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //warning = GameObject.FindGameObjectWithTag("Warning").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (rainTimer > 0)
        {
            rainTimer -= Time.deltaTime;
        }
        else if (rainTimer < 0)
        {
            isRaining = true;
        }

        if (rainTimer < 5)
        {
            if (player.transform.position.x > transform.position.x &&
                (player.transform.position.x + numberOfArrows) < transform.position.x)
            {
                if (warning != null)
                    warning.enabled = true;
                //StartCoroutine(FlashWarning());
            }
        }
        else
        {
            if (warning != null)
                warning.enabled = false;
        }

        if (isRaining)
        {
            // Here down created by Cameron Mullins
            //start rain of arrows
            for (int i = 0; i < numberOfRows; i++)
            {
                Vector3 spawnPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                if (i % 2 != 1)
                {
                    spawnPoint.x += 0.5f;
                }
                spawnPoint.y += i;
                for (int o = 0; o < numberOfArrows; o++)
                {
                    spawnPoint.x += 1;
                    Instantiate(arrow, spawnPoint, transform.rotation);
                }
            }
            // End Cameron Mullins work
            //reset the timer 
            rainTimer = resetTime;
            isRaining = false;
        }
	}

    IEnumerator FlashWarning ()
    {
        yield return new WaitForSeconds(1);
        if (warning != null)
            warning.enabled = !warning.enabled;
    }
}
