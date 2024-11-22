using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDevilGeneral : EnemiesWithAttackAnimation
{
    protected override void SetEnemyInfo()
    {
        MAX_HP = 38;
        DEF = 6;
        ATK = 7;
        distanceAttack = 1.8f;
        attackCountdown = 0.8f;
    }
    protected override void StopMoveToPlayer(bool hideHpBar = true)
    {
        agent.isStopped = true;
        enemyRigidbody.velocity = Vector2.zero;
        isMovingToPlayer = false;
    }
    protected override void MoveTowardsPlayer()
    {
        if (agent != null && PlayerController.Instance.gameObject.transform != null)
        {
            if (agent.isStopped == true) agent.isStopped = false;
            Vector3 moveDr = agent.desiredVelocity.normalized;
            if (Mathf.Abs(moveDr.x) > Mathf.Abs(moveDr.y))
            {
                moveDr = new Vector3(Mathf.Sign(moveDr.x), 0, 0);
            }
            else
            {
                moveDr = new Vector3(0, Mathf.Sign(moveDr.y), 0);
            }
            this.moveDir = moveDr;
            animator.SetBool("isMove", true);
            animator.SetFloat("moveX", moveDr.x);
            animator.SetFloat("moveY", moveDr.y);
            agent.SetDestination(PlayerController.Instance.gameObject.transform.position);

        }
    }
    protected override void OnDieAnimationEnd()
    {
        GameController.instance.gameStoryManager.SetIsHanleEnter(true);
        GameController.instance.gameStoryManager.IncreaseGameDialogIndex();
        Destroy(gameObject);
    }
    protected override void OnAttackAnimationEnd()
    {
        state = State.Roaming;
        StartCoroutine(WaitAttackCountDown());
    }
    protected override void Update()
    {
        if (state == State.Dieing || GameController.instance.GetGameState() != GAME_STATE.FREE_ROAM)
        {
            StopMoveToPlayer(false);
            if (state != State.Dieing)
            {
                animator.speed = 0; ;
                animator.SetFloat("moveX", moveDir.x);
                animator.SetFloat("moveY", moveDir.y);
            }
            return;
        }
        animator.speed = 1;
        var distance = Vector2.Distance(gameObject.transform.position, PlayerController.Instance.gameObject.transform.position);
        if (distance <= distanceAttack && state != State.Attack)
        {
            Attack();
        }
        else if (state == State.Roaming || !canAttack)
        {

            MoveTowardsPlayer();
        }
    }
    public override void OnEnemyDie()
    {
        StopMoveToPlayer();
        GameController.instance.gameStoryManager.SetIsHanleEnter(false);
        GameController.instance.SetGameState(GAME_STATE.STORY_STATE);
        animator.SetTrigger("Die");
    }

}
