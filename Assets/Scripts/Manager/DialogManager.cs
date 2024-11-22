using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;

    public static DialogManager instance { get; private set; }
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
    public void ShowDialog(string dialog)
    {
        dialogBox.SetActive(true);
        dialogText.text = dialog;
    }
    public void HideDialog()
    {
        dialogBox.SetActive(false);
    }

}
