using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class UIDialogBox : MonoBehaviour
{
    public static UIDialogBox Instance = null;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private Button ConfirmButton;
    [SerializeField] private Button CancelButton;
    [SerializeField] private Button OkButton;

    private Action onClickConfirm;
    private Action onClickCancel;

    [Inject] private SettingsManager settings;

    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void ShowPopUp(string popupText)
    {
        ShowPopUp(popupText, HidePopUp, null);
    }

    public void ShowPopUp(string popupText, Action onConfirm, Action onCancel)
    {
        if (!gameObject.activeSelf) {
            if (onCancel == null) {
                ConfirmButton.gameObject.SetActive(false);
                CancelButton.gameObject.SetActive(false);
                OkButton.gameObject.SetActive(true);

                OkButton.Select();
                OkButton.OnSelect(null);
            } else {
                ConfirmButton.gameObject.SetActive(true);
                CancelButton.gameObject.SetActive(true);
                OkButton.gameObject.SetActive(false);
                onClickCancel = onCancel;

                ConfirmButton.Select();
                ConfirmButton.OnSelect(null);
            }

            dialogText.text = popupText;
            onClickConfirm = onConfirm;
            gameObject.SetActive(true);
            settings.UpdateFont();
        } else {
            Debug.LogWarning("There is already pop up window on scene!\nWith text: " + dialogText.text);
        }
    }

    public void HidePopUp()
    {
        gameObject.SetActive(false);
    }

    public void OnClickConfirmButton()
    {
        onClickConfirm();
    }

    public void OnClickCancelButton()
    {
        onClickCancel();
        gameObject.SetActive(false);
    }
}
