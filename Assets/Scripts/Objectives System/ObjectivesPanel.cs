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
    [SerializeField] private VoidEvent objectiveComplete; // TODO: Remove this once heatmap data is collected

    [Inject] private ObjectivesManager manager;
    private Objective currentObjective;

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
            objectiveComplete.Raise();
            return;
        }

        EliminationObjective objective = (EliminationObjective)currentObjective;

        objectiveText.text = "Killed: " + objective.GetKillCount() + " / " + objective.GetKillsRequired();
    }
}
