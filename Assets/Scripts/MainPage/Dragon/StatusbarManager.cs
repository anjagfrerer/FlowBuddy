using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The Dragon Script makes the dragon an instance, and therefore persists his gameobject through scenes
/// </summary>
public class StatusBarManager : MonoBehaviour
{
    public static Action<int> OnStatusValueAdd;
    private StatusBarUI statusBar;

    void Awake()
    {
        statusBar = GetComponent<StatusBarUI>();
    }

    void Start()
    {
        statusBar.UpdateStatusBar(DataManager.Instance.appData.StatusValue);
        DragonRenderer.OnChangeMood(CheckMood(DataManager.Instance.appData.StatusValue));
    }

    void OnEnable()
    {
        OnStatusValueAdd += AddStatusValue;
    }

    void OnDisable()
    {
        OnStatusValueAdd -= AddStatusValue;
    }

    void AddStatusValue(int value)
    {
        DataManager.Instance.appData.StatusValue += value;
        DataManager.Instance.SaveData();
        Debug.Log(DataManager.Instance.appData.StatusValue);
        DragonRenderer.OnChangeMood(CheckMood(DataManager.Instance.appData.StatusValue));
    }

    [Header("")]
    [SerializeField] int maxHappy = 60;
    [SerializeField] int maxOkay = 20;

    Mood CheckMood(int value)
    {
        if(value > maxHappy) return Mood.Happy;
        if(value > maxOkay) return Mood.Okay;
        return Mood.Sad;

    }
}
