using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastlesManager : MonoBehaviour
{
    public static CastlesManager instance;

    public Transform m_CastlesParent;
    public GameObject m_WrapperCastle;
    public List<GameObject> m_Blocks = new List<GameObject>();
    public List<GameObject> m_Roofs = new List<GameObject>();
    public Vector2 m_MinMaxHeight = new Vector2(2, 3);
    public float m_CollapsingSpeed = 1.0f;

    Dictionary<Transform, Castle> m_Castles = new Dictionary<Transform, Castle>();

    Castle m_CurrentBuildingCastle;
    Castle m_LastSuccessfulCastle;

    List<GameObject> m_CurrentlyCollapsingCastles = new List<GameObject>();

    class Castle
    {
        public List<GameObject> blocks = new List<GameObject>();
        public Transform parent;
        public int maxHeight;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            for (int i = 0; i < m_CastlesParent.childCount; i++)
            {
                Destroy(m_CastlesParent.transform.GetChild(i).gameObject);
            }
        }
        else
        {
            Debug.LogWarning("Instance already exists, destroying 'this'!");
            Destroy(this);
        }
    }

    void Update()
    {
        for (int i = 0; i < m_CurrentlyCollapsingCastles.Count; i++)
        {
            var castle = m_CurrentlyCollapsingCastles[i];
            var move = new Vector3(0.0f, m_CollapsingSpeed * Time.deltaTime, 0.0f);
            
            castle.transform.Translate(-move);
            var sh = castle.GetComponent<ParticleSystem>().shape;
            sh.position += move;

            if (castle.transform.position.y < -30.0f)
            {
                m_CurrentlyCollapsingCastles.RemoveAt(i--);
                Destroy(castle);
            }
        }
    }

    public void CreateNewCastle(Vector3 pos)
    {
        Castle castleObj = new Castle();

        GameObject newCastle = Instantiate(m_WrapperCastle, pos, Quaternion.identity, m_CastlesParent);
        newCastle.transform.localRotation = Quaternion.identity;
        castleObj.parent = newCastle.transform;

        var randomBlock = m_Blocks[Random.Range(0, m_Blocks.Count)];
        var spawnedBlock = Instantiate(randomBlock, castleObj.parent);
        castleObj.blocks.Add(spawnedBlock);

        castleObj.maxHeight = Random.Range((int)m_MinMaxHeight.x, (int)m_MinMaxHeight.y + 1);
        
        m_Castles.Add(newCastle.transform, castleObj);
        m_CurrentBuildingCastle = castleObj;
    }

    public void AddNewBlock(GameObject castleBlock)
    {
        Castle castleObj = m_Castles[castleBlock.transform.parent];

        GameObject randomBlock;
        if (castleObj.blocks.Count < castleObj.maxHeight - 1)
            randomBlock = m_Blocks[Random.Range(0, m_Blocks.Count)];
        else
            randomBlock = m_Roofs[Random.Range(0, m_Roofs.Count)];
        
        var spawnedBlock = Instantiate(randomBlock, castleObj.parent);
        spawnedBlock.transform.localRotation = Quaternion.identity;
        spawnedBlock.transform.Rotate(new Vector3(0.0f, 90.0f * (int)Random.Range(0, 3), 0.0f), Space.Self);
        spawnedBlock.transform.Translate(new Vector3(0.0f, castleObj.blocks.Count * 2.0f, 0.0f), Space.Self);
        
        foreach (var mat in spawnedBlock.GetComponent<MeshRenderer>().materials)
        {
            mat.SetFloat("_Fill", 0.0f);
        }

        castleObj.blocks.Add(spawnedBlock);
        m_CurrentBuildingCastle = castleObj;
    }

    public bool IsCastleMaxHeight(GameObject castleBlock)
    {
        Castle castleObj = m_Castles[castleBlock.transform.parent];
        return castleObj.blocks.Count == castleObj.maxHeight;
    }

    public void UpdateBuildingProgress(float progress)
    {
        foreach (var mat in m_CurrentBuildingCastle.blocks[m_CurrentBuildingCastle.blocks.Count - 1].GetComponent<MeshRenderer>().materials)
        {
            mat.SetFloat("_Fill", progress);
        }
    }

    public void OnPuzzleWin()
    {
        m_CurrentBuildingCastle.blocks[m_CurrentBuildingCastle.blocks.Count - 1].GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
        UpdateBuildingProgress(1.0f);
        m_LastSuccessfulCastle = m_CurrentBuildingCastle;
        m_CurrentBuildingCastle = null;
    }

    public void OnPuzzleLose()
    {
        // Function is also used when releasing mouse button
        if (m_CurrentBuildingCastle == null)
            return;

        // Stop last castle block particles system
        m_CurrentBuildingCastle.blocks[m_CurrentBuildingCastle.blocks.Count - 1].GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
        // Demolition particle system
        m_CurrentBuildingCastle.parent.gameObject.GetComponent<ParticleSystem>().Play();
        m_CurrentlyCollapsingCastles.Add(m_CurrentBuildingCastle.parent.gameObject);
        m_CurrentBuildingCastle = null;
        m_LastSuccessfulCastle = null;
    }

    public bool IsPuzzleLost()
    {
        return m_LastSuccessfulCastle == null;
    }

    public bool IsBuilding()
    {
        return m_CurrentBuildingCastle != null;
    }
}
