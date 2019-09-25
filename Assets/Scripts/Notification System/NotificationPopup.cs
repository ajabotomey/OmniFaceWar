using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class NotificationPopup : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float speed;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float popupUpTime;

    private Vector2 position = Vector2.zero;
    private float elapsedTime;
    private bool isFading = false;

    void Start()
    {
        //Destroy(this.gameObject, 5.0f);
        elapsedTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.toVector2() != position) {
            // Move to new position
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, position, step);
        }

        if (elapsedTime > popupUpTime) {
            if (!isFading) {
                FadeOut();
            }
        }

        elapsedTime += Time.deltaTime;
    }

    public void ShowNotification(Notification notification)
    {
        string parsedText = ParseEmojis.Parse(notification.AbbreviatedText);
        text.text = parsedText;
        notification.Push();
    }

    public void Disappear()
    {
        Destroy(this.gameObject);
    }

    public void SetPosition(Vector2 position)
    {
        this.position = position;
    }

    public void FadeOut()
    {
        StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 0));
        isFading = true;
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

        Destroy(this.gameObject);
    }

    public class Factory : PlaceholderFactory<NotificationPopup> { }
}
