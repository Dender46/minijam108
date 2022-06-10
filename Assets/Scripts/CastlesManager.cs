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

    Dictionary<Vector2, Castle> m_Castles = new Dictionary<Vector2, Castle>();

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
        Vector2 key = new Vector2(pos.x, pos.z);
        instance.m_Castles.Add(key, castleObj);

        GameObject newCastle = new GameObject("Castle");
        newCastle.transform.SetParent(instance.m_CastlesParent);
        newCastle.transform.position = pos;
        castleObj.parent = newCastle.transform;

        var randomBlock = instance.m_Blocks[Random.Range(0, instance.m_Blocks.Count)];
        var spawnedBlock = Instantiate(randomBlock, newCastle.transform.position, randomBlock.transform.rotation);
        spawnedBlock.transform.SetParent(castleObj.parent);
        castleObj.blocks.Add(spawnedBlock);

        castleObj.maxHeight = (int)Random.Range(instance.m_MinMaxHeight.x, instance.m_MinMaxHeight.y + 1);
    }

    public static void AddNewBlock(GameObject castleBlock)
    {
        Vector3 castleBlockPos = castleBlock.transform.position;
        Vector2 key = new Vector2(castleBlockPos.x, castleBlockPos.z);
        Castle castleObj = instance.m_Castles[key];
        
        var lastCastleBlock = castleObj.blocks[castleObj.blocks.Count - 1];
        var newPos = lastCastleBlock.transform.position;
        newPos.y += 2;

        GameObject randomBlock;
        if (castleObj.blocks.Count < castleObj.maxHeight - 1)
            randomBlock = instance.m_Blocks[Random.Range(0, instance.m_Blocks.Count)];
        else
            randomBlock = instance.m_Roofs[Random.Range(0, instance.m_Roofs.Count)];
        
        var spawnedBlock = Instantiate(randomBlock, newPos, randomBlock.transform.rotation);
        spawnedBlock.transform.SetParent(castleObj.parent);
        castleObj.blocks.Add(spawnedBlock);
    }

    public static bool IsCastleMaxHeight(GameObject castleBlock)
    {
        Vector3 castleBlockPos = castleBlock.transform.position;
        Vector2 key = new Vector2(castleBlockPos.x, castleBlockPos.z);
        Castle castleObj = instance.m_Castles[key];
        return castleObj.blocks.Count == castleObj.maxHeight;
    }

}
