using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;

public class ObjectivesPanel : MonoBehaviour
{
    [Header("Text Fields")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text objectiveText;

    [Inject] private ObjectivesManager manager;
    private Objective currentObjective;

    // Start is called before the first frame update
    public void Initialize()
    {
        currentObjective = manager.GetCurrentObjective();
        titleText.text = currentObjective.GetTitle();
        descriptionText.text = currentObjective.GetDescription();

        if (currentObjective is EliminationObjective) {
            EliminationObjective objective = (EliminationObjective)currentObjective;

            objectiveText.text = "Killed: " + objective.GetKillCount() + " / " + objective.GetKillsRequired();
        }
    }

    public void UpdateEliminationObjective()
    {
        if (currentObjective.IsComplete()) {
            objectiveText.text = "Objective complete!";
            return;
        }

        EliminationObjective objective = (EliminationObjective)currentObjective;

        objectiveText.text = "Killed: " + objective.GetKillCount() + " / " + objective.GetKillsRequired();
    }
}
