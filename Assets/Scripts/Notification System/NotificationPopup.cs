using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class NotificationPopup : MonoBehaviour
{
    [Header("NotificationUI Elements")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text text;
    [SerializeField] private float speed;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float popupUpTime;
    [SerializeField] private RectTransform rect;
    [SerializeField] private Image image;

    [Header("Game Events")]
    [SerializeField] private VoidEvent NotificationPushed;

    private Vector2 position = Vector2.zero;
    private float elapsedTime;
    public bool IsFading { get; set; }
    private Notification notification;

    [Inject] private RewiredActionManager rewiredActionManager;
    [Inject] private GameUIController gameUI;

    void Start()
    {
        //Destroy(this.gameObject, 5.0f);
        elapsedTime = 0.0f;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!notification.IsQueued) { // TODO: Swap to a possible inQueue boolean
            if (rect.localPosition.toVector2() != position) {
                // Move to new position
                float step = speed * Time.deltaTime;
                rect.localPosition = Vector2.MoveTowards(rect.localPosition, position, step);
            } else {
                // Raise event here
                NotificationPushed.Raise();
            }

            if (elapsedTime > popupUpTime) {
                if (!IsFading) {
                    FadeOut();
                }
            }

            if (!gameUI.IsInteractingWithUI())
                elapsedTime += Time.deltaTime;
        }
    }

    public void SetNotification(Notification notification)
    {
        this.notification = notification;
    }

    public void ShowNotification()
    {
        if (notification.RewiredAction == -1) { // No action here
            image.sprite = notification.Image;
        } else { // We have an action
            image.sprite = notification.Image = rewiredActionManager.GetSpriteFromAction(notification.RewiredAction);
        }

        titleText.text = notification.Title;
        string parsedText = ParseEmojis.Parse(notification.AbbreviatedText);
        text.text = parsedText;
        notification.Pushed = true;
        notification.IsQueued = false;

        elapsedTime = 0.0f;
    }

    public void Disappear()
    {
        Destroy(gameObject);
    }

    public void SetPosition(Vector2 position)
    {
        this.position = position;
    }

    public void FadeOut()
    {
        StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 0));
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 0.5f)
    {
        float timeStartedLerping = Time.time;
        float timeSinceStarted;
        float percentageComplete;

        while (true) {
            timeSinceStarted = Time.time - timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            cg.alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();
        }

        IsFading = true;
        Destroy(gameObject);
    }

    public class Factory : PlaceholderFactory<NotificationPopup> { }
}
