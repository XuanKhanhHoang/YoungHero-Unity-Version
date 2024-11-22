using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSettingUIManager : MonoBehaviour
{
    [SerializeField] private RectTransform bgmBar, sfxBar;
    [SerializeField] private GameObject bgm, sfx, save, mainMenu, exit;

    private int selected = 0;
    public void HandleUpdate()
    {
        gameObject.SetActive(true);
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameController.instance.SetGameState(GAME_STATE.FREE_ROAM);
            gameObject.SetActive(false);
            return;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && selected > 0)
        {
            selected--;
            return;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && selected < 4)
        {
            selected++;
            return;

        }
        if (selected == 0) handleBar();
        else if (selected == 1) handleBar(false);

        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (selected == 2) HandleSaveGame();
            else if (selected == 3)
            {
                SceneManager.LoadScene("StartScene");
                gameObject.SetActive(false);
                return;
            }
            else HandleExit();
        }
        bgmBar.offsetMax = new Vector2((int)Math.Round(SoundManager.instance.GetBGMVolume() * 160.0f / 5) - 160, bgmBar.offsetMax.y);
        sfxBar.offsetMax = new Vector2((int)Math.Round(SoundManager.instance.GetSFXVolume() * 160.0f / 5) - 160, sfxBar.offsetMax.y);

        bgm.SetActive(selected == 0);
        sfx.SetActive(selected == 1);
        save.SetActive(selected == 2);
        mainMenu.SetActive(selected == 3);
        exit.SetActive(selected == 4);
    }
    private void HandleSaveGame()
    {
        string dialog;
        SaveGameStatus status = SaveGameManager.SaveGame();
        if (status == SaveGameStatus.SUCCESS) dialog = "Game saved successfully!";
        else if (status == SaveGameStatus.CANT_SAVE) dialog = "You can't save game now!";
        else dialog = "Failed to save game";
        GameController.instance.SetGameState(GAME_STATE.DIALOG);
        DialogManager.instance.ShowDialog(dialog);
        gameObject.SetActive(false);
    }
    private void handleBar(bool isBgm = true)
    {
        if (isBgm)
        {
            int vol = SoundManager.instance.GetBGMVolume();
            if (Input.GetKeyDown(KeyCode.RightArrow) && vol < 5)
            {
                SoundManager.instance.SetBGMVolume(vol + 1);
            }

            else if (Input.GetKeyDown(KeyCode.LeftArrow) && vol > 0)
            {
                SoundManager.instance.SetBGMVolume(vol - 1);

            }
        }
        else
        {
            int vol = SoundManager.instance.GetSFXVolume();
            if (Input.GetKeyDown(KeyCode.RightArrow) && vol < 5)
            {
                SoundManager.instance.SetSFXVolume(vol + 1);

            }

            else if (Input.GetKeyDown(KeyCode.LeftArrow) && vol > 0)
            {
                SoundManager.instance.SetSFXVolume(vol - 1);

            }
        }
    }
    private void HandleExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
