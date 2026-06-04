
using System;

/// <summary>
/// MoodEvent get called by the Moodmanager
/// 
/// Subscribe to the moodEvent via 
///     OnChangeMood += myFunction; // Put it in OnEnable()
/// 
/// Unsubscribe to the moodEvent via // Put it in OnDisable()
///     OnChangeMood -= myFunction;
/// 
/// My Function requires Mood as an input parameter
/// 
/// </summary>
public static class MoodEvents
{
    public static Action<Mood> OnChangeMood;
}