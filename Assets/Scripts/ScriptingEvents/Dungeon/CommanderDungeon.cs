using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Yarn.Unity;

public class CommanderDungeon : NPC
{
    [Header("Objectives")]
    [SerializeField] private Objective objective;
    [SerializeField] private ObjectivesPanel panel;

    [Header("Overlord")]
    [SerializeField] private DungeonOverlord overlord;

    [Inject] private ObjectivesManager objectivesManager;

    [YarnCommand("setdungeonobjective")]
    public void SetObjective()
    {
        // Setup Objectives
        objectivesManager.SetCurrentObjective(objective);
        ((NavigationObjective)objective).ResetObjective();
        panel.Initialize();
    }

    public override void StartDialogue()
    {

    }
}
