using UnityEngine;
using Creatures.Player;
using System.Collections.Generic;

public class TraceController : MonoBehaviour
{
    [SerializeField] private GameObject _tracePrefab;
    [SerializeField] private float _traceLifetimeSecond;
    [SerializeField] private float _traceOffset;

    private Dictionary<(float, float), Trace> _traces = new();

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

        if(_traces.ContainsKey((tracePosition.x, tracePosition.y)))
        {
            _traces[(tracePosition.x, tracePosition.y)].UpdateTimer();
        }
        else
        {
            InstantiateTrace(tracePosition);
        }
    }

    private Vector2 GetTracePosition(Vector2 position)
    {
        float xPos = Mathf.Floor(position.x) + _traceOffset;
        float yPos = Mathf.Floor(position.y) - _tracePrefab.transform.localScale.y / 2;
        return new Vector2(xPos, yPos);
    }

    private void InstantiateTrace(Vector2 tracePosition)
    {
        float traceStart = tracePosition.x;
        while (_traces.ContainsKey((traceStart - 1, tracePosition.y)))
            traceStart--;

        float traceEnd = tracePosition.x;
        while (_traces.ContainsKey((traceEnd + 1, tracePosition.y)))
            traceEnd++;

        InstantiateTrace(tracePosition, traceStart, traceEnd);
    }

    private void InstantiateTrace(Vector2 tracePosition, float traceStart, float traceEnd)
    {
        var newTrace = Instantiate(_tracePrefab, transform);
        newTrace.transform.localScale = new Vector2(traceEnd - traceStart + 1, newTrace.transform.localScale.y);
        newTrace.transform.position = new Vector2(traceEnd - newTrace.transform.localScale.x / 2.0f + _traceOffset, tracePosition.y);

        HashSet<GameObject> tracesToDelete = new();

        for (float i = traceStart; i <= traceEnd; i++)
        {
            (float, float) tracePos = (i, tracePosition.y);
            float traceLifeTime = _traceLifetimeSecond;
            if (_traces.ContainsKey(tracePos))
            {
                tracesToDelete.Add(_traces[tracePos].gameObject);
                traceLifeTime = _traces[tracePos].TraceLifetime;
            }
            Trace traceComponent = newTrace.AddComponent<Trace>();
            traceComponent.Initialize(new Vector2(tracePos.Item1, tracePos.Item2), traceLifeTime);
            _traces[tracePos] = traceComponent;
        }

        foreach (var trace in tracesToDelete)
            Destroy(trace.gameObject);
    }

    private void TraceController_OnTraceDestroy(object sender, Trace.OnTraceDestroyEventArgs e)
    {
        GameObject trace = _traces[(e.position.x, e.position.y)].gameObject;
        float traceStart = trace.transform.position.x - trace.transform.localScale.x / 2.0f + _traceOffset;
        float traceEnd = trace.transform.position.x + trace.transform.localScale.x / 2.0f - _traceOffset;

        float traceToDeletePos = e.position.x;

        Destroy(_traces[(traceToDeletePos, trace.transform.position.y)].gameObject);
        _traces.Remove((traceToDeletePos, trace.transform.position.y));
        if(traceStart == traceToDeletePos && traceEnd == traceToDeletePos)
        {
            return;
        }
        if (traceStart == traceToDeletePos)
        {
            InstantiateTrace(trace.transform.position, traceStart + 1, traceEnd);
        }
        else if (traceEnd == traceToDeletePos)
        {
            InstantiateTrace(trace.transform.position, traceStart, traceEnd - 1);
        }
        else
        {
            InstantiateTrace(trace.transform.position, traceStart, traceToDeletePos - 1);
            InstantiateTrace(trace.transform.position, traceToDeletePos + 1, traceEnd);
        }
    }
}
