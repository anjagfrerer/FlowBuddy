using UnityEngine;
using System;

public class DragonSatisfactionManager : MonoBehaviour
{
    public static DragonSatisfactionManager Instance { get; private set; }

    public static event Action<float> OnSatisfactionChanged; 
    public static event Action<SatisfactionState> OnMoodStateChanged;

    public enum SatisfactionState { Happy, Neutral, Sad, Critical }
    private SatisfactionState currentStage = SatisfactionState.Happy;

    [Header("State (0.0 to 1.0)")]
    [SerializeField] private float currentSatisfaction;

    [Header("Threshold Settings")]
    [SerializeField] private float thresholdNeutral = 0.75f;
    [SerializeField] private float thresholdSad = 0.45f;
    [SerializeField] private float thresholdCritical = 0.15f;

    [Header("Android Notification Settings")]
    [Tooltip("What percentage does the dragon lose per hour? (0.05 = 5%)")]
    [SerializeField] private float satisfactionLossPerHour = 0.05f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            InitializeAndLoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateGameplayAndUI();
    }

    private void InitializeAndLoadData()
    {
        if (!PlayerPrefs.HasKey("Dragon_Satisfaction"))
        {
            Debug.Log("No data found. Initializing dummy data: Start satisfaction at 80% (0.8f)");
            currentSatisfaction = 0.8f; 
            SaveSatisfaction();
        }
        else
        {
            currentSatisfaction = PlayerPrefs.GetFloat("Dragon_Satisfaction");
            Debug.Log($"Base data loaded. Saved satisfaction: {currentSatisfaction * 100}%");
            
            CalculateOfflineLoss();
        }
    }

    private void CalculateOfflineLoss()
    {
        if (PlayerPrefs.HasKey("Dragon_LastQuitTime"))
        {
            string lastQuitTimeString = PlayerPrefs.GetString("Dragon_LastQuitTime");
            DateTime lastQuitTime = DateTime.Parse(lastQuitTimeString);
            DateTime now = DateTime.Now;

            TimeSpan timePassed = now - lastQuitTime;
            float totalHoursPassed = (float)timePassed.TotalHours;

            if (totalHoursPassed > 0)
            {
                float satisfactionToDeduct = totalHoursPassed * satisfactionLossPerHour;
                
                currentSatisfaction = Mathf.Clamp01(currentSatisfaction - satisfactionToDeduct);
                
                Debug.Log($"Offline Catchup: {totalHoursPassed:F2} hours have passed. " +
                          $"Dragon loses {satisfactionToDeduct * 100:F1}% satisfaction. " +
                          $"New satisfaction: {currentSatisfaction * 100:F1}%");
                
                SaveSatisfaction();
            }
        }
    }

    public void ModifySatisfaction(float amount)
    {
        currentSatisfaction = Mathf.Clamp01(currentSatisfaction + amount);
        UpdateGameplayAndUI();
        SaveSatisfaction();
    }

    private void CheckSatisfactionThresholds(float percentage)
    {
        SatisfactionState previousStage = currentStage;

        if (percentage <= thresholdCritical)
            currentStage = SatisfactionState.Critical;
        else if (percentage <= thresholdSad)
            currentStage = SatisfactionState.Sad;
        else if (percentage <= thresholdNeutral)
            currentStage = SatisfactionState.Neutral;
        else
            currentStage = SatisfactionState.Happy;

        if (currentStage != previousStage)
        {
            OnMoodStateChanged?.Invoke(currentStage);
        }
    }

    private void UpdateGameplayAndUI()
    {
        OnSatisfactionChanged?.Invoke(currentSatisfaction);
        CheckSatisfactionThresholds(currentSatisfaction);
    }

    public float GetCurrentSatisfaction() => currentSatisfaction;
    public SatisfactionState GetCurrentMoodState() => currentStage;

    private void SaveSatisfaction()
    {
        PlayerPrefs.SetFloat("Dragon_Satisfaction", currentSatisfaction);
        PlayerPrefs.Save();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            PlayerPrefs.SetString("Dragon_LastQuitTime", DateTime.Now.ToString());
            PlayerPrefs.Save();

            if (NotificationSettingsManager.Instance != null && !NotificationSettingsManager.Instance.AreNotificationsEnabled())
            {
                return;
            }

            if (MobileNotificationManagerAndroid.Instance != null)
            {
                float abstandBisTraurig = currentSatisfaction - thresholdSad; 

                if (abstandBisTraurig > 0)
                {
                    float punkteVerlustProSekunde = satisfactionLossPerHour / 3600f; 
                    float sekundenBisTraurig = abstandBisTraurig / punkteVerlustProSekunde;

                    sekundenBisTraurig = 10f;

                    MobileNotificationManagerAndroid.Instance.ScheduleAndroidNotification(
                        "Your dragon misses you!",
                        "Its satisfaction has dropped significantly. Come back and check on it!",
                        sekundenBisTraurig
                    );
                }
            }
        }
        else
        {
            if (MobileNotificationManagerAndroid.Instance != null)
            {
                MobileNotificationManagerAndroid.Instance.CancelAllAndroidNotifications();
            }

            CalculateOfflineLoss();
            UpdateGameplayAndUI();
        }
    }

    [ContextMenu("Reset Satisfaction Dummy Data")]
    public void ResetToDummyData()
    {
        PlayerPrefs.DeleteKey("Dragon_Satisfaction");
        PlayerPrefs.DeleteKey("Dragon_LastQuitTime");
        InitializeAndLoadData();
        UpdateGameplayAndUI();
    }
}