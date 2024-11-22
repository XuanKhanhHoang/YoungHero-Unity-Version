using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranningDummy : Enemy
{
    public SecondStoryObject gameStoryManager;
    public override void OnEnemyDie()
    {
        gameStoryManager.OnDummyDie();
        gameObject.SetActive(false);
    }

    protected override void SetEnemyInfo()
    {
        MAX_HP = 4;
        curHP = MAX_HP;
        DEF = 1;
        ATK = 0;
        EXP = 0;
    }

    protected override void Update()
    {
    }



}
