using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _jumpForce;
    [SerializeField]
    private float _walkingSpeed;
    [SerializeField]
    private float _balancingWalkingSpeed;

    private bool _balancing;
    public bool Balancing
    {
        get { return _balancing; }
        set
        {
            _balancing = value;
            if (_balancing)
            {
                _balancingWalkSource.Play();
            }
            else
            {
                _balancingWalkSource.Stop();
            }
            _animator.SetBool("Balancing", _balancing);
        }
    }

    [SerializeField]
    private bool _onGround;

    private Collider2D _collider;

    private Rigidbody2D _rigidbody;

    private AudioSource _balancingWalkSource;
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _jumpSound;
    [SerializeField]
    private AudioClip _landSound;

    private Animator _animator;
    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _jumpForce = 7.0f;
        _walkingSpeed = 2.5f;
        _balancingWalkingSpeed = 1.0f;
        _collider = GetComponent<Collider2D>();
        _onGround = true;
        _balancingWalkSource = GameObject.Find("Balanced Walking Sound").GetComponent<AudioSource>();
        _audioSource = GameObject.Find("Sound Effects").GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();

        // Push player forward initially
        _rigidbody.AddForce(Vector2.right * _rigidbody.mass * 10.0f, ForceMode2D.Impulse);
    }


    private float CurrentSpeed()
    {
        return Balancing ? _balancingWalkingSpeed : _walkingSpeed;
    }

    // Update is called once per frame
    private void Update()
    {

        if (Time.timeScale == 0.0f)
        {
            return;
        }

        _animator.SetBool("Idle", true);
        _animator.SetBool("Left", false);
        _animator.SetBool("Right", false);

        _rigidbody.velocity = new Vector2(0.0f, _rigidbody.velocity.y);

        if (Input.GetKey(KeyCode.D))
        {
            _animator.SetBool("Idle", false);
            _animator.SetBool("Right", true);
            _rigidbody.velocity = new Vector2(CurrentSpeed(), _rigidbody.velocity.y);
        }

        if (Input.GetKey(KeyCode.A))
        {
            _animator.SetBool("Idle", false);
            _animator.SetBool("Left", true);
            _rigidbody.velocity = new Vector2(-CurrentSpeed(), _rigidbody.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.W) && _onGround && !Balancing)
        {
            _rigidbody.AddForce(new Vector2(0.0f, _jumpForce), ForceMode2D.Impulse);
            _audioSource.PlayOneShot(_jumpSound);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            _onGround = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            _onGround = true;
            _audioSource.PlayOneShot(_landSound);
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform") || collision.gameObject.layer == LayerMask.NameToLayer("InactiveToPlayer"))
        {
            _onGround = false;
        }
    }
}
