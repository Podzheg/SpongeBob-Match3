using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MatchedEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public MatchedEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public MatchedEventClass Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void Matched(List<GameObject> matchedItems, bool vertical)
    {
        Response.Invoke(matchedItems, vertical);
    }
}

