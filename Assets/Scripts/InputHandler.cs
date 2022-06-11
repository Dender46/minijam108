using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public GameObject m_PuzzlePrefab;

    GameObject m_PuzzleObject;

    bool m_IsPressing;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMousePressed();
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnMouseReleased();
        }
    }

    void OnMousePressed()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out hit))
            return;

        m_IsPressing = true;

        CreateCastle(hit);
        CreatePuzzle(hit);
    }

    void OnMouseReleased()
    {
        //if (!CastlesManager.LastBuildIsSuccessful())
        //    CastlesManager.DestroyCastle();
        Destroy(m_PuzzleObject);
    }

    void CreateCastle(RaycastHit hit)
    {
        switch(hit.collider.tag)
        {
            case "CastleBlock":
                GameObject castleBlock = hit.collider.gameObject;
                if (!CastlesManager.IsCastleMaxHeight(castleBlock))
                    CastlesManager.AddNewBlock(castleBlock);
                break;
            case "Ground":
                CastlesManager.CreateNewCastle(hit.point);
                break;
        }
    }

    void CreatePuzzle(RaycastHit hit)
    {
        m_PuzzleObject = Instantiate(m_PuzzlePrefab);
        m_PuzzleObject.transform.position = hit.point;
    }


}
