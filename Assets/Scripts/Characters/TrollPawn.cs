using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollPawn : SimplePawn
{
    private void Awake()
    {
        actionSelection = new WandererActionSelection();
        steering = new TrollSteering();
        locomotion = gameObject.AddComponent<LargeEntityLocomotion>();

        ((WandererActionSelection)actionSelection).FindShelter += ((TrollSteering)steering).OnFindShelter;
        ((WandererActionSelection)actionSelection).FindSleepingArea += ((TrollSteering)steering).OnFindSleepingArea;
        ((WandererActionSelection)actionSelection).PatrolArea += ((TrollSteering)steering).OnPatrolArea;
        ((WandererActionSelection)actionSelection).SearchForFood += ((TrollSteering)steering).OnSearchForFood;

        ((TrollSteering)steering).GoToGoal += ((LargeEntityLocomotion)locomotion).MoveToGoal;

    }

    private void Start()
    {
        StartCoroutine(Test());
    }

    private IEnumerator Test()
    {
        yield return new WaitForSeconds(0.5f);
        actionSelection.UpdateGoalValue("gameReady", "true");
    }
}
