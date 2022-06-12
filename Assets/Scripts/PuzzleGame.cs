using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGame : MonoBehaviour
{
    public GameObject m_PlayerPrefab;

    GameObject m_Player;
    Vector3 m_CameraPosDiff = Vector3.zero;

    Vector3 m_EndingPosition;
    float m_MaxDistance;
    Vector3 m_OrigPos;

    void Start()
    {
        // Add player controller prefab
        m_Player = Instantiate(m_PlayerPrefab, transform);
        
        m_EndingPosition = GetComponent<PuzzleGenerator>().GetEndingPosition();
        m_MaxDistance = Vector3.Distance(m_Player.transform.position, m_EndingPosition);
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

        
        float dist = Vector3.Distance(m_Player.transform.position, m_EndingPosition);
        CastlesManager.instance.UpdateBuildingProgress(Remap(dist, 0.0f, m_MaxDistance, 1.5f, 0.0f));
    }

    public void OnWin()
    {
        CastlesManager.instance.OnPuzzleWin();
        AudioManager.instance.PlayWin();
        Destroy(gameObject);
    }

    public void OnFail()
    {
        CastlesManager.instance.OnPuzzleLose();
        AudioManager.instance.PlayLose();
        Destroy(gameObject);
    }

    float Remap(float source, float sourceFrom, float sourceTo, float targetFrom, float targetTo)
    {
	    return targetFrom + (source-sourceFrom)*(targetTo-targetFrom)/(sourceTo-sourceFrom);
    }
}
