using UnityEngine;
using Creatures.Player;
using System.Collections.Generic;

public class TraceController : MonoBehaviour
{
    [SerializeField] private GameObject _tracePrefab;
    [SerializeField] private float _traceLifetimeSecond;

    private Dictionary<Vector2, Trace> _traces = new();

    public float TraceLifeTime => _traceLifetimeSecond;

    public static TraceController Instance { get; private set; }

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;

        Player.Instance.OnPlayerGrounded += TraceController_OnPlayerGrounded;
        Trace.OnTraceDestroy += TraceController_OnTraceDestroy;
    }

    private void TraceController_OnPlayerGrounded(object sender, Player.OnPlayerGroundedEventArgs e)
    {
        Vector2 tracePosition = GetTracePosition(e.position);

        if(_traces.ContainsKey(tracePosition))
        {
            _traces[tracePosition].UpdateTimer();
        }
        else
        {
            Trace trace = Instantiate(_tracePrefab, new Vector3(tracePosition.x, tracePosition.y),
                                      Quaternion.identity, transform).GetComponent<Trace>();
            _traces[tracePosition] = trace;
        }
    }

    private Vector2 GetTracePosition(Vector2 position)
    {
        float xPos = Mathf.Floor(position.x) + 0.5f;
        float yPos = Mathf.Floor(position.y) - _tracePrefab.transform.localScale.y / 2;
        return new Vector2(xPos, yPos);
    }

    private void TraceController_OnTraceDestroy(object sender, Trace.OnTraceDestroyEventArgs e)
    {
        _traces.Remove(e.position);
    }
}
