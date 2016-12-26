using UnityEngine;
using System.Collections;
using System;

public class MeleeState : IEnemyState
{
    private Enemy enemy;

    private float attackTimer;
    private float attackCooldown = 3;
    private bool canThrow = true;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        Attack();

        if (enemy.InThrowRange && !enemy.InMeleeRange)
        {
            enemy.ChangeState(new RangedState());
        }
        else if (enemy.Target == null)
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void Exit()
    {
        
    }

    public void OnTriggerEnter(Collider2D other)
    {
        
    }

    private void Attack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            canThrow = true;
            attackTimer = 0;
        }

        if (canThrow)
        {
            canThrow = false;
            enemy.MyAnimator.SetTrigger("attack");
        }
    }
}
