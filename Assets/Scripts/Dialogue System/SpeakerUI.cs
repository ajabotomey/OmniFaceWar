using UnityEngine;
using UnityEngine.UI;

public class SpeakerUI : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text fullName;
    [SerializeField] private TMPro.TMP_Text dialogText;

    private Character speaker;
    public Character Speaker {
        get { return speaker; }
        set {
            speaker = value;
            fullName.text = speaker.GetFullName();
        }
    }

    public string Dialog {
        set {
            dialogText.SetText(value);
        }
    }

    public bool HasSpeaker()
    {
        return speaker != null;
    }

    public bool SpeakerIs(Character character)
    {
        return speaker == character;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
