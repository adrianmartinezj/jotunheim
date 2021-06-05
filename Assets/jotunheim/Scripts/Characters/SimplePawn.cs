using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SimplePawn : MonoBehaviour
{
    public ActionSelectionController actionSelection;
    public SteeringController steering;
    public LocomotionController locomotion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        //steering.CheckSteering();
        //locomotion.MoveToGoal();
    }
}
