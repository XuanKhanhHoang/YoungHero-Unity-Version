using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDetailController : MonoBehaviour
{
    public static PlayerDetailController Instance { get; private set; }
    public TextMeshProUGUI lv, hp, atk, def, crit, mission, coin;
    public RectTransform exp;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void HandleUpdate()
    {
        lv.text = "Level: " + PlayerController.Instance.level;
        hp.text = $"HP: {PlayerController.Instance.curHP}/{PlayerController.Instance.GetMaxHP()}";
        atk.text = "ATK: " + PlayerController.Instance.GetATK();
        def.text = "DEF: " + PlayerController.Instance.GetDEF();
        crit.text = "CRIT: " + PlayerController.Instance.GetCrit() + "%";
        exp.offsetMax = new Vector2((int)Math.Round(PlayerController.Instance.GetCurEXP() * 100.0f / PlayerController.Instance.nextLevelEXP) - 100, exp.offsetMax.y);
        mission.text = GameController.instance.gameStoryManager.GetCurMission();
        coin.text = "COIN: " + PlayerController.Instance.GetCoin();
    }

}
