using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandererActionSelection : ActionSelectionController
{
    private Dictionary<string, string> wandererGoalMap = new Dictionary<string, string>
    {
        { "start", "food" },
        { "food", "shelter" },
        { "shelter", "safety" },
        { "safety", "sleep" },
        { "sleep", "food" }
    };
    private Dictionary<string, string> wandererGoalEval = new Dictionary<string, string>
    {
        { "start", "gameReady" },
        { "food", "foodLevel" },
        { "shelter", "hasShelter" },
        { "safety", "hasPatrolled" },
        { "sleep", "hasSlept" }
    };
    private Dictionary<string, string> wandererGoalCriteria = new Dictionary<string, string>
    {
        { "gameReady", "true"},
        { "foodLevel", "100" },
        { "hasShelter", "false" },
        { "hasPatrolled", "true" },
        { "hasSlept", "true" }
    };
    private Dictionary<string, string> wandererGoalProgress = new Dictionary<string, string>
    {
        { "gameReady", "false"},
        { "foodLevel", "0" },
        { "hasShelter", "false" },
        { "hasPatrolled", "false" },
        { "hasSlept", "false" }
    };

    public WandererActionSelection()
    {
        UpdatePersonality(ECharacterPersonality.Wanderer);
    }

    public event Action SearchForFood;
    public event Action FindShelter;
    public event Action PatrolArea;
    public event Action FindSleepingArea;

    public override void UpdateGoalValue(string goal, string val)
    {
        Debug.Log("test: " + goal + " " + val);
        wandererGoalProgress[goal] = val;
        base.UpdateGoalValue(goal, val);
    }

    protected override void EvaluateNewGoal()
    {
        var currentGoal = wandererGoalEval[goal];
        Debug.Log("[WandererActionSelection] currentGoal: " + currentGoal);
        var currentGoalCriteria = wandererGoalCriteria[currentGoal];
        if (currentGoalCriteria == "true" || currentGoalCriteria == "false")
        {
            bool goalVal = bool.Parse(currentGoalCriteria), curVal = bool.Parse(wandererGoalProgress[currentGoal]);
            Debug.Log("[WandererActionSelection] " + currentGoal + ": " + curVal);

            // we're doing bool comparisons
            if (goalVal == curVal)
            {
                // work is all done on this goal
                goal = wandererGoalMap[goal];
                EvaluateNewGoal();
            }
            else
            {
                // work still needs to be done
                DoWorkOnGoal();
            }
        }
        else
        {
            float goalVal = float.Parse(currentGoalCriteria), curVal = float.Parse(wandererGoalProgress[currentGoal]); ;
            Debug.Log("[WandererActionSelection] " + currentGoal + ": " + curVal);
            // we're doing float comparisons
            if (goalVal == curVal)
            {
                // work is all done on this goal
                goal = wandererGoalMap[goal];
                EvaluateNewGoal();
            }
            else
            {
                // work still needs to be done
                DoWorkOnGoal();
            }

        }
    }

    private void DoWorkOnGoal()
    {
        switch (wandererGoalEval[goal])
        {
            case "gameReady":
                break;
            case "foodLevel":
                SearchForFood?.Invoke();
                break;
            case "hasShelter":
                FindShelter?.Invoke();
                break;
            case "hasPatrolled":
                PatrolArea?.Invoke();
                break;
            case "hasSlept":
                FindSleepingArea?.Invoke();
                break;
            default:
                break;
        }
    }
}
