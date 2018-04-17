using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaAttack : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Perform the basic attack of the Katana
    public void KatanaBasic(bool attacking)
    {
        if (attacking)
        {
            Debug.Log("Basic Katana attack");

        }
    }

    // Perform the special attack of the Katana
   public void KatanaSpecial(bool attacking)
    {
        if (attacking)
        {
            Debug.Log("Special Katana attack");

        }
    }
}
