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
    private float CAMERA_ROTATE_SPEED = 2.0f;
    private float CAMERA_SPLINE_DISTANCE_TOLERANCE = 10f;
    private Vector3 m_cameraOffset = new Vector3(0, 25, -20);
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
        }
        else if (mode == GameCameraMode.Cinematic)
        {
            // If we're not within a certain distance tolerance, move towards spline
            if (Vector3.Distance(transform.position, m_cameraSpline.GetPoint(0f)) > CAMERA_SPLINE_DISTANCE_TOLERANCE)
            {
                transform.position = Vector3.Slerp(transform.position, m_cameraSpline.GetPoint(0f), Time.deltaTime * CAMERA_FOLLOW_SPEED);
            }
            else
            {
                transform.position = Vector3.Slerp(
                    transform.position, 
                    m_cameraSpline.GetClosestPointOnSplineToPosition(2, 4, GetAveragePlayerPosition(), 0, 1),
                    Time.deltaTime * CAMERA_FOLLOW_SPEED);
            }
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
