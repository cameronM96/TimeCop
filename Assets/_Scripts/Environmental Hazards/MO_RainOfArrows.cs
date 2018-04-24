using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by Myles Okorn
public class MO_RainOfArrows : MonoBehaviour {

    public float rainTimer = 500.0f;

    private bool isRaining = false;

	// Use this for initialization
	void Start ()
    {
		
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

        if (isRaining)
        {
            //start rain of arrows

            //reset the timer 
            rainTimer = 500.0f;
        }
	}
}
