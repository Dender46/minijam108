using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public GameObject m_PuzzlePrefab;
    public GameObject m_Water;
    public float m_WaterOscilationScale;

    public GameObject m_EscText;

    GameObject m_PuzzleObject;

    void Start()
    {
        m_EscText.SetActive(false);
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            m_EscText.SetActive(true);
        }
    }

    void Update()
    {
        // TODO: Move oscilating water
        m_Water.transform.Rotate(Mathf.Sin(Time.time) / m_WaterOscilationScale, 0.0f, 0.0f);

        if (Input.GetMouseButtonDown(0))
        {
            OnMousePressed();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Application.Quit();
        }

    }

    void OnMousePressed()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out hit))
            return;

        switch(hit.collider.tag)
        {
            case "CastleBlock":
                GameObject castleBlock = hit.collider.gameObject;
                if (!CastlesManager.instance.IsCastleMaxHeight(castleBlock))
                {
                    CastlesManager.instance.AddNewBlock(castleBlock);
                    CreatePuzzle(ray.origin, hit);
                }
                break;
            case "Ground":
                CastlesManager.instance.CreateNewCastle(hit.point);
                CreatePuzzle(ray.origin, hit);
                break;
        }
    }

    void CreatePuzzle(Vector3 hitOrigin, RaycastHit hit)
    {
        m_PuzzleObject = Instantiate(m_PuzzlePrefab);
        m_PuzzleObject.transform.position = hit.point;

        Vector3 dirToCameraPlane = hitOrigin - hit.point;
        m_PuzzleObject.transform.Translate(dirToCameraPlane * 0.5f);
    }


}
