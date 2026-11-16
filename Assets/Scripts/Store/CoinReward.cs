using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

/// <summary>
/// Münzformel (S3_1.2) und Gutschrift beim Abschließen eines Lernpakets (S3_1.3).
/// Zu späte Aufgaben bekommen nur keinen Bonus, es gibt keinen Abzug (kein Druck).
/// </summary>
public static class CoinReward
{
    public const int CoinsPerSpoon = 5;
    // Bonus in Prozent des Grundwerts
    public const int BonusEarlyPercent = 50;  // vor dem Deadline-Tag
    public const int BonusOnTimePercent = 25; // am Deadline-Tag

    public static int BaseCoins(Task task) =>
        Mathf.Clamp(task.estimatedEffort, EnergyBudget.MinEffort, EnergyBudget.MaxEffort) * CoinsPerSpoon;

    public static int BonusCoins(Task task, DateTime completedAt)
    {
        long dueTicks = TaskPriority.DueTicks(task);
        if (dueTicks <= 0) return 0;

        int daysLeft = (int)(new DateTime(dueTicks).Date - completedAt.Date).TotalDays;
        if (daysLeft < 0) return 0;

        int percent = daysLeft > 0 ? BonusEarlyPercent : BonusOnTimePercent;
        return Mathf.RoundToInt(BaseCoins(task) * percent / 100f);
    }

    public static int Calculate(Task task, DateTime completedAt) =>
        BaseCoins(task) + BonusCoins(task, completedAt);

    // Maßgeblich ist der Tag des Abhakens, nicht der des Paket-Abschlusses
    private static DateTime CompletedAt(Task task) =>
        DateTime.TryParseExact(task.completedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out DateTime date) ? date : DateTime.Now;

    // Nur erledigte Aufgaben zählen
    public static int PackageTotal(IEnumerable<Task> package) =>
        package.Where(t => t.isDone).Sum(t => Calculate(t, CompletedAt(t)));

    // Schreibt die Münzen des Pakets gut und speichert (der Münzstand liegt in User, JSON)
    public static int AwardPackage(IEnumerable<Task> package)
    {
        int coins = PackageTotal(package);
        if (coins <= 0) return 0;

        DataManager.Instance.appData.user.coins += coins;
        DataManager.Instance.SaveData();
        return coins;
    }
}
