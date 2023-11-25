using System;
using UnityEngine;

public class Trace : MonoBehaviour
{
    public static EventHandler<OnTraceDestroyEventArgs> OnTraceDestroy;
    public class OnTraceDestroyEventArgs : EventArgs
    {
        public Vector2 position;
    }

    private Vector2 _position;
    private float _timer;

    private bool _isInitialized;

    public float TraceLifetime => _timer;

    public void Initialize(Vector2 position, float traceLifeTime)
    {
        _position = position;
        _timer = traceLifeTime;

        _isInitialized = true;
    }

    private void FixedUpdate()
    {
        if (!_isInitialized)
            return;

        _timer -= Time.deltaTime;
        if(_timer < 0)
        {
            OnTraceDestroy?.Invoke(this, new OnTraceDestroyEventArgs
            {
                position = _position
            });
            Destroy(gameObject);
        }
    }

    public void UpdateTimer()
    {
        _timer = TraceController.Instance.TraceLifeTime;
    }
}
