using UnityEngine;
using Unity.Notifications.Android;
using System;

public class MobileNotificationManagerAndroid : MonoBehaviour
{
    public static MobileNotificationManagerAndroid Instance { get; private set; }

    private const string ChannelId = "dragon_satisfaction_channel";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ScheduleAndroidNotification(string title, string body, float delayInSeconds)
    {
        CancelAllAndroidNotifications();

        var androidNotification = new AndroidNotification
        {
            Title = title,
            Text = body,
            FireTime = DateTime.Now.AddSeconds(delayInSeconds),
            SmallIcon = "icon_0",
            LargeIcon = "icon_1"
        };

        AndroidNotificationCenter.SendNotification(androidNotification, ChannelId);
        Debug.Log($"Android Notification scheduled: '{title}' in {delayInSeconds} seconds.");
    }

    public void CancelAllAndroidNotifications()
    {
        AndroidNotificationCenter.CancelAllScheduledNotifications();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            CancelAllAndroidNotifications();
        }
    }
}