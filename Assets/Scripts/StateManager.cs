using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{

    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _gameOverSound;
    [SerializeField]
    private AudioClip _victorySound;


    private GameObject _gameOverUi;
    private GameObject _victoryUi;
    private GameObject _pauseUi;
    // Start is called before the first frame update
    private void Start()
    {
        _audioSource = GameObject.Find("Sound Effects").GetComponent<AudioSource>();
        var ui = GameObject.Find("UI");
        _gameOverUi = ui.transform.Find("GameOver").gameObject;
        _victoryUi = ui.transform.Find("Victory").gameObject;
        _pauseUi = ui.transform.Find("Paused").gameObject;
        _gameOverUi.SetActive(false);
        _victoryUi.SetActive(false);
        _pauseUi.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Victory()
    {
        _audioSource.PlayOneShot(_victorySound);
        _victoryUi.SetActive(true);
        Time.timeScale = 0;
    }

    public void GameOver()
    {
        _audioSource.PlayOneShot(_gameOverSound);
        _gameOverUi.SetActive(true);
        Time.timeScale = 0;
    }

    private void Pause()
    {
        _pauseUi.SetActive(true);
        Time.timeScale = 0;
    }

    public void Unpause()
    {
        _pauseUi.SetActive(false);
        Time.timeScale = 1;
    }
}
