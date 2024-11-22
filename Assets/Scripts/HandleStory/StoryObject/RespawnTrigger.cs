using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameController.instance.SetGameState(GAME_STATE.DIALOG);
        GameController.instance.gameStoryManager.IncreaseGameActionIndex();
        PlayerRespawnManager.instance.SetRespawnPoint();
        DialogManager.instance.ShowDialog("Your respawn poin is setted");
        PlayerController.Instance.SetIsMoving(false);
        gameObject.SetActive(false);
    }

}
