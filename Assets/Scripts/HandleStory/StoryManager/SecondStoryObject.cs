using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecondStoryObject : GameStoryManager
{
    [SerializeField] private Commander1 commander1;
    [SerializeField] private GameObject dummy;

    public override void HandleUpdate()
    {
        if ((animationFade.fadeIn || animationFade.fadeOut)) return;
        DialogManager.instance.ShowDialog(DIALOGS_STORY[curGameDialogIndex]);
        if (Input.GetKeyDown(KeyCode.Return) && isHandleEnter)
        {
            if (curGameDialogIndex == 9)
            {
                BgUICanvasGr.alpha = 1;
            }
            else if (curGameDialogIndex == 10)
            {
                BgUICanvasGr.alpha = 0;
                playerController.movePlayer(new Vector2(-14.22f, 2.48f));
                playerController.SetDirection(DIRECTION.LEFT);
                commander1.gameObject.transform.position = new Vector2(-12.24f, 3.53f);
            }
            else if (curGameDialogIndex == 13)
            {
                IncreaseGameActionIndex();
                GameController.instance.SetGameState(GAME_STATE.FREE_ROAM);
            }
            else if (curGameDialogIndex == 16)
            {
                animationFade.FadeOut();
                playerController.movePlayer(new Vector2(-1f, 24.66f));
                playerController.SetDirection(DIRECTION.UP);
                DialogManager.instance.HideDialog();
            }
            else if (curGameDialogIndex == 17)
            {
                curGameDialogIndex++;
                PlayerRespawnManager.instance.SetRespawnPoint();
                gameController.SetGameState(GAME_STATE.FREE_ROAM);
                return;

            }
            else if (curGameDialogIndex == 22)
            {
                gameController.SetGameState(GAME_STATE.FREE_ROAM);
            }
            else if (curGameDialogIndex == 24)
            {
                gameController.SetGameState(GAME_STATE.FREE_ROAM);
                IncreaseGameActionIndex();//now is 7
                PlayerRespawnManager.instance.SetRespawnPoint();
            }
            else if (curGameDialogIndex == 30)
            {
                animationFade.FadeOut();
                SceneManager.LoadScene("ThirdScene");
            }
            curGameDialogIndex++;
        }


    }
    protected override void CallOnStart()
    {
        GameController.instance.SetGameState(GAME_STATE.STORY_STATE);
        GameController.instance.isHandleInventory = true;

    }
    protected override void SetDialogStory()
    {
        DIALOGS_STORY.Add("[OldCommander] Stop! Who are you !");
        DIALOGS_STORY.Add("[Player] I'm  Hiramoto's grandson, My name is Kankeda. He told me to come here.");
        DIALOGS_STORY.Add("[OldCommander] Oh, so you're Hiramoto's grandson!");
        DIALOGS_STORY.Add("[OldCommander] I'm Nagumoto Hiroshi, your grandfather's friend.");
        DIALOGS_STORY.Add("[OldCommander]How is your grandfather? Is he still healthy?");
        DIALOGS_STORY.Add("[Player] He just died not long ago due to the demon army's attack on the village.");
        DIALOGS_STORY.Add("[OldCommander] Impossible, he's very strong! Even though he's a bit old, normal demons can't kill him.");
        DIALOGS_STORY.Add("[Player] He fought to protect the whole village, in a moment of carelessness he was attacked and then...");
        DIALOGS_STORY.Add("[OldCommander] Haizz, my old friend!");
        DIALOGS_STORY.Add("[OldCommander] Come inside with me, I will train you to become a strong person like his wish.");//9
        DIALOGS_STORY.Add("A few days later ...");//10
        DIALOGS_STORY.Add("[OldCommander] This is a wooden dummy for practice, you can press Space to attack.");
        DIALOGS_STORY.Add("[OldCommander] The red bar above the wooden dummy's head is the target's health bar. Try attacking the wooden man!");
        DIALOGS_STORY.Add("Mission: Destroy the wooden dummy");//13
        DIALOGS_STORY.Add("[OldCommander] Very good, now I have a mission for you.");
        DIALOGS_STORY.Add("[OldCommander] There are some blue slimes roaming around north of the base, go there and kill 5 of them.");
        DIALOGS_STORY.Add("[OldCommander] I give you 2 normal hp bottles, use them when needed.");//16
        DIALOGS_STORY.Add("Mission: Destroy 5 slimes");//17
        DIALOGS_STORY.Add("[OldCommander] Good Job !");//18
        DIALOGS_STORY.Add("[OldCommander] I have an mission for you, but now first you should relax in a little time !");
        DIALOGS_STORY.Add("[OldCommander] You can walk around to see the base !");
        DIALOGS_STORY.Add("[OldCommander] If you want to buy any thing, you can buy it in a shop in the North");
        DIALOGS_STORY.Add("[OldCommander] When you ready, come here and i will give you a mission.");//22
        DIALOGS_STORY.Add("[OldCommander] Hmm, First I want you to the Slime Base in the North and train until you get at least level 4");
        DIALOGS_STORY.Add("[OldCommander] And then come here. I'll give you an important mission");//24
        DIALOGS_STORY.Add("[OldCommander] You're so exellent!");
        DIALOGS_STORY.Add("[OldCommander] In the north of your village, skeletons are harassing peoplen");
        DIALOGS_STORY.Add("[OldCommander] It makes it impossible for people to leave the village and communicate.");
        DIALOGS_STORY.Add("[OldCommander] I want you to go back there to kill the skeletons and take the opportunity to visit your old village.");
        DIALOGS_STORY.Add("[OldCommander] Be careful!");
        DIALOGS_STORY.Add("[Player] Got it! I'm ready to do it now");//30

    }
    public void OnDummyDie()
    {
        animationFade.FadeOut();
        commander1.SetIsDown(true);
        commander1.Tele(new Vector2(-12.74f, 4.7f));
        playerController.movePlayer(new Vector2(-12.74f, 2.8f));
        GameController.instance.SetGameState(GAME_STATE.STORY_STATE);
        playerController.SetDirection(DIRECTION.UP);
        IncreaseGameActionIndex();
    }
    public void OnSlimeQuestComplete()
    {
        animationFade.FadeOut();
        commander1.Tele(new Vector2(8.49f, -2.63f));
        playerController.movePlayer(new Vector2(8.49f, -4.26f));
        playerController.SetDirection(DIRECTION.UP);
        GameController.instance.SetGameState(GAME_STATE.STORY_STATE);
        IncreaseGameActionIndex();
    }

    public override string GetCurMission()
    {

        if (curGameActionIndex == 1) return "Destroy the wooden dummy";
        if (curGameActionIndex < 7) return "Destroy 5 Blue slimes";
        if (curGameActionIndex == 7) return "Walk around and buy something if it needed. Then back to commander to get new mission";
        if (curGameActionIndex == 8) return "Get at least Lv4 and Back to Commander to get new mission";
        return "";



    }

    protected override void ResetGameStoryObjects(int gameActionIndex)
    {
        if (gameActionIndex == 0)
        {
            MainCameraController.instance.SetPlayerTarget();
            playerController.SetDirection(DIRECTION.RIGHT);
            playerController.movePlayer(new Vector2(-30.0f, -4.5f));
        }
        else if (gameActionIndex == 1)
        {
            commander1.gameObject.transform.position = new Vector2(-12.24f, 3.53f);
            dummy.SetActive(true);
        }
        else if (gameActionIndex > 1)
        {
            commander1.SetIsDown(true);
            commander1.Tele(new Vector2(8.53f, -2.41f));
            dummy.SetActive(false);
        }
        if (gameActionIndex < 7)
        {
            BlueSlimeController.ResetSoryMode(true);
        }
        else BlueSlimeController.ResetSoryMode(false);
    }

}
