using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastlesManager : MonoBehaviour
{
    public static CastlesManager instance;

    public Transform m_CastlesParent;
    public List<GameObject> m_Blocks = new List<GameObject>();

    Dictionary<Vector2, Castle> m_Castles = new Dictionary<Vector2, Castle>();

    class Castle
    {
        public List<GameObject> blocks = new List<GameObject>();
        public Transform parent;
        public int size;
        public int maxSize;
    }

    void Start()
    {
        if (instance == null)
        {
            instance = this;
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
    }

    public static void AddNewBlock(GameObject castleBlock)
    {
        Vector3 castleBlockPos = castleBlock.transform.position;
        Vector2 key = new Vector2(castleBlockPos.x, castleBlockPos.z);
        Castle castleObj = instance.m_Castles[key];
        
        var lastCastleBlock = castleObj.blocks[castleObj.blocks.Count - 1];
        var newPos = lastCastleBlock.transform.position;
        newPos.y += 2;

        var randomBlock = instance.m_Blocks[Random.Range(0, instance.m_Blocks.Count)];
        var spawnedBlock = Instantiate(randomBlock, newPos, randomBlock.transform.rotation);
        spawnedBlock.transform.SetParent(castleObj.parent);
        castleObj.blocks.Add(spawnedBlock);
    }

}
