using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 50;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ScrollingMessageTextManager.instance.AddText("Get " + value + "coin !");
            PlayerController.Instance.ChangeCoin(value);
            Destroy(gameObject);
            SoundManager.instance.PlaySFX(0);
        }
    }
}