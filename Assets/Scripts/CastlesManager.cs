using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastlesManager : MonoBehaviour
{
    public static CastlesManager instance;

    public Transform m_CastlesParent;
    public List<GameObject> m_Blocks = new List<GameObject>();
    public List<GameObject> m_Roofs = new List<GameObject>();
    public Vector2 m_MinMaxHeight = new Vector2(2, 3);

    Dictionary<Transform, Castle> m_Castles = new Dictionary<Transform, Castle>();
    GameObject m_EmptyCastle;

    class Castle
    {
        public List<GameObject> blocks = new List<GameObject>();
        public Transform parent;
        public int maxHeight;
    }

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            m_EmptyCastle = new GameObject("Castle");
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

    public static void CreateNewCastle(Vector3 pos)
    {
        Castle castleObj = new Castle();

        GameObject newCastle = Instantiate(instance.m_EmptyCastle, pos, Quaternion.identity, instance.m_CastlesParent);
        newCastle.transform.localRotation = Quaternion.identity;
        castleObj.parent = newCastle.transform;

        var randomBlock = instance.m_Blocks[Random.Range(0, instance.m_Blocks.Count)];
        var spawnedBlock = Instantiate(randomBlock, castleObj.parent);
        castleObj.blocks.Add(spawnedBlock);

        castleObj.maxHeight = Random.Range((int)instance.m_MinMaxHeight.x, (int)instance.m_MinMaxHeight.y + 1);
        
        instance.m_Castles.Add(newCastle.transform, castleObj);
    }

    public static void AddNewBlock(GameObject castleBlock)
    {
        Castle castleObj = instance.m_Castles[castleBlock.transform.parent];

        GameObject randomBlock;
        if (castleObj.blocks.Count < castleObj.maxHeight - 1)
            randomBlock = instance.m_Blocks[Random.Range(0, instance.m_Blocks.Count)];
        else
            randomBlock = instance.m_Roofs[Random.Range(0, instance.m_Roofs.Count)];
        
        var spawnedBlock = Instantiate(randomBlock, castleObj.parent);
        spawnedBlock.transform.localRotation = Quaternion.identity;
        spawnedBlock.transform.Rotate(new Vector3(0.0f, 90.0f * (int)Random.Range(0, 3), 0.0f), Space.Self);
        spawnedBlock.transform.Translate(new Vector3(0.0f, castleObj.blocks.Count * 2.0f, 0.0f), Space.Self);

        castleObj.blocks.Add(spawnedBlock);
    }

    public static bool IsCastleMaxHeight(GameObject castleBlock)
    {
        Castle castleObj = instance.m_Castles[castleBlock.transform.parent];
        return castleObj.blocks.Count == castleObj.maxHeight;
    }

}
