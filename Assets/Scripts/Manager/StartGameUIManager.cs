using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameUIManager : MonoBehaviour
{

    private int selected = 0;
    private int state = 0;
    [SerializeField] GameObject newgameSelected, continueSelected, exitSelected;

    private void Start()
    {
        MainCameraController.instance.SetCameraStaticPos(Vector2.zero);
        //UIManager.instance.gameObject.SetActive(false);
        PlayerController.Instance.setActive(false);
    }

    public void Update()
    {
        if (state == 1)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                DialogManager.instance.HideDialog();
                state = 0;
            }
            return;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && selected > 0)
        {
            selected--;
            return;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && selected < 2)
        {
            selected++;
            return;

        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (selected == 0) { HandleNewGame(); return; }
            if (selected == 1) { HandleContinue(); return; }
            HandleExit();
        }

        newgameSelected.SetActive(selected == 0);
        continueSelected.SetActive(selected == 1);
        exitSelected.SetActive(selected == 2);
    }
    private void HandleNewGame()
    {
        AnimationFade.instance.FadeOut();
        SceneManager.LoadScene("SampleScene");
    }
    private void HandleContinue()
    {
        if (!SaveGameManager.instance.LoadGame()) { DialogManager.instance.ShowDialog("You have no saved game yet!"); state = 1; return; }
        AnimationFade.instance.FadeIn();

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