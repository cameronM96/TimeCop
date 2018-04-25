using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by Myles Okorn
public class MO_RainOfArrows : MonoBehaviour {

    public float rainTimer = 500.0f;
    public GameObject arrow;
    public float numberOfArrows;
    public float numberOfRows;

    private bool isRaining = false;

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

        if (isRaining)
        {

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
            //reset the timer 
            rainTimer = 500.0f;
            isRaining = false;
        }
	}
}
