using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject playerInfo;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Update()
    {
        if (GameController.instance != null) playerInfo.SetActive(GameController.instance.GetGameState() == GAME_STATE.FREE_ROAM);
    }
}
