using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatModeBossTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameController.instance.SetGameState(GAME_STATE.DIALOG);
            DialogManager.instance.ShowDialog("I have to kill Megicula!");
            PlayerController.Instance.movePlayer(new Vector2(PlayerController.Instance.gameObject.transform.position.x + 0.5f, PlayerController.Instance.gameObject.transform.position.y));
        }
    }
}