using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class PatrolState : IEnemyState
    {
        private Enemy enemy;
        private float patrolTimer;
        private float patrolDuration = 10;

        public void Enter(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public void Execute()
        {
            Patrol();

            if (enemy.Target != null)
            {
                enemy.ChangeState(new AttackState());
            }
        }

        public void Exit()
        {

        }

        public void OnTriggerEnter(Collider2D other)
        {
            if (other.tag == "Edge")
            {
                // Make him turn around somehow...
                Debug.Log("Turning around");
                if (enemy.transform.localScale.x > 0)
                {
                    enemy.Move(-1, false);
                }
                else
                {
                    enemy.Move(1, false); 
                }
            }
        }

        private void Patrol()
        {
            if (enemy.transform.localScale.x > 0)
            {
                enemy.h = 1;
            } 
            else
            {
                enemy.h = -1;
            }
            patrolTimer += Time.deltaTime;

            if (patrolTimer >= patrolDuration)
            {
                enemy.ChangeState(new IdleState());
                patrolTimer = 0;
            }
        }
    }
}
