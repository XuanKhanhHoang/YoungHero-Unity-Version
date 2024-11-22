using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : EnemiesWithAttackAnimation
{
    public GameObject hpPotionPrefab, coinPrefab;

    protected override void SetEnemyInfo()
    {
        MAX_HP = 15;
        DEF = 4;
        ATK = 6;
        EXP = 8;
        coin = 75;
        moveInterval = 5;
        attackCountdown = 1.4f;
        coin = 75;
        rampantDistance = 4;
        distanceAttack = 1.5f;
        movingTime = 2;
        enemyName = "Skeleton";
    }


    protected override void OnDieAnimationEnd()
    {
        TriggerOnEnemyDeath();
        Destroy(gameObject);
        int rnd = UnityEngine.Random.Range(0, 100);
        if (
          rnd < 15
             ) Instantiate(hpPotionPrefab, gameObject.transform.position, new Quaternion());
        else if (rnd < 40)
        {
            GameObject newCoin = Instantiate(coinPrefab, gameObject.transform.position, new Quaternion());
            Coin coinScript = newCoin.GetComponent<Coin>();
            if (coinScript != null)
            {
                coinScript.value = coin;
            }
        }
    }
    protected override void Update()
    {
        if (state == State.Dieing || GameController.instance.GetGameState() != GAME_STATE.FREE_ROAM) { StopMoveToPlayer(); return; }
        var distance = Vector2.Distance(gameObject.transform.position, PlayerController.Instance.gameObject.transform.position);
        if (distance <= distanceAttack && state != State.Attack)
        {
            Attack();
        }
        else if (distance <= rampantDistance && state == State.Roaming)
        {

            MoveTowardsPlayer();
        }
        else
        {
            HandleOnFreeRoam();

        }
    }

}
