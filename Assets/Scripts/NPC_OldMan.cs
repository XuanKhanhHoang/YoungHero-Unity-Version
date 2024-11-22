using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_OldMan : NPCController
{
    public const string IS_MOVE_NAME = "isMoving";
    public const string ID_IDLE_KEY = "isIdle";

    public bool isMoving = false;
    public Animator animator;
    public static NPC_OldMan instance { get; private set; }

    protected override void InitDialog() { }

    private new void Awake()
    {
        instance = this;
        dialogs = new List<List<string>>();
        InitDialog();
        animator = GetComponent<Animator>();

    }
    public void HandleMoveStory()
    {
        if (GameController.instance.gameStoryManager.GetCurGameActionIndex() == 1)
        {
            animator.SetBool(IS_MOVE_NAME, true);
            StartCoroutine(move());
        }

    }
    public IEnumerator move()
    {
        Vector2 tmp = new Vector2(14.47f, transform.position.y);

        if (transform.position.x < tmp.x)
        {
            transform.position = new Vector2(transform.position.x + 1f * Time.deltaTime, tmp.y);
            yield return null;
        }
        else
        {
            transform.position = tmp;
            GameController.instance.gameStoryManager.IncreaseGameActionIndex();
            GameController.instance.gameStoryManager.SetIsHanleEnter(true);
            animator.SetBool(IS_MOVE_NAME, false);

        }
    }



}
