using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarUI : MonoBehaviour
{
    private Slider statusBar;

    void Awake()
    {
        statusBar = GetComponent<Slider>();
        
    }
    public void UpdateStatusBar(int value)
    {
        statusBar.value = value;
    }

    private IEnumerator StatusBarCoroutine()
    {
        yield return null;
    }
}