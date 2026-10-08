using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Navigation_bar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    List<Navigation_bar_button> buttons = new();

    void Start()
    {
        foreach(var navButton in GetComponentsInChildren<Navigation_bar_button>())
        {
            buttons.Add(navButton);
            navButton.navigation_Bar = this;

            if(SceneChanger.currentScene == navButton.scene_to_navigate)
                navButton.Select(true);
            else
                navButton.Select(false);
        }
    }

    public void Select(Navigation_bar_button button)
    {
        buttons.ForEach(button => button.Select(false));
        button.Select(true);
        SceneChanger.Load(button.scene_to_navigate);
    }
}
