using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public string enemyTag;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(enemyTag))
        {
            bool isCrit = PlayerController.Instance.GetIsCrit();
            int damage = PlayerController.Instance.GetRealAttackDamage(isCrit);
            if (isCrit) ScrollingMessageTextManager.instance.AddText("Critical Strike!");
            other.GetComponent<Enemy>()?.Hit(damage);

        }



    }
}