using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by Myles Okorn
public class MO_BuildingFalling : MonoBehaviour {

    public GameObject triggerVolume;
    public GameObject levelBuilding;

    public float fallingSpeed = 1.0f;

    private bool isFalling = false;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isFalling)
        {
            levelBuilding.transform.Translate(0, fallingSpeed, 0);
        }
        if (levelBuilding.transform.position.y >= -50)
        {
            levelBuilding.SetActive(false);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isFalling)
        {
            isFalling = true;
        }
    }
}
