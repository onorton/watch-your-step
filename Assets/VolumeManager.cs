using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeManager : MonoBehaviour
{

    private AudioSource[] _audioSources;

    [SerializeField]
    private GameObject _mutedButton;
    [SerializeField]
    private GameObject _unmutedButton;


    // Start is called before the first frame update
    private void Start()
    {
        _audioSources = FindObjectsOfType<AudioSource>();

    }

    // Update is called once per frame
    public void Mute()
    {

        foreach (var audioSource in _audioSources)
        {
            audioSource.mute = true;
        }
        _mutedButton.SetActive(true);
        _unmutedButton.SetActive(false);
    }

    public void Unmute()
    {

        foreach (var audioSource in _audioSources)
        {
            audioSource.mute = false;
        }
        _mutedButton.SetActive(false);
        _unmutedButton.SetActive(true);
    }
}
