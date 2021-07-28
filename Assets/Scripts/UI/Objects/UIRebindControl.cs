using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class UIRebindControl : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference action;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private Button inputButton;
    [SerializeField] private Button resetButton;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
