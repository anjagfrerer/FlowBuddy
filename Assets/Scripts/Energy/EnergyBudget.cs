using System;
using System.Globalization;
using UnityEngine;

public enum DayEnergyMode { Rest = 0, Low = 1, Normal = 2, High = 3 }

/// <summary>
/// Energie-Konto nach der Spoon Theory: Jeder Tag startet mit einer Anzahl Löffel,
/// erledigte Aufgaben ziehen ihren Aufwand davon ab. Einzige Quelle für das Energie-Limit.
/// </summary>
public static class EnergyBudget
{
    public const int MinEffort = 1;
    public const int MaxEffort = 5;
    public const int DefaultBaseline = 12;
    // Reihenfolge wie die Buttons in den Settings: Low, Medium, High, Very High
    public static readonly int[] BaselinePresets = { 6, 12, 18, 24 };

    // Die Statusbar bekam früher Minuten; 1 Löffel = 5 Minuten hält ihre Balance gleich
    public const int StatusPointsPerSpoon = 5;

    // Jede Änderung (Abhaken, Tagesmodus, Basiswert), z.B. für die Anzeige
    public static event Action OnEnergyChanged;
    // Nur wenn sich das Tagesbudget ändert, das Lernpaket muss dann neu gebaut werden
    public static event Action OnBudgetChanged;

    private const string LegacyPrefsKey = "User_Selected_EnergyLimit";
    private const int CurrentModelVersion = 1;

    public static bool IsReady => DataManager.Instance != null && DataManager.Instance.appData != null;

    public static string Today => DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

    private static User User => DataManager.Instance.appData.user;

    public static int Baseline => IsReady ? User.energyLimit : DefaultBaseline;

    public static DayEnergyMode TodayMode
    {
        get
        {
            if (!IsReady) return DayEnergyMode.Normal;
            EnsureToday();
            return (DayEnergyMode)User.todayEnergyMode;
        }
    }

    public static int TodayBudget => BudgetFor(Baseline, TodayMode);

    public static int SpentToday
    {
        get
        {
            if (!IsReady) return 0;
            EnsureToday();
            return User.energySpentToday;
        }
    }

    public static int Remaining => Mathf.Max(0, TodayBudget - SpentToday);

    // Ruhetag oder Energie aufgebraucht: der Drache verliert dann keine Zufriedenheit
    public static bool IsResting => IsReady && (TodayMode == DayEnergyMode.Rest || Remaining == 0);

    public static int BudgetFor(int baseline, DayEnergyMode mode) => mode switch
    {
        DayEnergyMode.Rest => 0,
        DayEnergyMode.Low => Mathf.CeilToInt(baseline * 0.5f),
        DayEnergyMode.High => Mathf.CeilToInt(baseline * 1.5f),
        _ => baseline
    };

    public static string EffortLabel(int effort) => Mathf.Clamp(effort, MinEffort, MaxEffort) switch
    {
        1 => "Very light",
        2 => "Light",
        3 => "Medium",
        4 => "Heavy",
        _ => "Very heavy"
    };

    public static int ClosestPreset(int spoons)
    {
        int best = BaselinePresets[0];
        foreach (int preset in BaselinePresets)
        {
            if (Mathf.Abs(preset - spoons) < Mathf.Abs(best - spoons))
                best = preset;
        }
        return best;
    }

    public static void SetBaseline(int spoons)
    {
        if (!IsReady) return;

        User.energyLimit = Mathf.Max(1, spoons);
        Save();
        OnBudgetChanged?.Invoke();
        OnEnergyChanged?.Invoke();
    }

    public static void SetTodayMode(DayEnergyMode mode)
    {
        if (!IsReady) return;

        EnsureToday();
        if (User.todayEnergyMode == (int)mode) return;

        User.todayEnergyMode = (int)mode;
        Save();
        OnBudgetChanged?.Invoke();
        OnEnergyChanged?.Invoke();
    }

    // Beim Abhaken Löffel abziehen, beim Zurücknehmen am selben Tag wieder gutschreiben
    public static void RegisterCompletion(Task task, bool completed)
    {
        if (!IsReady || task == null) return;

        EnsureToday();
        if (completed)
        {
            task.completedDate = Today;
            User.energySpentToday += task.estimatedEffort;
        }
        else
        {
            if (task.completedDate == Today)
                User.energySpentToday = Mathf.Max(0, User.energySpentToday - task.estimatedEffort);
            task.completedDate = "";
        }

        Save();
        OnEnergyChanged?.Invoke();
    }

    // Wird der Aufwand einer heute erledigten Aufgabe geändert, muss der Verbrauch mitziehen
    public static void RegisterEffortChange(Task task, int oldEffort)
    {
        if (!IsReady || task == null || !task.isDone || task.completedDate != Today) return;

        EnsureToday();
        User.energySpentToday = Mathf.Max(0, User.energySpentToday - oldEffort + task.estimatedEffort);
        Save();
        OnEnergyChanged?.Invoke();
    }

    // Einmalige Umstellung alter Daten (Limit in Minuten, Aufwand 1-10) auf Löffel
    public static bool MigrateIfNeeded(AppData data)
    {
        if (data.energyModelVersion >= CurrentModelVersion) return false;

        int legacy = PlayerPrefs.HasKey(LegacyPrefsKey)
            ? PlayerPrefs.GetInt(LegacyPrefsKey) / StatusPointsPerSpoon
            : data.user.energyLimit;
        data.user.energyLimit = legacy > 0 ? ClosestPreset(legacy) : DefaultBaseline;
        PlayerPrefs.DeleteKey(LegacyPrefsKey);

        foreach (Task task in data.tasks)
            task.estimatedEffort = Mathf.Clamp(Mathf.CeilToInt(task.estimatedEffort / 2f), MinEffort, MaxEffort);

        data.energyModelVersion = CurrentModelVersion;
        Debug.Log($"[EnergyBudget] Daten auf Löffel umgestellt. Basiswert: {data.user.energyLimit} Löffel.");
        return true;
    }

    // Neuer Tag: Verbrauch zurücksetzen und wieder mit "Normal" starten
    private static void EnsureToday()
    {
        if (User.energyDate == Today) return;

        User.energyDate = Today;
        User.energySpentToday = 0;
        User.todayEnergyMode = (int)DayEnergyMode.Normal;
        Save();
    }

    private static void Save() => DataManager.Instance.SaveData();
}
