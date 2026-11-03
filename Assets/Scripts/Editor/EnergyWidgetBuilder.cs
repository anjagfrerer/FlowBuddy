using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Legt das Energie-Widget fertig verdrahtet an:
/// UI-Objekt im Canvas auswählen > Rechtsklick > FlowBuddy > Energy Widget.
/// </summary>
public static class EnergyWidgetBuilder
{
    private static readonly string[] ModeLabels = { "Rest", "Low", "Normal", "High" };

    // Wie "Satisfaction:" auf der MainPage
    private const float LabelSize = 48f;
    private const float TextSize = 36f;
    private const float HintSize = 48f; // Fließtextgröße, kleiner wäre unter 16 sp
    private const float CharacterSpacing = -5f;
    private const float Spacing = 8f;
    private const float BarHeight = 24f;
    // Sichtbar schmaler Button, die Touch-Fläche bleibt über Raycast Padding
    // bei mind. 48x48 dp (Android Accessibility Guidelines)
    private const float ButtonHeight = 72f;
    private const float TouchPadding = 36f;
    private const float SettingsButtonSpace = 120f;

    private static readonly Color TextColor = new Color(0.11372549f, 0.12941177f, 0.14509805f);
    private static readonly Color BarBackground = new Color(0.85f, 0.85f, 0.85f);
    private static readonly Color BarColor = new Color(0.43f, 0.73f, 0.73f);

    [MenuItem("GameObject/FlowBuddy/Energy Widget", false, 10)]
    private static void Create(MenuCommand command)
    {
        GameObject parent = command.context as GameObject;
        if (parent == null) parent = Selection.activeGameObject;

        if (parent == null || parent.GetComponentInParent<Canvas>() == null)
        {
            EditorUtility.DisplayDialog("Energy Widget", "Bitte zuerst ein UI-Objekt innerhalb eines Canvas auswählen.", "OK");
            return;
        }

        TMP_FontAsset font = FindProjectFont(parent);

        GameObject root = NewUI("EnergyWidget", parent.transform);
        RectTransform rootRect = root.GetComponent<RectTransform>();
        rootRect.anchorMin = new Vector2(0f, 1f);
        rootRect.anchorMax = new Vector2(1f, 1f);
        rootRect.pivot = new Vector2(0.5f, 1f);
        rootRect.sizeDelta = new Vector2(-64f, 0f);
        rootRect.anchoredPosition = Vector2.zero;

        VerticalLayoutGroup layout = root.AddComponent<VerticalLayoutGroup>();
        layout.spacing = Spacing;
        // Platz unten, damit die vergrößerte Tippfläche nicht in die nächste Zeile ragt
        layout.padding = new RectOffset(0, 0, 0, (int)TouchPadding);
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;
        // In einer Layout Group bestimmt der Parent die Größe, sonst passt sich das Widget selbst an
        if (parent.GetComponent<LayoutGroup>() == null)
            root.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        else
        {
            // Flexible 0: Widget nimmt nur so viel Höhe wie nötig, der Rest bleibt für Liste und Drache
            LayoutElement rootElement = root.AddComponent<LayoutElement>();
            rootElement.minHeight = 200f;
            rootElement.flexibleHeight = 0f;
        }

        // Zahl und Balken in einer Zeile, damit das Widget flach bleibt
        GameObject statusRow = NewUI("StatusRow", root.transform);
        HorizontalLayoutGroup statusLayout = statusRow.AddComponent<HorizontalLayoutGroup>();
        statusLayout.spacing = 16f;
        // Rechts frei lassen, dort sitzt auf der MainPage der Settings-Button
        statusLayout.padding = new RectOffset(0, (int)SettingsButtonSpace, 0, 0);
        statusLayout.childAlignment = TextAnchor.MiddleLeft;
        statusLayout.childControlWidth = true;
        statusLayout.childControlHeight = true;
        statusLayout.childForceExpandWidth = false;
        statusLayout.childForceExpandHeight = false;

        TMP_Text energyText = NewText("EnergyText", statusRow.transform, "Energy: 12 / 12", LabelSize, font, FontStyles.Normal);
        energyText.alignment = TextAlignmentOptions.Left;
        energyText.textWrappingMode = TextWrappingModes.NoWrap;

        GameObject bar = NewUI("Bar", statusRow.transform);
        Image barImage = bar.AddComponent<Image>();
        barImage.color = BarBackground;
        barImage.raycastTarget = false;
        LayoutElement barElement = bar.AddComponent<LayoutElement>();
        barElement.minHeight = BarHeight;
        barElement.preferredHeight = BarHeight;
        barElement.flexibleWidth = 1f;

        GameObject fill = NewUI("Fill", bar.transform);
        Image fillImage = fill.AddComponent<Image>();
        fillImage.color = BarColor;
        fillImage.raycastTarget = false;
        RectTransform fillRect = fill.GetComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;

        TMP_Text hintText = NewText("HintText", root.transform, "How much energy do you have today?", HintSize, font, FontStyles.Normal);
        hintText.alignment = TextAlignmentOptions.Left;

        GameObject row = NewUI("ModeButtons", root.transform);
        HorizontalLayoutGroup rowLayout = row.AddComponent<HorizontalLayoutGroup>();
        rowLayout.spacing = 12f;
        rowLayout.childControlWidth = true;
        rowLayout.childControlHeight = true;
        rowLayout.childForceExpandWidth = true;
        rowLayout.childForceExpandHeight = true;
        // Oben so viel Luft wie unten (TouchPadding), damit die Reihe vertikal mittig wirkt
        rowLayout.padding = new RectOffset(0, 0, (int)(TouchPadding - Spacing), 0);
        LayoutElement rowElement = row.AddComponent<LayoutElement>();
        rowElement.minHeight = ButtonHeight + TouchPadding - Spacing;
        rowElement.preferredHeight = ButtonHeight + TouchPadding - Spacing;
        rowElement.flexibleHeight = 0f;

        Button[] buttons = new Button[ModeLabels.Length];
        for (int i = 0; i < ModeLabels.Length; i++)
        {
            GameObject buttonObject = NewUI(ModeLabels[i] + "Button", row.transform);
            Image buttonImage = buttonObject.AddComponent<Image>();
            // Wie Generic Button: abgerundete Fläche, Farben setzt EnergyWidget je nach Auswahl
            buttonImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UIElements/RoundedButtonBackground.png");
            buttonImage.type = Image.Type.Sliced;
            // Sprite ist 192 px hoch und fast nur Ecke: so skalieren, dass die Rundung zur Button-Höhe passt
            buttonImage.pixelsPerUnitMultiplier = 192f / ButtonHeight;
            buttonImage.color = Color.white;
            // Negatives Padding vergrößert die Tippfläche nach oben und unten über den sichtbaren Button hinaus
            buttonImage.raycastPadding = new Vector4(0f, -TouchPadding, 0f, -TouchPadding);
            buttons[i] = buttonObject.AddComponent<Button>();

            TMP_Text label = NewText("Label", buttonObject.transform, ModeLabels[i], TextSize, font, FontStyles.Normal);
            RectTransform labelRect = label.rectTransform;
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;
        }

        EnergyWidget widget = root.AddComponent<EnergyWidget>();
        SerializedObject serializedWidget = new SerializedObject(widget);
        serializedWidget.FindProperty("barFill").objectReferenceValue = fillRect;
        serializedWidget.FindProperty("energyText").objectReferenceValue = energyText;
        serializedWidget.FindProperty("hintText").objectReferenceValue = hintText;
        SerializedProperty buttonArray = serializedWidget.FindProperty("modeButtons");
        buttonArray.arraySize = buttons.Length;
        for (int i = 0; i < buttons.Length; i++)
            buttonArray.GetArrayElementAtIndex(i).objectReferenceValue = buttons[i];
        serializedWidget.ApplyModifiedPropertiesWithoutUndo();

        Undo.RegisterCreatedObjectUndo(root, "Create Energy Widget");
        Selection.activeGameObject = root;
    }

