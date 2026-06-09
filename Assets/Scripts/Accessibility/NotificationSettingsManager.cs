using UnityEngine;
using Unity.Notifications.Android;

public class NotificationSettingsManager : MonoBehaviour
{
    public static NotificationSettingsManager Instance { get; private set; }

    private const string ChannelId = "dragon_satisfaction_channel";

    private bool notificationsEnabled = true;
    private bool soundEnabled = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateNotificationChannel();
    }

    public void SetNotificationsEnabled(bool enabled)
    {
        notificationsEnabled = enabled;
        PlayerPrefs.SetInt("Settings_NotificationsEnabled", enabled ? 1 : 0);
        PlayerPrefs.Save();

        UpdateNotificationChannel();

        if (!enabled && MobileNotificationManagerAndroid.Instance != null)
        {
            MobileNotificationManagerAndroid.Instance.CancelAllAndroidNotifications();
        }
    }

    public void SetSoundEnabled(bool enabled)
    {
        soundEnabled = enabled;
        PlayerPrefs.SetInt("Settings_SoundEnabled", enabled ? 1 : 0);
        PlayerPrefs.Save();

        UpdateNotificationChannel();
    }

    public bool AreNotificationsEnabled() => notificationsEnabled;
    public bool IsSoundEnabled() => soundEnabled;

    public void UpdateNotificationChannel()
    {
        var channel = new AndroidNotificationChannel();
        channel.Id = ChannelId;
        channel.Name = "Dragon Status";
        channel.Description = "Notifications regarding the satisfaction of your dragon";

        if (!notificationsEnabled)
        {
            channel.Importance = Importance.None;
        }
        else
        {
            if (!soundEnabled)
            {
                channel.Importance = Importance.Default;
            }
            else
            {
                channel.Importance = Importance.High;
            }
        }

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    private void LoadSettings()
    {
        notificationsEnabled = PlayerPrefs.GetInt("Settings_NotificationsEnabled", 1) == 1;
        soundEnabled = PlayerPrefs.GetInt("Settings_SoundEnabled", 1) == 1;
    }
}