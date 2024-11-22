using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander1 : NPCController
{
    private Animator animator;
    private GameController gameController;
    public void Start()
    {
        animator = GetComponent<Animator>();
        gameController = GameController.instance;
    }
    public override void Interact()
    {
        if (gameController.gameStoryManager.GetCurGameActionIndex() < 7 || (gameController.gameStoryManager.GetCurGameActionIndex() == 8 && PlayerController.Instance.level < 4))
        {
            GameController.instance.SetGameState(GAME_STATE.DIALOG);
            DialogManager.instance.ShowDialog("Do your mission!");
        }
        else if (gameController.gameStoryManager.GetCurGameActionIndex() == 6)
        {
            GameController.instance.SetGameState(GAME_STATE.STORY_STATE);

        }
        else
        {
            GameController.instance.SetGameState(GAME_STATE.STORY_STATE);
        }
    }
    protected override void InitDialog() { }
    public void SetIsDown(bool isDown)
    {
        animator.SetBool("isDown", isDown);
    }
    public void Tele(Vector2 vct)
    {
        transform.position = vct;
    }
}
