using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionSelectionController
{
    public ECharacterPersonality personality { get; private set; }
    public string goal
    {
        get
        {
            return m_goal;
        }
        set
        {
            if (value != m_goal)
            {
                m_goal = value;
                GoalChanged?.Invoke();
            }
        }
    }
    public event Action GoalChanged;

    private string m_goal = "start";

    public void UpdatePersonality(ECharacterPersonality personality)
    {
        this.personality = personality;
        EvaluateNewGoal();
    }

    public void CheckGoalCompletion(string state)
    {
        // have we achieved the goal?

        // if yes, update with new goal
        GoalChanged?.Invoke();
    }

    public virtual void UpdateGoalValue(string goal, string val)
    {
        EvaluateNewGoal();
    }

    protected abstract void EvaluateNewGoal();
}
