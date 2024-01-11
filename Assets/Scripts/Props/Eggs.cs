using Creatures.Player;
using DragonBones;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Props
{
    public class Eggs : MonoBehaviour
    {
        private const string BIRTH_KEY = "Birth";

        private void Start()
        {
            gameObject.SetActive(PlayerPrefsController.GetFirstStartState());
            UnityArmatureComponent armature = GetComponent<UnityArmatureComponent>();
            armature.animation.Play(BIRTH_KEY, 1);
            PlayerPrefsController.SetFirstStartState(false);
        }
    }
}
