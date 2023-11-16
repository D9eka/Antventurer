using Creatures;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Creatures.Player.Player;

public class Trace : MonoBehaviour
{
    public static EventHandler<OnTraceDestroyEventArgs> OnTraceDestroy;
    public class OnTraceDestroyEventArgs : EventArgs
    {
        public Vector2 position;
    }

    private float _timer;

    private void Start()
    {
        _timer = TraceController.Instance.TraceLifeTime;
    }

    private void FixedUpdate()
    {
        _timer -= Time.deltaTime;
        if(_timer < 0)
        {
            OnTraceDestroy?.Invoke(this, new OnTraceDestroyEventArgs
            {
                position = transform.position
            });
            Destroy(gameObject);
        }
    }

    public void UpdateTimer()
    {
        _timer = TraceController.Instance.TraceLifeTime;
    }
}
