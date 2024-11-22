using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    protected enum State
    {
        Roaming, Attack, Dieing
    }

    protected State state;
    protected int DEF = 0, ATK = 1, MAX_HP = 0, curHP = 0, CritChance = 0;
    protected bool isImortal = false;
    protected float imortalTime = 0.5f;
    protected Vector2 moveDir;
    protected float moveSpeed = 1f;
    protected int EXP = 0;
    protected float moveInterval = 3.0f, roamingTimeInterval = 2f;
    protected float moveTimer;
    protected int coin = 0;
    protected bool isMovingToPlayer = false, isStanding = true;
    protected bool isMoving = false;
    protected float rampantDistance = 4f;
    protected string enemyName = "";

    protected NavMeshAgent agent;
    protected Rigidbody2D enemyRigidbody;
    protected SpriteRenderer rd;
    protected Animator animator;
    [SerializeField]
    protected Image hpImage;
    [SerializeField]
    protected GameObject hpBar;
    public delegate void EnemyDeath();
    public event EnemyDeath OnEnemyDeath;




    protected abstract void Update();
    protected abstract void SetEnemyInfo();
    public virtual void OnEnemyDie()
    {
        StopMoveToPlayer();
        animator.SetTrigger("Die");
    }
    protected virtual void OnImortal()
    {
        Color color = rd.color;
        color.a = 0.5f;
        rd.color = color;
    }
    protected virtual void MoveTowardsPlayer()
    {
        isMovingToPlayer = true;
        if (agent != null && PlayerController.Instance.gameObject.transform != null)
        {
            hpBar.gameObject.SetActive(true);
            if (agent.isStopped == true) agent.isStopped = false;
            agent.SetDestination(PlayerController.Instance.gameObject.transform.position);
        }
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameController.instance.GetGameState() == GAME_STATE.FREE_ROAM && collision.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.Hit(GetRealAttackDamage());

        }
    }

    protected virtual void HandleOnFreeRoam()
    {
        if (isMovingToPlayer)
        {
            StopMoveToPlayer();
            return;
        }
        if (isStanding) moveTimer -= Time.deltaTime;
        if (moveTimer <= 0)
        {
            if (enemyRigidbody.velocity == Vector2.zero)
            {
                moveDir = GetRoamingDir();
            }
            enemyRigidbody.velocity = moveDir * (moveSpeed + 2);
            isStanding = false;
            StartCoroutine(StopMove());
            return;
        }
    }
    protected virtual IEnumerator StopMove()
    {
        yield return new WaitForSeconds(roamingTimeInterval);
        enemyRigidbody.velocity = Vector2.zero;
        isStanding = true;
        moveTimer = moveInterval;
        hpBar.SetActive(false);

    }
    protected virtual void StopMoveToPlayer(bool hideHpBar = true)
    {
        if (hideHpBar) hpBar.SetActive(false);
        agent.isStopped = true;
        enemyRigidbody.velocity = Vector2.zero;
        isMovingToPlayer = false;
    }

    public void TriggerOnEnemyDeath()
    {
        OnEnemyDeath?.Invoke();
        ScrollingMessageTextManager.instance.AddText(this.enemyName + " be killed !");

    }

    public int GetCurHP()
    {
        return curHP;
    }
    protected void Awake()
    {
        state = State.Roaming;
        enemyRigidbody = GetComponent<Rigidbody2D>();
        rd = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        SetEnemyInfo();
        curHP = MAX_HP;
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
        moveTimer = moveInterval;
        if (enemyRigidbody != null) enemyRigidbody.velocity = Vector2.zero;

    }
    protected bool IsOutOfView(Vector3 position)
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(position);
        return screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1;
    }
    public void Hit(int damage)
    {
        if (!isImortal && state != State.Dieing)
        {

            curHP -= damage - DEF > 0 ? (damage - DEF) : 0;
            ScrollingMessageTextManager.instance.AddText("Hit " + (damage - DEF) + " damage !");

            if (hpImage != null)
            {
                RectTransform rt = hpImage.GetComponent<RectTransform>();
                if (rt != null)
                {
                    int l = (int)Math.Round(curHP * 100.0f / MAX_HP) - 100;
                    rt.offsetMax = new Vector2(l < -100 ? -100 : l, rt.offsetMax.y);
                }
            }
            if (curHP <= 0)
            {
                state = State.Dieing;
                OnEnemyDie();
                PlayerController.Instance.CollectEXP(EXP);
                return;
            }
            isImortal = true;
            OnImortal();
            StartCoroutine(DelayHit());
        }

    }
    protected IEnumerator DelayHit()
    {
        yield return new WaitForSeconds(imortalTime);
        isImortal = false;
        Color color = rd.color;
        color.a = 1f;
        rd.color = color;
    }
    protected Vector2 GetRoamingDir()
    {
        int rnd = UnityEngine.Random.Range(0, 3);
        switch (rnd)
        {
            case 0: return new Vector2(1, 0);
            case 1: return new Vector2(0, 1);
            case 2: return new Vector2(-1, 0);
            case 3: return new Vector2(0, -1);
            default: return new Vector2(0, 1);

        }

    }
    public int GetRealAttackDamage()
    {
        int a = UnityEngine.Random.Range(0, 100);
        int cr = CritChance;
        int damage = ATK;
        return a < cr ?
       (int)Math.Round(damage * 1.75f) : damage;

    }
    protected void Move()
    {
        enemyRigidbody.velocity = moveDir * moveSpeed;
    }



}
