using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeyRebindController : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset inputActions;

    [Header("UI - Buttons")]
    [SerializeField] private Button applyChangesButton;
    [SerializeField] private Button resetChangesButton;
    [SerializeField] private Button backButton;

    [Header("UI - Rebind Controls")]
    [SerializeField] private UIRebindControl[] rebindControls;

    private static Dictionary<string, string> OverridesDictionary = new Dictionary<string, string>();

    // JSON string to be called later on for saving and loading
    private string savedInputOverrides = "";

    void Awake()
    {
        foreach (UIRebindControl control in rebindControls)
        {
            control.UpdateBehaviour();
        }
    }

    public void ApplyChanges()
    {
        // Save the changes into a dictionary to put into the save file later on
        WriteJson();
        Debug.Log(savedInputOverrides);
        LoadSaveManager.instance.SetBindingsJson(savedInputOverrides);
    }

    public void LoadChanges(string data)
    {
        // Load the saved input overrides from the save file. Assume we get a string that is valid JSON
        if (OverridesDictionary == null)
            OverridesDictionary = new Dictionary<string, string>();
        else
            OverridesDictionary.Clear();

        if (string.IsNullOrEmpty(data))
            Logger.Debug("JSON String is null or empty");

        var overridesData = JsonMapper.ToObject<JsonData>(data);

        if (JsonDataContainsKey(overridesData, "bindings")) {
            if (!overridesData["bindings"].IsArray) {
                Logger.Error("There are no binding overrides");
                return;
            }

            JsonData bindingOverrides = overridesData["bindings"];
            foreach (JsonData binding in bindingOverrides) {
                Guid actionID;
                int bindingID = 0;
                string path = "";

                if (JsonDataContainsKey(binding, "actionId")) {
                    string actionParse = binding["actionId"].ToString();

                    // Now split the string to get the action ID and the binding ID
                    string[] split = actionParse.Split(new string[] { " : " }, StringSplitOptions.None);

                    actionID = Guid.Parse(split[0]);
                    bindingID = int.Parse(split[1]);
                } else {
                    Debug.Log("<color=red>Failed parsing the action ID</color>");
                    break;
                }

                if (JsonDataContainsKey(binding, "path")) {
                    path = binding["path"].ToString();
                } else {
                    Debug.Log("<color=red>Failed parsing the action path</color>");
                    break;
                }

                // Add the action back to the dictionary
                AddOverrideToDictionary(actionID, path, bindingID);

                // Now apply the override
                inputActions.FindAction(actionID).ApplyBindingOverride(bindingID, path);
            }
        }
    }

    public void ResetChanges()
    {
        // Reset all of the changes that have been made to the default control configuration
        foreach (UIRebindControl control in rebindControls)
        {
            control.ResetBinding();
        }
    }

    public static void AddOverrideToDictionary(Guid actionId, string path, int bindingIndex)
    {
        string key = string.Format("{0} : {1}", actionId.ToString(), bindingIndex);

        if (OverridesDictionary.ContainsKey(key))
        {
            OverridesDictionary[key] = path;
        }
        else
        {
            OverridesDictionary.Add(key, path);
        }
    }

    private void WriteJson()
    {
        StringBuilder sb = new StringBuilder();
        JsonWriter writer = new JsonWriter(sb);

        writer.WriteObjectStart();

        writer.WritePropertyName("bindings");
        writer.WriteArrayStart();

        foreach (KeyValuePair<string, string> pair in OverridesDictionary)
        {
            writer.WriteObjectStart();
            writer.WritePropertyName("actionId");
            writer.Write(pair.Key);
            writer.WritePropertyName("path");
            writer.Write(pair.Value);
            writer.WriteObjectEnd();
        }

        writer.WriteArrayEnd();

        writer.WriteObjectEnd();

        savedInputOverrides = sb.ToString();
    }

    static public bool JsonDataContainsKey(JsonData data, string key)
    {
        bool result = false;
        if (data == null)
            return result;
        if (!data.IsObject)
            return result;
        IDictionary tdictionary = data as IDictionary;
        if (tdictionary == null)
            return result;
        if (tdictionary.Contains(key) && tdictionary[key] != null)
            result = true;

        return result;
    }
}
