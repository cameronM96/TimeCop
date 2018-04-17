using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace UnityStandardAssets._2D
{
    public class MeleeState : IEnemyState
    {
        private Enemy enemy;

        public void Enter(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public void Execute()
        {
            if (enemy.Target != null)
            {
                // different kind of move?
            }
            else
            {
                //maybe only do it if the AI reached the end of it's path
                enemy.ChangeState(new PatrolState());
            }
        }

        public void Exit()
        {

        }

        public void OnTriggerEnter(Collider2D other)
        {

        }
    }
}

