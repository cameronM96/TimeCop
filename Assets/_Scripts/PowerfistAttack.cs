using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerfistAttack : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Perform the basic attack of the Powerfist
    public void PowerfistBasic(bool attacking)
    {
        if (attacking)
        {
            Debug.Log("Basic Powerfist attack");

        }
    }

    // Perform the special attack of the Powerfist
    public void PowerfistSpecial(bool attacking)
    {
        if (attacking)
        {
            Debug.Log("Special hPowerfist attack");

        }
    }
}
