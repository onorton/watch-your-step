using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _gameOverSound;

    private StateManager _stateManager;

    private void Start()
    {
        _stateManager = FindObjectOfType<StateManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            Debug.Log("Game over");
            _stateManager.GameOver();
        }
    }
}
