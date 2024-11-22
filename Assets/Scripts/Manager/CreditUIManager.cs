using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditUIManager : MonoBehaviour
{
    private bool animationEnded = false;
    private CanvasGroup cnv;
    public void Awake()
    {
        cnv = DDOL_GameObj_Manager.instance.BgUICanvasGr;
    }
    public void OnAnimationEnd()
    {
        animationEnded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (animationEnded && Input.GetKeyDown(KeyCode.Return))
        {
            cnv.alpha = 0f;
            SceneManager.LoadScene(0);
        }
    }
}
