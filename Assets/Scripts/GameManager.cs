using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameManager instance;

    bool m_IsPuzzlePlaying = false;

    void Awake()
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

    public void OnStart()
    {
        
    }


    public void OnFail()
    {

    }
}
