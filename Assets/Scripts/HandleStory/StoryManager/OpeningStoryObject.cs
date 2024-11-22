using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningStoryObject : GameStoryManager
{
    [SerializeField] GameObject infantPlayer, childPlayer, oldManDie;

    public override bool CheckPlayerCanMove(Vector2 targetPos)
    {
        return (((targetPos.y < 28f && targetPos.y > 19.0f) && (targetPos.x < 7.0f && targetPos.x > -17.0f))
               && GetcurGameDialogIndex() < 20);
    }

    public override void HandleUpdate()
    {
        if (curGameActionIndex == 1)
        {
            NPC_OldMan.instance.HandleMoveStory();
        }

        if (curGameActionIndex != 1) DialogManager.instance.ShowDialog(DIALOGS_STORY[curGameDialogIndex]);
        if (Input.GetKeyDown(KeyCode.Return) && isHandleEnter && !(animationFade.fadeIn || animationFade.fadeOut))
        {

            if (curGameDialogIndex == 0)
            {
                DialogManager.instance.HideDialog();
                curGameActionIndex++;
                isHandleEnter = false;
            }
            else if (curGameDialogIndex == 1)
            {
                NPC_OldMan.instance.animator.SetBool(NPC_OldMan.ID_IDLE_KEY, true);
            }
            else if (curGameDialogIndex == 5)
            {
                animationFade.FadeOut();
                NPC_OldMan.instance.transform.position = new Vector2(15.45f, -8.24f);
                NPC_OldMan.instance.animator.SetBool(NPC_OldMan.ID_IDLE_KEY, false);

                infantPlayer.SetActive(false);
                childPlayer.SetActive(true);
            }
            else if (curGameDialogIndex == 6)
            {
                animationFade.FadeOut();
                childPlayer.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
                childPlayer.transform.position = new Vector2(16.32f, -10.47f);


            }
            else if (curGameDialogIndex == 7)
            {
                childPlayer.SetActive(false);

            }
            else if (curGameDialogIndex == 8)
            {

                playerController.setActive(true);
                MainCameraController.instance.SetPlayerTarget();
                playerController.transform.position = new Vector2(15.45f, -9.34f);
                playerController.SetDirection(DIRECTION.UP);
            }
            else if (curGameDialogIndex == 14)
            {
                animationFade.FadeOut();
                playerController.movePlayer(new Vector2(-1.1f, 21.4f));
            }
            else if (curGameDialogIndex == 18)
            {
                gameController.SetGameState(GAME_STATE.FREE_ROAM);
            }
            else if (curGameDialogIndex == 19)
            {
                animationFade.FadeOut();
                playerController.movePlayer(new Vector2(-0.72f, 7.25f));
                playerController.SetDirection(DIRECTION.DOWN);
                NPC_OldMan.instance.gameObject.SetActive(false);

            }
            else if (curGameDialogIndex == 20)
            {
                animationFade.FadeOut();
                oldManDie.SetActive(true);
                playerController.movePlayer(new Vector2(15.45f, -9.34f));
                playerController.SetDirection(DIRECTION.UP);
            }
            else if (curGameDialogIndex == 28)
            {
                animationFade.FadeOut();
                DialogManager.instance.HideDialog();
                SceneManager.LoadScene("SecondScene");

            }
            curGameDialogIndex++;
        }


    }
    protected override void SetDialogStory()
    {
        DIALOGS_STORY.Add("In one day 20 years ago");
        DIALOGS_STORY.Add("SomeWhere: Wailing sound!");
        DIALOGS_STORY.Add("OldMan: Oh , Just a moment , Seeming like i'm hearing a crying!");
        DIALOGS_STORY.Add("OldMan: That a baby! Who did abandon that baby");
        DIALOGS_STORY.Add("OldMan: Don't cry , Let go home with me");
        DIALOGS_STORY.Add("From that time , the baby is adopted by OldMan and named is Kekashi");//5
        DIALOGS_STORY.Add("The time was go on in peaceful");
        DIALOGS_STORY.Add("The time was go on in peaceful");
        DIALOGS_STORY.Add("Until one day in 4 years ago");//8
        DIALOGS_STORY.Add("OldMan[Kekashi's GrandFather]: Hey Kekashi, Can you go to the mountain and take some herb for me ?");
        DIALOGS_STORY.Add("Kekashi[Player]: Of course, What is the herb which you want me to take ?");
        DIALOGS_STORY.Add("OldMan[Kekashi's GrandFather]: Do you remember Yatsuki Grass ?");
        DIALOGS_STORY.Add("Kekashi[Player]: Yes, I remember it. I'll take it for you");
        DIALOGS_STORY.Add("OldMan[Kekashi's GrandFather]: Be careful and come back before dark !");
        DIALOGS_STORY.Add("Kekashi[Player]: Got it. I'll do it quickly!");//14
        DIALOGS_STORY.Add("[Tutorial] Your Mission is find special grass");
        DIALOGS_STORY.Add("[Tutorial] Press: W to move up,S to move down");
        DIALOGS_STORY.Add("[Tutorial] Press: A to move Left,D to move right");
        DIALOGS_STORY.Add("[Tutorial] Control the player to touch the grass to get it.");//18
        DIALOGS_STORY.Add("Kekashi[Player]: Yeah, I found it. I need go to home now.");
        DIALOGS_STORY.Add("Kekashi[Player]: Why it so silent, I need runing quickly");//20
        DIALOGS_STORY.Add("Kekashi[Player]: Grandfather! What's happened!");
        DIALOGS_STORY.Add("OldMan[Kekashi's GrandFather]: There was a demon army attacking the village, I helped everyone evacuate.");
        DIALOGS_STORY.Add("OldMan[Kekashi's GrandFather]: In a moment of carelessness we were ambushed by them.");
        DIALOGS_STORY.Add("OldMan[Kekashi's GrandFather]: I have a friend at Hirashi barracks, go there and meet my friend there.");
        DIALOGS_STORY.Add("OldMan[Kekashi's GrandFather]: Maybe I can't grow up with you anymore, live well and become a strong person!");
        DIALOGS_STORY.Add("OldMan[Kekashi's GrandFather]: My dear nephew ...");
        DIALOGS_STORY.Add("Kekashi[Player]: Grandfather! Noooo!");
        DIALOGS_STORY.Add("Kekashi[Player]: I swear I will avenge you.!");//28
    }
    protected override void NeededCallOnAwake()
    {
        MainCameraController.instance.SetNPCTarget(NPC_OldMan.instance.transform);
    }
    protected override void CallOnStart()
    {
        GameController.instance.SetGameState(GAME_STATE.STORY_STATE);
    }
    public override string GetCurMission()
    {
        return "";
    }
    protected override void ResetGameStoryObjects(int gameActionIndex)
    {
        if (gameActionIndex == 0)
        {
            playerController.SetStartGamePlayerInfo();
        }
        else
        {
            NPC_OldMan.instance.gameObject.SetActive(false);
            infantPlayer.SetActive(false);
            childPlayer.SetActive(false);
            oldManDie.SetActive(false);
        }
    }

}
