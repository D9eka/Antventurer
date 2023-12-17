using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    [SerializeField] private string _name;

    public string Name => _name;

    public static Location Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
