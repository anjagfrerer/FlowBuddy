using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Zeigt die Tagesenergie ("Energy: 7 / 12") und lässt den Nutzer mit einem Tipp angeben,
/// wie viel Energie er heute hat. Anlegen über GameObject > FlowBuddy > Energy Widget.
/// </summary>
public class EnergyWidget : MonoBehaviour
{
    [Header("Anzeige")]
    [SerializeField] private RectTransform barFill;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text hintText;

    [Header("Tagesauswahl (Reihenfolge: Rest, Low, Normal, High)")]
    [SerializeField] private Button[] modeButtons = new Button[4];
    [SerializeField] private Color selectedColor = new Color(0.23f, 0.56f, 0.35f);
    [SerializeField] private Color deselectedColor = new Color(0.85f, 0.85f, 0.85f);

    private void Awake()
    {
        // Nur die Buttons sollen Klicks abfangen, sonst blockieren Text und Balken
        // darunterliegende Elemente wie den Settings-Button
        foreach (Graphic graphic in GetComponentsInChildren<Graphic>(true))
        {
            if (graphic.GetComponent<Selectable>() == null)
                graphic.raycastTarget = false;
        }

        for (int i = 0; i < modeButtons.Length; i++)
        {
            int index = i;
            if (modeButtons[index] != null)
                modeButtons[index].onClick.AddListener(() => EnergyBudget.SetTodayMode((DayEnergyMode)index));
        }
    }

    private void OnEnable()
    {
        EnergyBudget.OnEnergyChanged += Redraw;
        Redraw();
    }

    private void OnDisable()
    {
        EnergyBudget.OnEnergyChanged -= Redraw;
    }

    // Falls der DataManager in OnEnable noch nicht bereit war
    private void Start()
    {
        Redraw();
    }

    private void Redraw()
    {
        if (!EnergyBudget.IsReady) return;

        int budget = EnergyBudget.TodayBudget;
        int remaining = EnergyBudget.Remaining;
        DayEnergyMode mode = EnergyBudget.TodayMode;

        if (energyText != null)
            energyText.text = $"Energy: {remaining} / {budget}";

        if (barFill != null)
            barFill.anchorMax = new Vector2(budget > 0 ? (float)remaining / budget : 0f, 1f);

        if (hintText != null)
            hintText.text = HintFor(mode, budget, remaining);

        for (int i = 0; i < modeButtons.Length; i++)
        {
            Button button = modeButtons[i];
            if (button == null) continue;

            bool isActive = i == (int)mode;
            ColorBlock cb = button.colors;
            cb.normalColor = isActive ? selectedColor : deselectedColor;
            cb.selectedColor = isActive ? selectedColor : deselectedColor;
            button.colors = cb;

            // Auswahl nicht nur über Farbe zeigen (Farbenblindheit)
            TMP_Text label = button.GetComponentInChildren<TMP_Text>();
            if (label != null)
                label.fontStyle = isActive ? FontStyles.Bold | FontStyles.Underline : FontStyles.Normal;
        }
    }

    private static string HintFor(DayEnergyMode mode, int budget, int remaining)
    {
        if (mode == DayEnergyMode.Rest)
            return "Rest day. No tasks today, your dragon is fine.";
        if (EnergyBudget.SpentToday > budget)
            return "You did more than planned today. Take it easy!";
        if (remaining == 0)
            return "Today's energy is used up. Time to rest!";
        return "How much energy do you have today?";
    }
}
