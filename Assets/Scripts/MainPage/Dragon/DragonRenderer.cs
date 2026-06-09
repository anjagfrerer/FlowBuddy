using System;
using UnityEngine;
using UnityEngine.UI;
public class DragonRenderer : MonoBehaviour
{
    [Header("Scriptable Objects for Mood")]
    [SerializeField] private DragonMoodSprites happy;
    [SerializeField] private DragonMoodSprites okay;
    [SerializeField] private DragonMoodSprites sad;

    [Header("DragonBodyParts")]
    [SerializeField] Image dragonEyes;
    [SerializeField] Image dragonSnout;
    [SerializeField] Image dragonWings;
    [SerializeField] Image dragonArms;

    public static Action<Mood> OnChangeMood;
    public static Action OnPackageComplete;

    void OnEnable()
    {
        OnChangeMood += SetMood;
    }

    void OnDisable()
    {
        OnChangeMood -= SetMood;
    }

    void SetMood(Mood mood)
    {
        DragonMoodSprites set = mood switch
        {
            Mood.Happy => happy,
            Mood.Okay => okay,
            Mood.Sad => sad,
            _ => okay
        };

        dragonEyes.sprite = set.eyes;
        dragonSnout.sprite = set.snout;
        dragonWings.sprite = set.wings;
    }

}

