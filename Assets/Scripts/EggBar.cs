using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class controls the meter in the GUI that shows how many eggs the player
/// has collected.
/// </summary>
public class EggBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxEggsNeeded(int eggsNeeded)
    {
        slider.maxValue = eggsNeeded;
    }

    public void SetEgg(int eggsCollected)
    {
        slider.value = eggsCollected;
    }
}
