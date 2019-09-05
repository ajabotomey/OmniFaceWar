using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionController : MonoBehaviour
{
    [SerializeField] private Question question;
    [SerializeField] private TMPro.TMP_Text questionText;
    [SerializeField] private Button choiceButton;

    private List<ChoiceController> choiceControllers = new List<ChoiceController>();

    public void Change(Question _question)
    {
        RemoveChoices();
        question = _question;
        gameObject.SetActive(true);
        Initialize();
    }

    public void Hide(Conversation conversation)
    {
        RemoveChoices();
        gameObject.SetActive(false);
    }

    public void Hide()
    {
        RemoveChoices();
        gameObject.SetActive(false);
    }

    private void RemoveChoices()
    {
        foreach (ChoiceController c in choiceControllers) {
            Destroy(c.gameObject);
        }

        choiceControllers.Clear();
    }

    private void Initialize()
    {
        questionText.SetText(question.GetText());

        for (int index = 0; index < question.GetChoiceLength(); index++) {
            ChoiceController c = ChoiceController.AddChoiceButton(choiceButton, question.GetChoice(index), index);
            choiceControllers.Add(c);
        }

        choiceButton.gameObject.SetActive(false);
    }
}
