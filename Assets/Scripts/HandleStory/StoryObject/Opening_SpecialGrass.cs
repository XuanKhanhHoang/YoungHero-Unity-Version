using UnityEngine;

public class Opening_SpecialGrass : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.SetActive(false);
        GameController.instance.SetGameState(GAME_STATE.STORY_STATE);
        PlayerController.Instance.SetIsMoving(false);
    }
}
