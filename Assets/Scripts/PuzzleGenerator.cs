using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGenerator : MonoBehaviour
{
    [Serializable]
    public struct Path
    {
        public Vector2 beginPos;
        public Vector2 nextPos;
        public GameObject path;
    }

    public List<Path> m_Paths;
    public List<Path> m_EndPaths;

    public int m_MaxLength = 5;
    public int m_Length = 0;
    public float m_NewPosOffset = 3.999f;

    private Dictionary<Vector2, List<Path>> m_DictPaths = new Dictionary<Vector2, List<Path>>();
    private Dictionary<Vector2, List<Path>> m_DictEndPaths = new Dictionary<Vector2, List<Path>>();
    private GameObject m_LastPathGameObject;
    private Path m_LastPath;

    public static PuzzleGenerator instance;

    void Start()
    {
        Initialize();
        CreatePuzzle();
    }

    void Initialize()
    {
        foreach (Path path in m_Paths)
        {
            var key = path.beginPos;
            if (!m_DictPaths.ContainsKey(key))
                m_DictPaths[key] = new List<Path>();

            m_DictPaths[key].Add(path);
        }

        foreach (Path path in m_EndPaths)
        {
            var key = path.beginPos;
            if (!m_DictEndPaths.ContainsKey(key))
                m_DictEndPaths[key] = new List<Path>();

            m_DictEndPaths[key].Add(path);
        }
    }

    void Update()
    {
        //transform.Translate(0.0f, -1.0f * Time.deltaTime, 0.0f);
    }

    void CreatePuzzle()
    {
        m_LastPath = m_Paths[0];
        m_LastPathGameObject = Instantiate(m_LastPath.path, transform);

        for (int i = 0; i < m_MaxLength; i++)
        {
            CreateNewSegment();
        }

        CreateFinishSegment();
    }


    void CreateNewSegment()
    {
        // TODO: skip same path as last one; right now brute force it
        Path o;
        do
        {
            o = m_DictPaths[m_LastPath.nextPos][UnityEngine.Random.Range(0, m_DictPaths[m_LastPath.nextPos].Count)];
        }
        while (o.path == m_LastPath.path);

        Vector3 newPos = new Vector3(
            m_LastPathGameObject.transform.position.x + m_LastPath.nextPos.x * m_NewPosOffset,
            m_LastPathGameObject.transform.position.y + m_LastPath.nextPos.y * m_NewPosOffset,
            m_LastPathGameObject.transform.position.z
        );
        GameObject newNewPath = Instantiate(o.path, newPos, transform.rotation, transform);
            
        m_LastPath = o;
        m_LastPathGameObject = newNewPath;
    }

    void CreateFinishSegment()
    {
        Path o = m_DictEndPaths[m_LastPath.nextPos][UnityEngine.Random.Range(0, m_DictEndPaths[m_LastPath.nextPos].Count)];
        
        Vector3 newPos = new Vector3(
            m_LastPathGameObject.transform.position.x + m_LastPath.nextPos.x * m_NewPosOffset,
            m_LastPathGameObject.transform.position.y + m_LastPath.nextPos.y * m_NewPosOffset,
            m_LastPathGameObject.transform.position.z
        );
        GameObject newNewPath = Instantiate(o.path, newPos, transform.rotation, transform);
    }

    public Vector3 GetEndingPosition()
    {
        return m_LastPathGameObject.transform.position;
    }


}
