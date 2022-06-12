using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGame : MonoBehaviour
{
    public GameObject m_PlayerPrefab;

    GameObject m_Player;
    Vector3 m_CameraPosDiff = Vector3.zero;

    Vector3 m_OldPos;

    void Start()
    {
        // Add player controller prefab
        m_Player = Instantiate(m_PlayerPrefab, transform);
    }

    void Update()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + m_CameraPosDiff;
        if (m_CameraPosDiff == Vector3.zero)
        {
            //m_CameraPosDiff = m_Player.transform.position - mousePos;
        }
        
        Vector3 oldPos = transform.position;
        Vector3 newPos = new Vector3(
            mousePos.x,
            mousePos.y,
            transform.position.z
        );
        //m_Player.transform.position = newPos;

        // Move whole puzzle
        transform.position = newPos;
    }
}
