using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOL_GameObj_Manager : MonoBehaviour
{
    public static DDOL_GameObj_Manager instance;

    public CanvasGroup BgUICanvasGr;
    public Player_d_invt_State_Manager PlayerDetailAndInvetoryManager;
    public DIedUIManager DiedUIManager;
    public GameSettingUIManager GameSettingUIManager;


    private void Awake()
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
}
