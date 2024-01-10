using Components.Audio;
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
    }

    private void Start()
    {
        if (_backgroundMusic != null)
            AudioHandler.Instance.PlayMusic(_backgroundMusic);
    }
}
