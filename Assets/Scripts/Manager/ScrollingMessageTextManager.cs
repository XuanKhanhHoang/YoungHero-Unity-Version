using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingMessageTextManager : MonoBehaviour
{
    [SerializeField] GameObject textPrefab;

    private Queue<GameObject> textQueue = new Queue<GameObject>();

    public static ScrollingMessageTextManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void AddText(string message)
    {
        GameObject newText = Instantiate(textPrefab, gameObject.transform);
        newText.GetComponent<TextMeshProUGUI>().text = message;
        textQueue.Enqueue(newText);
        StartCoroutine(RemoveTextAfterDelay(newText, 1f));
    }

    private IEnumerator RemoveTextAfterDelay(GameObject textObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        textQueue.Dequeue();
        Destroy(textObject);
    }
}

