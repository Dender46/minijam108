using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
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
                        GameObject castleBlock = hit.collider.gameObject;
                        if (!CastlesManager.IsCastleMaxHeight(castleBlock))
                            CastlesManager.AddNewBlock(castleBlock);
                        break;
                    case "Ground":
                        CastlesManager.CreateNewCastle(hit.point);
                        break;
                }
            }
        }
    }

}
