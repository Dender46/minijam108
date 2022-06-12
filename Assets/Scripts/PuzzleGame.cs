using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGame : MonoBehaviour
{
    public GameObject m_PlayerPrefab;

    GameObject m_Player;
    Vector3 m_CameraPosDiff = Vector3.zero;

    Vector3 m_OrigPos;

    void Start()
    {
        // Add player controller prefab
        m_Player = Instantiate(m_PlayerPrefab, transform);
        m_OrigPos = transform.position;
    }

    void Update()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + m_CameraPosDiff;
        
        Vector3 newPos = new Vector3(
            mousePos.x,
            mousePos.y,
            transform.position.z
        );

        // Move whole puzzle and player
        transform.position = m_OrigPos - (newPos - m_OrigPos);
        m_Player.transform.position = newPos;
    }
}
