using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FoodEatenListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public FoodEatenEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public FoodEatenEventClass Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnFoodEaten(GameObject food, bool vertical)
    {
        Response.Invoke(food, vertical);
    }
}
