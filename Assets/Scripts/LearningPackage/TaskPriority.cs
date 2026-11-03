using System;
using System.Globalization;

// Priorisierung für Lernpakete: Deadline zuerst, danach 80/20-Wichtigkeit (Pareto-Prinzip)
public static class TaskPriority
{
    // Puffer für einen Rest- und einen Low-Tag, klar unter dem Wochenrhythmus von Übungen
    public const int UrgentDays = 3;

    public const int TierUrgent = 0;
    public const int TierImportant = 1;
    public const int TierRest = 2;

    public static int Tier(Task task)
    {
        return Tier(task, DateTime.Today);
    }

    // today als Parameter, damit sich die Stufen in Unit-Tests mit festem Datum prüfen lassen
    public static int Tier(Task task, DateTime today)
    {
        long ticks = DueTicks(task);
        // Ohne gültiges Datum nicht künstlich dringend machen
        if (ticks > 0 && ticks < today.AddDays(UrgentDays + 1).Ticks) return TierUrgent;
        return task.isImportant ? TierImportant : TierRest;
    }

    // Ältere Tasks haben teils keine dueDateTicks gespeichert, dann aus dem Text lesen
    public static long DueTicks(Task task)
    {
        return task.dueDateTicks > 0 ? task.dueDateTicks : ParseDueTicks(task.dueDateString);
    }

    public static long ParseDueTicks(string dueDateString)
    {
        return DateTime.TryParse(dueDateString, new CultureInfo("de-DE"), DateTimeStyles.None, out DateTime due)
            ? due.Ticks
            : 0;
    }
}
