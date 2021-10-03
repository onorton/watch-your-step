using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tightrope : MonoBehaviour
{

    public float MaxStabilityOffset { get; private set; }
    public float Stability;
    private float _stabilityChange;
    private float _playerStabilityChange;

    [SerializeField]
    private bool _playerOn;

    private Collider2D _platform;

    private StabilityMeter _stabilityMeter;

    private void Awake()
    {
        _stabilityMeter = FindObjectOfType<StabilityMeter>();
    }

    private void Start()
    {
        MaxStabilityOffset = 10.0f;
        Stability = 0.0f;
        _stabilityChange = 3.0f;
        _playerStabilityChange = 0.4f;

        _playerOn = false;

        _platform = transform.Find("Platform").GetComponent<Collider2D>();

    }

    private void Update()
    {

        if (_playerOn)
        {
            // Change stability in current direction
            Stability += _stabilityChange * Time.deltaTime * Mathf.Sign(Stability);

            if (Mathf.Abs(Stability) > MaxStabilityOffset)
            {
                _platform.gameObject.layer = LayerMask.NameToLayer("InactiveToPlayer");
                _stabilityMeter.RemoveTightrope();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Stability += _playerStabilityChange;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Stability -= _playerStabilityChange;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            _playerOn = true;
            other.GetComponent<PlayerController>().Balancing = true;
            _stabilityMeter.SetTightrope(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            _playerOn = false;
            other.GetComponent<PlayerController>().Balancing = false;
            _stabilityMeter.RemoveTightrope();

            // Reset if player falls off
            if (_platform.gameObject.layer == LayerMask.NameToLayer("InactiveToPlayer"))
            {
                Stability = 0.0f;
            }

            _platform.gameObject.layer = LayerMask.NameToLayer("Platform");
        }
    }
}
