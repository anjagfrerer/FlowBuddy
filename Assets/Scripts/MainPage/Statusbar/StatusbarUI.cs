using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarUI : MonoBehaviour
{
    [SerializeField] AnimationCurve curve = AnimationCurve.EaseInOut(0,0,1,1);

    [Header("Animation Parameters")]
    [SerializeField] 
    float duration = 0.5f;
    private Slider statusBar;

    void Awake()
    {
        statusBar = GetComponent<Slider>();
        statusBar.value = DataManager.Instance.appData.StatusValue;
    }
    public void UpdateStatusBar(int value)
    {
        StartCoroutine(StatusBarCoroutine(statusBar.value,value));
    }

    private IEnumerator StatusBarCoroutine(float startValue, float endValue)
    {
        float time = 0f;
        do
        {
            time += Time.deltaTime;
            float t = curve.Evaluate(time/duration);
            statusBar.value = Mathf.Lerp(startValue,endValue,t);
            yield return null;
        } while (time < duration);

        statusBar.value = endValue;
    }
}