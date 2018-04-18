using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace UnityStandardAssets._2D
{
    public class AttackState : IEnemyState
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
                if (Vector3.Distance(enemy.Target.transform.position, enemy.transform.position) < 1)
                {
                    // Do attack
                    enemy.h = 0;
                    enemy.attack = true;
                }
                else
                {
                    float xDir = enemy.Target.transform.position.x - enemy.transform.position.x;

                    if (xDir < 0)
                    {
                        enemy.h = -1;
                    }
                    else if (xDir > 0)
                    {
                        enemy.h = 1;
                    }
                    else
                    {
                        enemy.h = 0;
                    }
                }
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

