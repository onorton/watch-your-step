using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    private Rigidbody2D _rigidbody;


    private Vector3 _targetPosition;

    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _targetPosition = transform.position;
    }

    private void FixedUpdate()
    {
        // Counter gravity
        _rigidbody.AddForce(-Physics2D.gravity * _rigidbody.mass);

        var displacementY = _targetPosition.y - transform.position.y;



        if (Mathf.Abs(displacementY) > 0.1f || Mathf.Abs(_rigidbody.velocity.y) > 0.0f)
        {

            var acceleration = Mathf.Sign(displacementY);
            // Assume that we have to slow down
            var finalVelocity = Mathf.Sqrt((_rigidbody.velocity.y * _rigidbody.velocity.y) + 2 * (Mathf.Sign(-_rigidbody.velocity.y)) * displacementY);

            if (Mathf.Abs(finalVelocity) > 0.0f)
            {
                acceleration = Mathf.Sign(-_rigidbody.velocity.y);
            }

            _rigidbody.AddForce(acceleration * _rigidbody.mass * _rigidbody.mass * Vector3.up);
        }

        // Don't let it rotate too far
        if (Mathf.Abs(_rigidbody.rotation) > 45.0)
        {
            _rigidbody.MoveRotation(Mathf.Sign(_rigidbody.rotation) * 45.0f);
        }
        //Solve angular rotation in the same way
        else if (Mathf.Abs(_rigidbody.rotation) > 0.1f || Mathf.Abs(_rigidbody.angularVelocity) > 0.0f)
        {
            var angularAcceleration = Mathf.Sign(-_rigidbody.rotation);

            var finalAngularVelocity = Mathf.Sqrt((_rigidbody.angularVelocity * _rigidbody.angularVelocity) + 2 * (Mathf.Sign(-_rigidbody.angularVelocity)) * -_rigidbody.rotation);

            if (Mathf.Abs(finalAngularVelocity) > 0.0f)
            {
                angularAcceleration = Mathf.Sign(-_rigidbody.angularVelocity);
            }


            _rigidbody.AddTorque(angularAcceleration);
        }

    }
}
