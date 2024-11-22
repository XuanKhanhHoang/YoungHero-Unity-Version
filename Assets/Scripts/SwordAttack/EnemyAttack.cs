using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Enemy enm;
    private void Awake()
    {

        enm = GetComponentInParent<Enemy>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>()?.Hit(enm.GetRealAttackDamage());
        }



    }
}