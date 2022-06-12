using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> m_BuildingSounds;
    public List<AudioClip> m_DestructionSounds;
    public List<AudioClip> m_SuccessSounds;

    class AudioWrapper
    {
        public AudioClip m_LastClip;
        public AudioSource m_Source;
    }

    AudioWrapper m_Src = new AudioWrapper();
    AudioWrapper m_BuildingSrc = new AudioWrapper();

    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            m_Src.m_Source = GetComponents<AudioSource>()[0];
            m_BuildingSrc.m_Source = GetComponents<AudioSource>()[1];
        }
        else
        {
            Debug.LogWarning("Instance already exists, destroying 'this'!");
            Destroy(this);
        }
    }

    void Update()
    {
        if (CastlesManager.instance.IsBuilding())
        {
            if (!m_BuildingSrc.m_Source.isPlaying)
            {
                AudioClip nextClip = GetRandomSource(m_BuildingSounds, m_BuildingSrc.m_LastClip);
                m_BuildingSrc.m_Source.PlayOneShot(nextClip);
                m_BuildingSrc.m_LastClip = nextClip;
            }
        }
            
    }

    AudioClip GetRandomSource(List<AudioClip> audio, AudioClip lastPlayed)
    {
        AudioClip nextClip;
        do
        {
            nextClip = audio[Random.Range(0, audio.Count)];
            if (lastPlayed == null)
                return nextClip;
        }
        while (nextClip == lastPlayed);

        return nextClip;
    }

    public void PlayLose()
    {
        AudioClip nextClip = GetRandomSource(m_DestructionSounds, m_Src.m_LastClip);
        m_Src.m_Source.PlayOneShot(nextClip);
        m_Src.m_LastClip = nextClip;
    }

    public void PlayWin()
    {
        m_Src.m_Source.PlayOneShot(m_SuccessSounds[0]);
    }
}
