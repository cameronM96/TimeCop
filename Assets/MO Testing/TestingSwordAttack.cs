using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingSwordAttack : MonoBehaviour {

    public GameObject weapon;

    private float restingROT = -40.0f;
    private Vector2 restingPOS;

    private float attackingROT = -105.0f;
    private Vector2 attackingPOS;

    private void Start()
    {
        restingPOS.x = 0.7099755f;
        restingPOS.y = 0.3534068f;

        attackingPOS.x = 0.95f;
        attackingPOS.y = -0.29f;
    }

    void update(bool attacking)
    {
        if (attacking) 
        {
            weapon.transform.eulerAngles.Set(0,0,attackingROT);
            weapon.transform.position.Set(attackingPOS.x,attackingPOS.y,0);
        }
        else if (!attacking)
        {
            weapon.transform.eulerAngles.Set(0,0,restingROT);
            weapon.transform.position.Set(restingPOS.x, restingPOS.y, 0);
        }
    }
}

