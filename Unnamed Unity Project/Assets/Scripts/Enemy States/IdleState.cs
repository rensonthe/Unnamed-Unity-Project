using UnityEngine;
using System.Collections;
using System;

public class IdleState : IEnemyState
{
    private Enemy enemy;

    private float idleTimer;
    private float idleDuration;

    public void Enter(Enemy enemy)
    {
        idleDuration = UnityEngine.Random.Range(3,6);
        this.enemy = enemy;
    }

    public void Execute()
    {
        Idle();

        if(enemy.Target != null)
        {
            enemy.ChangeState(new PatrolState());
        }
    }

    public void Exit()
    {
        
    }

    public void OnTriggerEnter(Collider2D other)
    {
        if(other.tag == "Orb")
        {
            enemy.Target = PlayerController.Instance.gameObject;
        }
    }

    private void Idle()
    {
        enemy.MyAnimator.SetFloat("speed", 0);

        idleTimer += Time.deltaTime;

        if(idleTimer >= idleDuration)
        {
            enemy.ChangeState(new PatrolState());
        }
    }
}
