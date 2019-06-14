using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class FoodEatenEventClass : UnityEngine.Events.UnityEvent<GameObject, bool> { }

[CreateAssetMenu(fileName = "FoodEatenEvent", menuName = "Match3/FoodEatenEvent", order = 1)]
public class FoodEatenEvent : ScriptableObject
{
    private List<FoodEatenListener> listeners = new List<FoodEatenListener>(); // 3

    public void Raise(GameObject food, bool vertical) // 4
    {
        for (int i = listeners.Count - 1; i >= 0; i--) // 5
        {
            listeners[i].OnFoodEaten(food, vertical); // 6
        }
    }

    public void RegisterListener(FoodEatenListener listener) // 7
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(FoodEatenListener listener) // 8
    {
        listeners.Remove(listener);
    }
}
