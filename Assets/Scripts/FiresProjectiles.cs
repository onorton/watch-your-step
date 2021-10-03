using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiresProjectiles : MonoBehaviour
{
    [SerializeField]
    private GameObject _projectile;


    [SerializeField]
    private float _delaySeconds;

    private float _timePassed = 0.0f;

    private float _velocityMagnitude;

    private Transform _target;

    private void Start()
    {
        _delaySeconds = 2.0f;
        _velocityMagnitude = 7.5f;
        _target = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        _timePassed += Time.deltaTime;
        if (_timePassed >= _delaySeconds)
        {
            _timePassed = 0.0f;
            Fire();
        }
    }

    private void Fire()
    {
        var projectileFired = Instantiate(_projectile, transform);

        var displacement = _target.transform.position - transform.position;

        var velocity = Vector2.zero;
        // Sample 100 times until one is found or fail (as it can happen)
        for (var i = 0; i < 1000; i++)
        {
            var velocityX = Random.Range(-_velocityMagnitude, _velocityMagnitude);
            var t = displacement.x / velocityX;
            if (t < 0.0f)
            {
                continue;
            }

            var velocityY = (displacement.y - 0.5f * Physics2D.gravity.y * t * t) / t;

            var magnitude = Mathf.Sqrt(velocityX * velocityX + velocityY * velocityY);
            if (Mathf.Abs(magnitude - _velocityMagnitude) < 1.0f)
            {
                velocity = new Vector2(velocityX, velocityY);
            }
        }

        if (velocity != Vector2.zero)
        {
            projectileFired.GetComponent<Rigidbody2D>().velocity = velocity;
        }
        else
        {
            Destroy(projectileFired);
        }



    }
}
