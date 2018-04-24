using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created By Myles Okorn
public class MO_Blackout : MonoBehaviour {

    public float blackoutCountdown = 500.0f;
    public float blackoutTimer = 30.0f;

    public GameObject[] spotlights;

    public GameObject sceneLight;

    private bool isBlackout = false;

    // Use this for initialization
    void Start ()
    {
        foreach(GameObject light in spotlights)
        {
            light.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        blackoutCountdown -= Time.deltaTime;

        if (blackoutCountdown < 0)
        {
            blackoutTimer -= Time.deltaTime;

            if (!isBlackout)
            {
                blackout();
                isBlackout = true;
            }

            if (blackoutTimer < 0)
            {
                blackoutEnd();
                blackoutTimer = 30.0f;
                blackoutCountdown = 500.0f;
            }
        }
	}

    private void blackout()
    {
        sceneLight.SetActive(false);
        foreach (GameObject light in spotlights)
        {
            light.SetActive(true);
        }
    }

    private void blackoutEnd()
    {
        sceneLight.SetActive(true);
        foreach (GameObject light in spotlights)
        {
            light.SetActive(true);
        }
    }
}
