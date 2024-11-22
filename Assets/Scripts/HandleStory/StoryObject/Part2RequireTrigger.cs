using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part2RequireTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameController.instance.gameStoryManager.GetCurGameActionIndex() < 2)
            {
                PlayerController.Instance.SetIsMoving(false);
                PlayerController.Instance.movePlayer(
                    new Vector2(
                    PlayerController.Instance.gameObject.transform.position.x,
                    PlayerController.Instance.gameObject.transform.position.y - 0.2f)
                );
                PlayerController.Instance.SetDirection(DIRECTION.DOWN);
                GameController.instance.SetGameState(GAME_STATE.DIALOG);
                DialogManager.instance.ShowDialog("May be not that way!");
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
