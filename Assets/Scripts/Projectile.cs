using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float timeToLiveSeconds = 20.0f;

    private StateManager _stateManager;

    private void Start()
    {
        _stateManager = FindObjectOfType<StateManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (timeToLiveSeconds < 0.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Player")
        {
            Debug.Log("Game over");
            _stateManager.GameOver();
        }
        Destroy(gameObject);
    }
}
