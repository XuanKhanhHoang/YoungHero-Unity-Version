using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DIedUIManager : MonoBehaviour
{
    public static DIedUIManager instance;

    private int selected = 0;
    [SerializeField] GameObject dieObj;
    [SerializeField] GameObject respwanSelected, restartSelected, exitSelected;

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
    public void HandleUpdate()
    {
        dieObj.SetActive(true);
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
            if (selected == 0) { HandleRespawn(); return; }
            if (selected == 1) { HandleRestart(); return; }
            HandleExit();
        }

        respwanSelected.SetActive(selected == 0);
        restartSelected.SetActive(selected == 1);
        exitSelected.SetActive(selected == 2);
    }
    private void HandleRespawn()
    {
        PlayerRespawnManager.instance.Respawn();
        dieObj.SetActive(false);
    }
    private void HandleRestart()
    {
        dieObj.SetActive(false);
        PlayerController.Instance.RemoveInstancePlayer();
        SceneManager.LoadScene("SampleScene");
    }
    private void HandleExit()
    {
        dieObj.SetActive(false);
        SceneManager.LoadScene("StartScene");
    }
}
