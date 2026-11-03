using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Legt den "Important"-Schalter (80/20-Prinzip) fertig verdrahtet an:
/// Im Configure-Task-Panel ein UI-Objekt auswählen > Rechtsklick > FlowBuddy > Important Toggle.
/// </summary>
public static class ImportantToggleBuilder
{
    private const float LabelSize = 48f;
    private const float HintSize = 30f;
    private const float CharacterSpacing = -5f;
    // Ganze Zeile ist Tippfläche, mind. 48x48 dp (wie Energy Widget: 100 + 2 * 22)
    private const float RowHeight = 144f;
    private const float BoxSize = 72f;

    private static readonly Color TextColor = new Color(0.11372549f, 0.12941177f, 0.14509805f);
    private static readonly Color BoxColor = new Color(0.85f, 0.85f, 0.85f);
    private static readonly Color CheckColor = new Color(0.11372549f, 0.12941177f, 0.14509805f);

    [MenuItem("GameObject/FlowBuddy/Important Toggle", false, 11)]
    private static void Create(MenuCommand command)
    {
        GameObject parent = command.context as GameObject;
        if (parent == null) parent = Selection.activeGameObject;

        if (parent == null || parent.GetComponentInParent<Canvas>() == null)
        {
            EditorUtility.DisplayDialog("Important Toggle", "Bitte zuerst ein UI-Objekt innerhalb eines Canvas auswählen.", "OK");
            return;
        }

        TMP_FontAsset font = FindProjectFont(parent);

        GameObject root = NewUI("ImportantToggle", parent.transform);
        RectTransform rootRect = root.GetComponent<RectTransform>();
        rootRect.anchorMin = new Vector2(0f, 1f);
        rootRect.anchorMax = new Vector2(1f, 1f);
        rootRect.pivot = new Vector2(0.5f, 1f);
        rootRect.sizeDelta = new Vector2(-64f, RowHeight);
        rootRect.anchoredPosition = Vector2.zero;

        // Unsichtbarer Hintergrund, damit auch ein Tipp auf den Text den Schalter umlegt
        Image hitArea = root.AddComponent<Image>();
        hitArea.color = Color.clear;

        LayoutElement rootElement = root.AddComponent<LayoutElement>();
        rootElement.minHeight = RowHeight;
        rootElement.preferredHeight = RowHeight;
        rootElement.flexibleHeight = 0f;

        HorizontalLayoutGroup layout = root.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = 24f;
        layout.childAlignment = TextAnchor.MiddleLeft;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;

        GameObject box = NewUI("Box", root.transform);
        Image boxImage = box.AddComponent<Image>();
        boxImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        boxImage.type = Image.Type.Sliced;
        boxImage.color = BoxColor;
        LayoutElement boxElement = box.AddComponent<LayoutElement>();
        boxElement.minWidth = BoxSize;
        boxElement.minHeight = BoxSize;
        boxElement.preferredWidth = BoxSize;
        boxElement.preferredHeight = BoxSize;

        GameObject check = NewUI("Checkmark", box.transform);
        Image checkImage = check.AddComponent<Image>();
        checkImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Checkmark.psd");
        checkImage.color = CheckColor;
        checkImage.raycastTarget = false;
        RectTransform checkRect = check.GetComponent<RectTransform>();
        checkRect.anchorMin = Vector2.zero;
        checkRect.anchorMax = Vector2.one;
        checkRect.offsetMin = new Vector2(8f, 8f);
        checkRect.offsetMax = new Vector2(-8f, -8f);

        GameObject texts = NewUI("Texts", root.transform);
        VerticalLayoutGroup textLayout = texts.AddComponent<VerticalLayoutGroup>();
        textLayout.childAlignment = TextAnchor.MiddleLeft;
        textLayout.childControlWidth = true;
        textLayout.childControlHeight = true;
        textLayout.childForceExpandWidth = true;
        textLayout.childForceExpandHeight = false;
        texts.AddComponent<LayoutElement>().flexibleWidth = 1f;

        NewText("Label", texts.transform, "Important", LabelSize, font);
        // Kurz erklären, was der Schalter bewirkt (einfache Sprache)
        NewText("Hint", texts.transform, "Goes first when nothing is due soon.", HintSize, font);

        Toggle toggle = root.AddComponent<Toggle>();
        toggle.targetGraphic = boxImage;
        toggle.graphic = checkImage;
        // Kein Ein-/Ausblenden, Reizreduktion
        toggle.toggleTransition = Toggle.ToggleTransition.None;
        toggle.isOn = false;

        ConfigureTask configureTask = parent.GetComponentInParent<ConfigureTask>(true);
        if (configureTask == null) configureTask = Object.FindAnyObjectByType<ConfigureTask>(FindObjectsInactive.Include);
        if (configureTask != null)
        {
            SerializedObject serializedConfigure = new SerializedObject(configureTask);
            serializedConfigure.FindProperty("importantToggle").objectReferenceValue = toggle;
            serializedConfigure.ApplyModifiedProperties();
        }
        else
        {
            Debug.LogWarning("ImportantToggle: Kein ConfigureTask in der Szene gefunden, bitte 'Important Toggle' im Inspector selbst zuweisen.");
        }

        Undo.RegisterCreatedObjectUndo(root, "Create Important Toggle");
        Selection.activeGameObject = root;
    }

    private static GameObject NewUI(string name, Transform parent)
    {
        GameObject go = new GameObject(name, typeof(RectTransform));
        go.layer = LayerMask.NameToLayer("UI");
        go.transform.SetParent(parent, false);
        return go;
    }

    private static TMP_Text NewText(string name, Transform parent, string text, float size, TMP_FontAsset font)
    {
        GameObject go = NewUI(name, parent);
        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
        if (font != null) tmp.font = font;
        tmp.text = text;
        tmp.fontSize = size;
        tmp.characterSpacing = CharacterSpacing;
        tmp.color = TextColor;
        tmp.alignment = TextAlignmentOptions.Left;
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
