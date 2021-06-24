using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EInteractableType
{
    Tap,
    RepeatedTap,
    TapAndHold
}

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
    [Header("Camera Options")]
    [SerializeField]
    private bool m_hasCinematicCamera;
    [SerializeField]
    private BezierSpline m_cinematicCameraSpline;

    [Header("Interactable Options")]
    [SerializeField]
    private EInteractableType m_type;
    [SerializeField]
    private bool m_hasShimmer;
    [SerializeField]
    private float m_visualRadius = 12.0f;
    public float visualRadius
    {
        get
        {
            return m_visualRadius;
        }
    }
    


}
