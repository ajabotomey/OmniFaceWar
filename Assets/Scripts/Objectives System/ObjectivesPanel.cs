using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

public class ObjectivesPanel : MonoBehaviour
{
    [Header("Text Fields")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text objectiveText;

    [Header("Canvas Group")]
    [SerializeField] private CanvasGroup cg;

    [Header("Game Events")]
    [SerializeField] private VoidEvent objectiveComplete;

    [Header("Animation")]
    [SerializeField] private GameObject panel;
    [SerializeField] private RectTransform panelRect;
    [SerializeField] private RectTransform offscreenRect;
    [SerializeField] private float animationSpeed;

    [Inject] private ObjectivesManager manager;

    private Objective currentObjective;
    private bool isHidden;
    private Vector2 originalPos;
    private Vector2 offscreenPos;

    void Start()
    {
        isHidden = false;

        if (currentObjective == null) {
            cg.alpha = 0;
        } else {
            cg.alpha = 100;
        }

        // Record positions
        originalPos = panelRect.localPosition;
        offscreenPos = offscreenRect.localPosition;
    }

    void Update()
    {
        if (isHidden)
        {
            if (panelRect.localPosition.toVector2() != offscreenPos)
            {
                MoveToPosition(offscreenPos);
            }
        } else {
            if (panelRect.localPosition.toVector2() != originalPos)
            {
                MoveToPosition(originalPos);
            }
        }
    }

    private void MoveToPosition(Vector2 position)
    {
        float step = animationSpeed * Time.deltaTime;
        panelRect.localPosition = Vector2.MoveTowards(panelRect.localPosition, position, step);
    }

    public void Initialize()
    {
        currentObjective = manager.GetCurrentObjective();
        titleText.text = currentObjective.GetTitle();
        descriptionText.text = currentObjective.GetDescription();

        if (currentObjective is EliminationObjective) {
            EliminationObjective objective = (EliminationObjective)currentObjective;

            objectiveText.text = "Killed: " + objective.GetKillCount() + " / " + objective.GetKillsRequired();
        } else {
            objectiveText.text = "";
        }
    }

    public void Hide()
    {
        if (cg.alpha == 0)
            return;

        StartCoroutine(FadeCanvasGroup(cg, 100, 0));
    }

    public void Show()
    {
        if (cg.alpha == 100)
            return;

        StartCoroutine(FadeCanvasGroup(cg, 0, 100));
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

    public void UpdateNavigationObjective()
    {
        if (currentObjective.IsComplete()) {
            objectiveText.text = "Objective complete!";
            //objectiveComplete.Raise();
            return;
        }

        objectiveText.text = "";
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 0.5f)
    {
        float timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true) {
            timeSinceStarted = Time.time - timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            cg.alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();
        }

        //Destroy(this.gameObject);
    }

    public void TogglePanel()
    {
        isHidden = !isHidden;
    }
}
