using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmniFaceAssistant : MonoBehaviour
{
    [SerializeField] private GameObject textBubble;
    [SerializeField] private TMPro.TMP_Text bubbleText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Set a timer for text bubble visibility
    }

    public void ShowText(string text)
    {
        textBubble.SetActive(true);
        bubbleText.text = text;
    }

    public void ShowText(int num)
    {
        textBubble.SetActive(true);
        bubbleText.text = " " + num;
    }
}
