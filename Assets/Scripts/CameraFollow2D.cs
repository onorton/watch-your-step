using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;
    private Vector3 _previousPosition;

    private Camera _camera;
    private Vector3 _velocity = Vector3.zero;

    // yOffset of camera from target
    private float yOffset = 1.0f;

    // In viewport space
    private const float horizontalPadding = 0.4f;
    private const float verticalPadding = 0.3f;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        transform.position = new Vector3(target.position.x, target.position.y + yOffset, transform.position.z);
    }

    private void LateUpdate()
    {
        var point = _camera.WorldToViewportPoint(target.position);
        var deltaX = 0.0f;
        var deltaY = 0.0f;

        if (point.x > 1.0f - horizontalPadding)
        {
            deltaX = point.x - (1 - horizontalPadding);

        }
        else if (point.x < horizontalPadding)
        {
            deltaX = point.x - horizontalPadding;
        }

        if (point.y > 1.0f - verticalPadding)
        {
            deltaY = point.y - (1 - verticalPadding);

        }
        else if (point.y < verticalPadding)
        {
            deltaY = point.y - verticalPadding;
        }

        var destination = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f) + new Vector3(deltaX, deltaY, 0.0f));

        transform.position = Vector3.SmoothDamp(transform.position, destination, ref _velocity, 0.3f);

    }
}