    private static GameObject NewUI(string name, Transform parent)
    {
        GameObject go = new GameObject(name, typeof(RectTransform));
        go.layer = LayerMask.NameToLayer("UI");
        go.transform.SetParent(parent, false);
        return go;
    }

    private static TMP_Text NewText(string name, Transform parent, string text, float size, TMP_FontAsset font, FontStyles style)
    {
        GameObject go = NewUI(name, parent);
        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
        if (font != null) tmp.font = font;
        tmp.text = text;
        tmp.fontSize = size;
        tmp.fontStyle = style;
        tmp.characterSpacing = CharacterSpacing;
        tmp.color = TextColor;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.raycastTarget = false;

        // Text wächst mit der Schriftgrößen-Einstellung (Accessibility)
        DynamicTextFontScaler scaler = go.AddComponent<DynamicTextFontScaler>();
        SerializedObject serializedScaler = new SerializedObject(scaler);
        serializedScaler.FindProperty("originalFontSize").floatValue = size;
        serializedScaler.ApplyModifiedPropertiesWithoutUndo();

        return tmp;
    }

    // Gleiche Schrift wie der Rest der Seite, sonst die TMP-Standardschrift
    private static TMP_FontAsset FindProjectFont(GameObject context)
    {
        TMP_Text existing = context.GetComponentInParent<Canvas>().GetComponentInChildren<TMP_Text>(true);
        return existing != null ? existing.font : TMP_Settings.defaultFontAsset;
    }
}
