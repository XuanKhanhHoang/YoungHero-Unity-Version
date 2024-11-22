using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public abstract class GameStoryManager : MonoBehaviour
{
    [HideInInspector] public AnimationFade animationFade;

    protected List<string> DIALOGS_STORY;

    protected GameController gameController;
    protected PlayerController playerController;
    protected DDOL_GameObj_Manager dDOL_gameObjManager;
    protected CanvasGroup BgUICanvasGr;

    [SerializeField] protected GameObject[] gameStoryObjects;


    protected bool isHandleEnter = true;
    protected int curGameDialogIndex = 0;
    protected int curGameActionIndex = 0;


    public abstract string GetCurMission();
    protected abstract void SetDialogStory();

    public virtual bool CanSaveGame() => true;
    protected virtual void NeededCallOnAwake() { }
    public virtual void HandleUpdate() { }
    public virtual bool CheckPlayerCanMove(Vector2 targetPos)
    {
        Vector2 minPos = new Vector2(-33.43f, -14.89f), maxPos = new Vector2(24.31f, 44.3f);

        return !(targetPos.x < minPos.x || targetPos.x > maxPos.x || targetPos.y < minPos.y || targetPos.y > maxPos.y);

    }
    protected virtual void CallOnStart() { }
    protected virtual void ResetGameStoryObjects(int gameActionIndex) { }

    protected void Awake()
    {
        DIALOGS_STORY = new List<string>();
        SetDialogStory();
        NeededCallOnAwake();
    }
    protected void Start()
    {
        SetGameController();
        BgUICanvasGr = dDOL_gameObjManager.BgUICanvasGr;
        playerController.SetIsMoving(false);
        ResetGameStoryObjects(0);
        CallOnStart();
    }

    public void SetGameActionIndexAndDialogIndex(int actionIndex, int dialogIndex)
    {
        curGameActionIndex = actionIndex;
        curGameDialogIndex = dialogIndex;
        ResetGameStoryObjects(actionIndex);
    }
    public void IncreaseGameActionIndex() { curGameActionIndex++; }
    public void IncreaseGameDialogIndex() { curGameDialogIndex++; }
    public void IncreaseActionAndDialogIndex() { curGameActionIndex++; curGameDialogIndex++; }
    public int GetcurGameDialogIndex() { return curGameDialogIndex; }
    public int GetCurGameActionIndex() { return curGameActionIndex; }

    public void SetIsHanleEnter(bool isHandle) { this.isHandleEnter = isHandle; }
    public bool GetIsHanleEnter() { return this.isHandleEnter; }
    public void SetGameController()
    {
        gameController = GameController.instance;
        playerController = PlayerController.Instance;
        animationFade = AnimationFade.instance;
        dDOL_gameObjManager = DDOL_GameObj_Manager.instance;
    }


}
