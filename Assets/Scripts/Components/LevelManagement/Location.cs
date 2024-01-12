using Components.Audio;
using Creatures.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private AudioClip _backgroundMusic;

    public string Name => _name;

    public static Location Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        Enemies.CleanEnemiesList();
    }

    private void Start()
    {
        if (_backgroundMusic != null)
            AudioHandler.Instance.PlayMusic(_backgroundMusic);
    }
}
