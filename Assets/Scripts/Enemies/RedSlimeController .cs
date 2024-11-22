using System.Collections;
using UnityEngine;

public class RedSlimeController : Enemy
{
    public GameObject hpPotionPrefab, coinPrefab;
    private int countDie = 0, maxCountDie = 5;


    private void Start()
    {
        hpBar.SetActive(false);
    }
    protected override void Update()
    {

        if (state == State.Dieing || GameController.instance.GetGameState() != GAME_STATE.FREE_ROAM) { StopMoveToPlayer(); return; }
        if (Vector2.Distance(gameObject.transform.position, PlayerController.Instance.gameObject.transform.position) <= rampantDistance)
        {
            MoveTowardsPlayer();
        }
        else
        {
            HandleOnFreeRoam();

        }

    }

    protected override void SetEnemyInfo()
    {
        MAX_HP = 11;
        DEF = 3;
        ATK = 5;
        EXP = 4;
        coin = 50;
        rampantDistance = 4;
        enemyName = "Red Slime";

    }

    public override void OnEnemyDie()
    {
        StopMoveToPlayer();
        OnImortal();
        StartCoroutine(DieAnimation());

    }
    private IEnumerator DieAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        countDie++;
        if (countDie == maxCountDie)
        {
            OnEnemyAnimationEnd();
        }
        else
        {
            StartCoroutine(DieAnimation());
        }
    }
    public void OnEnemyAnimationEnd()
    {
        TriggerOnEnemyDeath();
        Destroy(gameObject);
        int rnd = UnityEngine.Random.Range(0, 100);
        if (
          rnd < 15
             ) Instantiate(hpPotionPrefab, gameObject.transform.position, new Quaternion());
        else if (rnd < 20)
        {
            GameObject newCoin = Instantiate(coinPrefab, gameObject.transform.position, new Quaternion());
            Coin coinScript = newCoin.GetComponent<Coin>();
            if (coinScript != null)
            {
                coinScript.value = coin;
            }
        }

    }




}