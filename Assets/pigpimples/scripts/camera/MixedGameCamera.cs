using System;
using System.Collections.Generic;
using UnityEngine;

public class MixedGameCamera : MonoBehaviour
{
    public enum GameCameraMode
    {
        Game,
        Cinematic
    }

    public GameCameraMode mode 
    { 
        get 
        { 
            return m_mode; 
        } 
        private set
        {
            m_mode = value;
            //UpdateCameraMode();
        }
    }

    [SerializeField]
    private float CAMERA_FOLLOW_SPEED = 2.0f;
    private float CAMERA_FOLLOW_SPLINE_SPEED = 1.0f;
    private float CAMERA_ROTATE_SPEED = 2.0f;
    private float CURVE_PROGRESS_CONSTANT = 0.1f;
    private float m_prevCurveProgress = 0.0f;
    private Vector3 m_cameraOffset = new Vector3(0, 25, -20);
    private Vector3 m_cameraRotationOffset = new Vector3(55, 0, 0);
    private BezierSpline m_cameraSpline = null;
    private GameCameraMode m_mode = GameCameraMode.Game;
    private GameObject[] m_players;

    private void Start()
    {
        m_players = GameObject.FindGameObjectsWithTag("Player");
    }

    public void SwitchToCinematicCamera(BezierSpline spline)
    {
        mode = GameCameraMode.Cinematic;
        m_cameraSpline = spline;
    }

    public void SwitchToGameCamera()
    {
        mode = GameCameraMode.Game;
    }

    private void Update()
    {
        if (mode == GameCameraMode.Game)
        {
            if (m_players.Length > 0)
            {
                Vector3 avgPlayerPosition = GetAveragePlayerPosition();
                transform.position = Vector3.Slerp(
                    transform.position, 
                    avgPlayerPosition + m_cameraOffset, 
                    Time.deltaTime * CAMERA_FOLLOW_SPEED);
            }
            if (transform.rotation != Quaternion.Euler(m_cameraRotationOffset))
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.Euler(m_cameraRotationOffset),
                    Time.deltaTime * CAMERA_ROTATE_SPEED
                );
            }
        }
        else if (mode == GameCameraMode.Cinematic)
        {
            //Vector3 closestPointOnSpline = m_cameraSpline.GetClosestPointOnSplineToPosition(4, 5, GetAveragePlayerPosition(), 0, 1, out float tOut);
            // If we're not within a certain distance tolerance, move towards spline
            //print("~WE'RE NOT IN THE SPLINE~");
            Vector3 avgPlayerPos = GetAveragePlayerPosition();
            float prevProg = m_prevCurveProgress - CURVE_PROGRESS_CONSTANT,
                nextProg = m_prevCurveProgress + CURVE_PROGRESS_CONSTANT;
            Vector3 closestPointOnSpline = m_cameraSpline.GetPoint(m_prevCurveProgress),
                prevPoint = m_cameraSpline.GetPoint(Mathf.Clamp(prevProg, 0, 1)),
                nextPoint = m_cameraSpline.GetPoint(Mathf.Clamp(nextProg, 0, 1));
            float curDist = Vector3.Distance(avgPlayerPos, closestPointOnSpline),
                prevDist = Vector3.Distance(avgPlayerPos, prevPoint),
                nextDist = Vector3.Distance(avgPlayerPos, nextPoint);


            if (nextDist < curDist)
            {
                closestPointOnSpline = nextPoint;
                m_prevCurveProgress = nextProg;
            }
            else if (prevDist < curDist)
            {
                closestPointOnSpline = prevPoint;
                m_prevCurveProgress = prevProg;
            }

            transform.position = Vector3.Slerp(
                transform.position, 
                closestPointOnSpline, 
                Time.deltaTime * CAMERA_FOLLOW_SPLINE_SPEED);

            // Instead of trying to grab the closest point on the spline,
            // try maintaining a certain distance from the avg player position.
            // While that number is below a certain threshold, increment the t value 
            // along the spline proportional to the difference between the current position distance
            // and the threshold.
            //transform.position = Vector3.Slerp(transform.position, closestPointOnSpline, Time.deltaTime * CAMERA_FOLLOW_SPLINE_SPEED);

            //else
            //{
            //    print("~WE'RE IN THE SPLINE~");
            //    transform.position = Vector3.Slerp(
            //        transform.position, 
            //        ,
            //        Time.deltaTime * CAMERA_FOLLOW_SPLINE_SPEED);
            //}
            // Move along spline based on players' progress along the spline
            // Look at player
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(GetAveragePlayerPosition() - transform.position),
                Time.deltaTime * CAMERA_ROTATE_SPEED);
        }
    }

    private Vector3 GetAveragePlayerPosition()
    {
        float numPlayers = m_players.Length;
        float avgX = 0f, avgY = 0f, avgZ = 0f;
        foreach (GameObject player in m_players)
        {
            Vector3 pos = player.transform.position;
            avgX += pos.x;
            avgY += pos.y;
            avgZ += pos.z;
        }
        Vector3 result = new Vector3(
            avgX / numPlayers,
            avgY / numPlayers,
            avgZ / numPlayers
        );
        return result;
    }
}
