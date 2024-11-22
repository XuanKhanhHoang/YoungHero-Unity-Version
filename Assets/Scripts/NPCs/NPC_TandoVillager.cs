using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_TandoVillager : NPCController
{
    protected override void InitDialog()
    {
        dialogs.Add(new List<string>()
        {
            "Thank for your help!"
        });
    }
    public override void Interact()
    {
        int acIndex = GameController.instance.gameStoryManager.GetCurGameActionIndex();
        if (acIndex == 2)
        {
            GameController.instance.SetGameState(GAME_STATE.STORY_STATE);
        }
        else
        {
            ShowDialog();
        }
    }
}
