using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemiesWithAttackAnimation : Enemy
{

    protected float attackCountdown = 0f;
    protected bool canAttack = true;
    protected float distanceAttack = 0f;
    protected float movingTime = 0f;

    protected bool isSetFreeRoamDirection = false;
    protected void Attack()
    {
        if (canAttack)
        {
            state = State.Attack;
            var pPos = PlayerController.Instance.gameObject.transform.position;
            var pos = transform.position;
            int y = pPos.y >= pos.y ? 1 : -1;
            int x = pPos.x >= pos.x ? 1 : -1;
            if (Math.Abs(pPos.x - pos.x) > Math.Abs(pPos.y - pos.y)) y = 0;
            else x = 0;
            this.moveDir = new Vector2(x, y);
            animator.SetFloat("moveX", x);
            animator.SetFloat("moveY", y);
            animator.SetTrigger("Attack");
            agent.isStopped = true;
            canAttack = false;
        }

    }
    protected override void MoveTowardsPlayer()
    {
        if (agent != null && PlayerController.Instance.gameObject.transform != null)
        {
            hpBar.SetActive(true);
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
            isMovingToPlayer = true;
        }
    }
    protected virtual void OnAttackAnimationEnd()
    {
        state = State.Roaming;
        StartCoroutine(WaitAttackCountDown());
    }
    protected IEnumerator WaitAttackCountDown()
    {
        yield return new WaitForSeconds(attackCountdown);
        canAttack = true;
    }
    protected virtual void OnDieAnimationEnd() { }
    protected override void HandleOnFreeRoam()
    {
        if (isMovingToPlayer)
        {
            StopMoveToPlayer();
            return;
        }
        if (isStanding) moveTimer -= Time.deltaTime;

        if (moveTimer <= 0)
        {
            if (agent == null) return;
            hpBar.SetActive(true);
            if (!isSetFreeRoamDirection) { moveDir = GetRoamingDir(); isSetFreeRoamDirection = true; }
            agent.SetDestination(gameObject.transform.position + (Vector3)moveDir);
            agent.isStopped = false;

            animator.SetBool("isMove", true);
            animator.SetFloat("moveX", moveDir.x);
            animator.SetFloat("moveY", moveDir.y);
            isStanding = false;
            StartCoroutine(StopMove());

        }

    }
    protected override IEnumerator StopMove()
    {
        yield return new WaitForSeconds(movingTime);
        enemyRigidbody.velocity = Vector2.zero;
        isStanding = true;
        animator.SetBool("isMove", false);
        moveTimer = moveInterval;
        agent.isStopped = true;
        hpBar.SetActive(false);
        isSetFreeRoamDirection = false;

    }
    protected override void StopMoveToPlayer(bool hideHpBar = true)
    {
        base.StopMoveToPlayer();
        animator.SetBool("isMove", false);
    }

}
