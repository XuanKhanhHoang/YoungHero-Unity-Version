using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending_SpecialHerb : MonoBehaviour
{
    public GameObject combatTrigger;
    public FireDevilGeneral boss;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameController.instance.gameStoryManager.animationFade.FadeIn();
            GameController.instance.gameStoryManager.IncreaseGameActionIndex();
            GameController.instance.SetGameState(GAME_STATE.STORY_STATE);
            combatTrigger.SetActive(true);
            boss.gameObject.SetActive(true);
            PlayerController.Instance.SetDirection(DIRECTION.RIGHT);
            PlayerController.Instance.movePlayer(new Vector2(15.42f, 36.16f));
            gameObject.SetActive(false);

        }
    }
}
