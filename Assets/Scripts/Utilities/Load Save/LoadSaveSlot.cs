using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class LoadSaveSlot : MonoBehaviour, ISelectHandler
{
    [SerializeField] private string saveName;

    [SerializeField] private TextMeshProUGUI saveNameTitle;





    [SerializeField] private TextMeshProUGUI emptySave; 

    void Awake()
    {

    }

    public void GetSaveData()
    {

    }

    public void OnSelect(BaseEventData data)
    {
        OnClick();
    }

    public void OnClick()
    {
        UIDialogBox.Instance.ShowPopUp("Would you like to save to this save slot?", ConfirmSave, CancelSave);
    }

    private void ConfirmSave()
    {
        #if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        LoadSaveManager.instance.StandalonePCSave(saveName);
        #endif
        UIDialogBox.Instance.HidePopUp();
    }

    private void CancelSave()
    {
        UIDialogBox.Instance.HidePopUp();
    }
}
