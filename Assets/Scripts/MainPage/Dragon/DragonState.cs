using UnityEngine;

public enum Mood
{
    Happy,
    Okay,
    Sad
}
public class DragonState : MonoBehaviour
{
    [SerializeField] private Mood currentMood;

    void SetMood(Mood mood)
    {
        currentMood = mood;
    }

}
