using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioClip[] m_audioClips;
    [SerializeField] private AudioSource m_audioSource;
    private void Awake()
    {
        instance ??= this;
    }
    public void PlayAudioClip(int l_audioIndex)
    {
        if (l_audioIndex < m_audioClips.Length)
        {
            m_audioSource.clip = m_audioClips[l_audioIndex];
            m_audioSource.Play();
        }
    }
}
