using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    // The following script was created by Cameron Mullins
    // This script is the Behaviour statemachine class, it makes sure each state has the following methods in them.
    public interface IEnemyState
    {
        void Execute();
        void Enter(Enemy enemy);
        void Exit();
        void OnTriggerEnter2D(Collider2D other);
    }
}
