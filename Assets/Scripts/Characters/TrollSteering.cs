using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollSteering : SteeringController
{
    public event Action<string, float> GoToGoal;

    public void OnFindShelter()
    {
        Debug.Log("[TrollSteering] OnFindShelter");
    }

    public void OnFindSleepingArea()
    {
        Debug.Log("[TrollSteering] OnFindSleepingArea");

    }

    public void OnPatrolArea()
    {
        Debug.Log("[TrollSteering] OnPatrolArea");

    }

    public void OnSearchForFood()
    {
        Debug.Log("[TrollSteering] OnSearchForFood");
        // do some stuff to determine where the food is
        // navigate to spot food should be at
        GoToGoal?.Invoke("food", 1.0f);

    }
}
