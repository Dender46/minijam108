using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public GameObject m_CastlesParent;

    void Start()
    {
        for (int i = 0; i < m_CastlesParent.transform.childCount; i++)
        {
            Destroy(m_CastlesParent.transform.GetChild(i).gameObject);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                switch(hit.collider.tag)
                {
                    case "CastleBlock":
                        Vector3 topOfBlock = hit.collider.transform.position;
                        topOfBlock.y += 2;
                        CastlesManager.AddNewBlock(hit.collider.gameObject);
                        break;
                    case "Ground":
                        CastlesManager.CreateNewCastle(hit.point);
                        break;
                }
            }
        }
    }

}
