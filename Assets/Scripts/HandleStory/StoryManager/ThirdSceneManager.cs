using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThirdSceneManager : GameStoryManager
{
    private CanvasGroup cnvg;

    [SerializeField] GameObject respawnTrigger;
    public override string GetCurMission()
    {
        switch (curGameActionIndex)
        {
            case 1: return "Go to the village";
            case 2: return "Talk to this NPC ";
            case 3: return "Return to Old Home and buy something if it needed";
            case 4: return "Find the special herb on the North of village ";
            case 6: return "Kill Megicula!";
            default: return "";
        }
    }
    protected override void NeededCallOnAwake()
    {
        cnvg = DDOL_GameObj_Manager.instance.BgUICanvasGr;
    }

    public override void HandleUpdate()
    {

        if ((animationFade.fadeIn || animationFade.fadeOut)) return;
        DialogManager.instance.ShowDialog(DIALOGS_STORY[curGameDialogIndex]);
        if (Input.GetKeyDown(KeyCode.Return) && isHandleEnter)
        {
            if (curGameDialogIndex == 2)
            {
                GameController.instance.SetGameState(GAME_STATE.FREE_ROAM);
                IncreaseGameActionIndex();//1
                PlayerRespawnManager.instance.SetRespawnPoint();
            }
            else if (curGameDialogIndex == 5)
            {
                GameController.instance.SetGameState(GAME_STATE.FREE_ROAM);
                IncreaseGameActionIndex();//2
            }

            else if (curGameDialogIndex == 18)
            {
                GameController.instance.SetGameState(GAME_STATE.FREE_ROAM);
                IncreaseGameActionIndex();//3
                respawnTrigger.SetActive(true);
                PlayerController.Instance.AddItemToInventory(ItemManager.Instance.GetItem("Kizamitsu Sword"));
                PlayerController.Instance.ChangeCoin(200);
            }
            else if (curGameDialogIndex == 28)
            {
                GameController.instance.SetGameState(GAME_STATE.FREE_ROAM);
                IncreaseGameActionIndex();//5
            }
            else if (curGameDialogIndex == 31)
            {
                cnvg.alpha = 1;
            }
            else if (curGameDialogIndex == 36)
            {
                DialogManager.instance.HideDialog();
                SceneManager.LoadScene("CreditEndingScene");
            }
            curGameDialogIndex++;
        }

    }

    protected override void SetDialogStory()
    {
        DIALOGS_STORY.Add("[Player] How nostalgic!");
        DIALOGS_STORY.Add("[Player] Time to go back to the village");
        DIALOGS_STORY.Add("[Player] If I remember correctly, the direction to the village is south (down).");//2
        DIALOGS_STORY.Add("[Player] How nostalgic");
        DIALOGS_STORY.Add("[Player] I see someone over there, I think it's Uncle Tondo.");
        DIALOGS_STORY.Add("[Player] I think I'll go talk to him first.");//5
        DIALOGS_STORY.Add("[Player] Hello Uncle Tondo, do you recognize who I am?");
        DIALOGS_STORY.Add("[Tondo-NPC] I think you are Kekashi, right? Long time no see!");
        DIALOGS_STORY.Add("[Player] It's been almost 3 years since that event...");
        DIALOGS_STORY.Add("[Player] Now that I'm a swordsman, I've been assigned to return to the village to kill the skeletons.");
        DIALOGS_STORY.Add("[Tondo] It's a pleasure to have you here!");
        DIALOGS_STORY.Add("[Tondo] There is someone in our village suffering from Ogreto disease, there is a special herb in the forest that can cure it.");
        DIALOGS_STORY.Add("[Tondo] But we can't go out and get it because the skeletons are outside.");
        DIALOGS_STORY.Add("[Tondo] Can you help us get it?");
        DIALOGS_STORY.Add("[Player] Of course, I will help everyone.");
        DIALOGS_STORY.Add("[Tondo] I will give you a Kizamitsu sword and 200 coins as payment.");
        DIALOGS_STORY.Add("[Tondo] You can buy things at the southwest store (down left).");
        DIALOGS_STORY.Add("[Tondo] Be careful!");
        DIALOGS_STORY.Add("[Player] I think I should go back to my old house and buy some things if needed.");//18
        DIALOGS_STORY.Add("[Unknow Devil] What small human would want to take those herbs from me?");
        DIALOGS_STORY.Add("[Player] You devil, who are you?");
        DIALOGS_STORY.Add("[Unknow Devil] The first time a human dared to ask my name!");
        DIALOGS_STORY.Add("[Devil]Remember this name well because today is the day you die.");
        DIALOGS_STORY.Add("[Devil] Megicula - Fire Devil General is my name");
        DIALOGS_STORY.Add("[Player] Megicula - You killed my grandfather 3 years ago, right?");
        DIALOGS_STORY.Add("[Megicula] Oh many years ago I came here once but there was no one I would remember! Haha");
        DIALOGS_STORY.Add("[Player] Accept your death!");
        DIALOGS_STORY.Add("[Megicula] You have a big mouth, I will let you meet your grandfather soon.! Kakaka");
        DIALOGS_STORY.Add("[Player] You'll die !");//28
        DIALOGS_STORY.Add("[Megicula]No way! No ..... !");
        DIALOGS_STORY.Add("[Player] So I have successfully avenged you !");
        DIALOGS_STORY.Add("[Player]You see, I have become a strong person !");//31
        DIALOGS_STORY.Add("How will his story continue?");
        DIALOGS_STORY.Add("Who is Kekashi's true identity?");
        DIALOGS_STORY.Add("Who will he meet and what kind of person will he become on his journey?");
        DIALOGS_STORY.Add("Please continue to wait for the next update of the game.");
        DIALOGS_STORY.Add("Thank you for taking the time to play our game!");//36






















    }

    public override bool CanSaveGame()
    {
        return curGameActionIndex != 6;
    }
    protected override void ResetGameStoryObjects(int gameActionIndex)
    {

        if (gameStoryObjects == null || gameStoryObjects.Length == 0) return;
        if (gameActionIndex == 0)
        {
            GameController.instance.SetGameState(GAME_STATE.STORY_STATE);
            playerController.movePlayer(new Vector2(-32.59f, 26.82f));
            playerController.SetDirection(DIRECTION.RIGHT);
        }
        else if (gameActionIndex == 4)
        {
            gameStoryObjects[3].SetActive(false);
            gameStoryObjects[0].SetActive(true);
            gameStoryObjects[1].SetActive(false);
            gameStoryObjects[1].transform.position = new Vector2(18.9500008f, 36.6500015f);
            gameStoryObjects[2].SetActive(false);
        }
    }


}
