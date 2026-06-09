using System;
using UnityEngine;
using UnityEngine.UI;
public class UserEnergyEditor : MonoBehaviour
{
    [SerializeField] InputField energyInputField;
    [SerializeField] UserDataManager userDataManager;

    void Start()
    {
        energyInputField.text = userDataManager.GetEnergyLevel().ToString();
    }

    public void setEnergyValue()
    {
        if(!int.TryParse(energyInputField.text,out int i))
            return;
            
        userDataManager.SetEnergyLevel(i);
    }

}