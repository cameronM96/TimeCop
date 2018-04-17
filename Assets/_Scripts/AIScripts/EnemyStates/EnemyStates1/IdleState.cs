﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class IdleState : IEnemyState
    {
        private Enemy enemy;

        private float idleTimer;

        private float idleDuration = 5f;

        public void Enter(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public void Execute()
        {
            Idle();
            if (enemy.Target != null)
            {
                enemy.ChangeState(new PatrolState());
            }
        }

        public void Exit()
        {

        }

        public void OnTriggerEnter(Collider2D other)
        {

        }

        private void Idle()
        {
            enemy.myAnimator.SetFloat("speed", 0);

            idleTimer += Time.deltaTime;

            if (idleTimer >= idleDuration)
            {
                enemy.ChangeState(new PatrolState());
                idleTimer = 0;
            }
        }
    }
}
