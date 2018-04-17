using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdAttack : MonoBehaviour {

    public GameObject Halberd;

	// Use this for initialization
	void Start () {
		
	}
	
	// Perform the basic attack of the halberd
	public void HalberdBasic (bool attacking)
    {
        if (attacking)
        {
            Debug.Log("Basic Halberd attack");

        }
	}

    // Perform the special attack of the halberd
    public void HalberdSpecial (bool attacking)
    {
        if (attacking)
        {
            Debug.Log("Special halberd attack");

        }
    }

}
