using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoribleNotify : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("2d");
        if (GameController.instance.gameStoryManager.GetCurGameActionIndex() == 1)
        {
            GameController.instance.SetGameState(GAME_STATE.STORY_STATE);
            PlayerController.Instance.SetIsMoving(false);
        }
        else if (GameController.instance.gameStoryManager.GetCurGameActionIndex() == 3)
        {
            DialogManager.instance.ShowDialog("Maybe I should go back to the old house.");
            GameController.instance.SetGameState(GAME_STATE.DIALOG);
            PlayerController.Instance.SetIsMoving(false);
            PlayerController.Instance.movePlayer(
                new Vector2(
                PlayerController.Instance.gameObject.transform.position.x,
                PlayerController.Instance.gameObject.transform.position.y - 0.2f)
            );
            PlayerController.Instance.SetDirection(DIRECTION.DOWN);

        }

    }
}
