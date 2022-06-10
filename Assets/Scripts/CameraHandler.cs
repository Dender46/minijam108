using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public GameObject m_Object;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.point);
                SpawnObject(hit.point);
            }
        }
    }

    void SpawnObject(Vector3 pos)
    {
        Instantiate(m_Object, pos, Quaternion.identity);
    }

}
