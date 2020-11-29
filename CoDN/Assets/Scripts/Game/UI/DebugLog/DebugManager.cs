using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    [SerializeField] private GameObject textTemplate;

    private List<GameObject> textItems;
    [SerializeField] private int maxTextLogs;

    private void Start()
    {
        textItems = new List<GameObject>();
    }

    public void LogText(string logText)
    {
        if(textItems.Count >= maxTextLogs)
        {
            GameObject tempItem = textItems[0];
            textItems.Remove(tempItem);
            Destroy(tempItem.gameObject);
        }

        GameObject newText = Instantiate(textTemplate) as GameObject;
        newText.SetActive(true);
        Color c = Color.black;

        newText.GetComponent<TextLogItem>().SetText(logText, c);
        newText.transform.SetParent(textTemplate.transform.parent, false);

        textItems.Add(newText.gameObject);
    }

    public void LogTextColor(string logText, Color logColor)
    {
        if (textItems.Count >= maxTextLogs)
        {
            GameObject tempItem = textItems[0];
            textItems.Remove(tempItem);
            Destroy(tempItem.gameObject);
        }

        GameObject newText = Instantiate(textTemplate) as GameObject;
        newText.SetActive(true);
        
        newText.GetComponent<TextLogItem>().SetText(logText, logColor);
        newText.transform.SetParent(textTemplate.transform.parent, false);

        textItems.Add(newText.gameObject);
    }
}
