using UnityEngine;
using System.Collections;
using System;

public class PatrolState : IEnemyState
{
    private Enemy enemy;

    private float patrolTimer;
    private float patrolDuration;

    public void Enter(Enemy enemy)
    {
        patrolDuration = UnityEngine.Random.Range(3, 6);
        this.enemy = enemy;
    }

    public void Execute()
    {        
        Patrol();

        enemy.Move();

        if(enemy.Target != null && enemy.InThrowRange)
        {
            enemy.ChangeState(new RangedState());
        }
    }

    public void Exit()
    {
        
    }

    public void OnTriggerEnter(Collider2D other)
    {
        if (other.tag == "Orb")
        {
            enemy.Target = PlayerController.Instance.gameObject;
        }
    }

    private void Patrol()
    {
        enemy.MyAnimator.SetFloat("speed", 1);

        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolDuration)
        {
            enemy.ChangeState(new IdleState());
        }
    }
}
