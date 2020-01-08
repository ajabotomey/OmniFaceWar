using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAssistantText : MonoBehaviour
{
    [SerializeField] private StringEvent stringEvent;
    [SerializeField] private string testText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        stringEvent.Raise(testText);
    }
}
