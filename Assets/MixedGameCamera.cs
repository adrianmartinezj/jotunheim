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
    private Vector3 m_cameraOffset = new Vector3(0, 25, -20);
    private BezierSpline m_cameraSpline = null;
    private GameCameraMode m_mode = GameCameraMode.Game;

    public void SwitchToCinematicCamera()
    {
        mode = GameCameraMode.Cinematic;
    }

    public void SwitchToGameCamera()
    {
        mode = GameCameraMode.Game;
    }

    private void Update()
    {
        if (mode == GameCameraMode.Game)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length > 0)
            {
                Vector3 avgPlayerPosition = GetAveragePlayerPosition(players);
                transform.position = Vector3.Slerp(
                    transform.position, 
                    avgPlayerPosition + m_cameraOffset, 
                    Time.deltaTime * CAMERA_FOLLOW_SPEED);
            }
        }
    }

    private Vector3 GetAveragePlayerPosition(GameObject[] players)
    {
        float numPlayers = players.Length;
        float avgX = 0f, avgY = 0f, avgZ = 0f;
        foreach (GameObject player in players)
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
