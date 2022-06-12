using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public GameObject m_PuzzlePrefab;

    GameObject m_PuzzleObject;

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

    void OnMouseReleased()
    {
        if (CastlesManager.instance.IsCurrentlyBuilding())
            CastlesManager.instance.OnPuzzleLose();
        Destroy(m_PuzzleObject);
    }


    void CreatePuzzle(Vector3 hitOrigin, RaycastHit hit)
    {
        m_PuzzleObject = Instantiate(m_PuzzlePrefab);
        m_PuzzleObject.transform.position = hit.point;

        Vector3 dirToCameraPlane = hitOrigin - hit.point;
        m_PuzzleObject.transform.Translate(dirToCameraPlane * 0.5f);
    }


}
