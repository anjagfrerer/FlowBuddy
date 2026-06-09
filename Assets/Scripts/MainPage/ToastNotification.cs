using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class ToastNotification : MonoBehaviour
{
    private TextMeshProUGUI textComponent;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        textComponent = GetComponentInChildren<TextMeshProUGUI>();
        
        canvasGroup.alpha = 0f;
    }

    public void ShowToast(string message, float duration = 2.5f)
    {
        gameObject.SetActive(true);

        if (textComponent != null)
        {
            textComponent.text = message;
        }

        StopAllCoroutines();
        
        StartCoroutine(ToastRoutine(duration));
    }

    private IEnumerator ToastRoutine(float duration)
    {
        float counter = 0f;
        float fadeDuration = 0.3f;
        while (counter < fadeDuration)
        {
            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, counter / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(duration);

        counter = 0f;
        while (counter < fadeDuration)
        {
            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, counter / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;

        gameObject.SetActive(false);
    }
}