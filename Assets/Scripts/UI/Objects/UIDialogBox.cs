using System;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogBox : MonoBehaviour
{
    public static UIDialogBox Instance = null;
    [SerializeField] private Text dialogText;
    [SerializeField] private Button ConfirmButton;
    [SerializeField] private Button CancelButton;
    [SerializeField] private Button OkButton;

    private Action onClickConfirm;
    private Action onClickCancel;

    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void ShowDialogBox(string dialogText)
    {

    }

    public void ShowDialogBox(string dialogText, Action onConfirm, Action onCancel)
    {

    }
}
