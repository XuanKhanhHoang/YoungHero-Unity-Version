using System.Collections;
using UnityEngine;

public class BlueSlimeController : Enemy
{
    public GameObject hpPotionPrefab, coinPrefab;

    private static bool isInStoryMode = true;
    public static void ResetSoryMode(bool mode = false)
    {
        isInStoryMode = mode;
    }
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
        MAX_HP = 6;
        DEF = 1;
        ATK = 2;
        EXP = 2;
        coin = 50;
        rampantDistance = 4;
        enemyName = "Blue Slime";

    }

    public override void OnEnemyDie()
    {
        StopMoveToPlayer();
        animator.SetTrigger("Die");

    }
    public void OnEnemyAnimationEnd()
    {
        TriggerOnEnemyDeath();
        Destroy(gameObject);
        int rnd = UnityEngine.Random.Range(0, 100);
        if (
          rnd < 10
             ) Instantiate(hpPotionPrefab, gameObject.transform.position, new Quaternion());
        else if (rnd < 15)
        {
            GameObject newCoin = Instantiate(coinPrefab, gameObject.transform.position, new Quaternion());
            Coin coinScript = newCoin.GetComponent<Coin>();
            if (coinScript != null)
            {
                coinScript.value = coin;
            }
        }
        if (isInStoryMode)
        {
            if (GameController.instance.gameStoryManager.GetCurGameActionIndex() == 6)
            {
                ((SecondStoryObject)GameController.instance.gameStoryManager).OnSlimeQuestComplete();
                isInStoryMode = false;
            }
            else
            {
                GameController.instance.gameStoryManager.IncreaseGameActionIndex();
            }
        }

    }




}