using Creatures.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningWindow : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(PlayerPrefsController.GetFirstStartState());
    }
}
