using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class MatchedEventClass : UnityEngine.Events.UnityEvent<List<GameObject>, bool> { }

[CreateAssetMenu(fileName = "MatchedEvent", menuName = "Match3/MatchedEvent", order = 2)]
public class MatchedEvent : ScriptableObject
{
    private List<MatchedEventListener> listeners = new List<MatchedEventListener>(); // 3

    public void Raise(List<GameObject> matchedItems, bool vertical) // 4
    {
        for (int i = listeners.Count - 1; i >= 0; i--) // 5
        {
            listeners[i].Matched(matchedItems, vertical); // 6
        }
    }

    public void RegisterListener(MatchedEventListener listener) // 7
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(MatchedEventListener listener) // 8
    {
        listeners.Remove(listener);
    }
}
