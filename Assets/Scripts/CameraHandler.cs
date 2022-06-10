using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public GameObject m_CastlesParent;
    public List<GameObject> m_Blocks;

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
        var randomBlock = m_Blocks[Random.Range(0, m_Blocks.Count-1)];
        var spawnedBlock = Instantiate(randomBlock, pos, randomBlock.transform.rotation);
        spawnedBlock.transform.SetParent(m_CastlesParent.transform);
    }

}
