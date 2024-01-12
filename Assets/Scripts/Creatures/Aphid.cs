using Components.UI;
using Creatures.Player;
using DragonBones;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aphid : MonoBehaviour
{
    [SerializeField] private GameObject _interaction;

    private UnityArmatureComponent armature;

    private const string IDLE_KEY = "Aphid_base";
    private const string ATTENTION_KEY = "Aphid_attention";

    private void Start()
    {
        armature = GetComponent<UnityArmatureComponent>();
        SetIdleAnim();
    }

    public void SetIdleAnim()
    {
        armature.animation.Play(IDLE_KEY, 0);
        _interaction.SetActive(false);
    }

    public void SetAttentionAnim()
    {
        armature.animation.Play(ATTENTION_KEY, 0);
        _interaction.SetActive(true);
    }

    public void SavePosition()
    {
        PlayerPrefsController.SavePlayerData();

        HUD.Instance.SendMessage("Сохранение...", 3f);
    }
}
