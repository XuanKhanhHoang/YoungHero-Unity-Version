using System.Collections.Generic;
using UnityEngine;

public abstract class NPCController : MonoBehaviour, Interactable
{
    protected List<List<string>> dialogs;
    protected int curDialogIndex = 0;
    public int moveSpeed = 0;

    public virtual void Awake()
    {
        dialogs = new List<List<string>>();
        InitDialog();
    }
    protected abstract void InitDialog();
    public void SetVisibleNPC(bool isVisible) { gameObject.SetActive(isVisible); }

    public virtual void Interact()
    {
        ShowDialog();

    }
    protected void ShowDialog()
    {
        GameController.instance.SetGameState(GAME_STATE.DIALOG);
        List<string> curDialogs = dialogs[0];
        if (curDialogIndex > curDialogs.Count - 1) curDialogIndex = 0;
        string curDialog = curDialogs[curDialogIndex];
        DialogManager.instance.ShowDialog(curDialog);
        curDialogIndex++;
    }

}
