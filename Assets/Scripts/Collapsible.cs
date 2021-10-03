using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collapsible : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    [SerializeField]
    private float _jitter;

    [SerializeField]
    private float _delaySeconds;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _delaySeconds = 0.3f;
        _jitter = 0.005f;
        _rigidbody.gravityScale = 0;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(Collapse());
    }

    private IEnumerator Collapse()
    {
        var counter = 0f;
        while (counter < _delaySeconds)
        {
            var direction = new Vector2(Random.Range(-_jitter, _jitter), Random.Range(-_jitter, _jitter));
            counter += Time.deltaTime;
            transform.Translate(direction);

            yield return null;
        }
        _rigidbody.gravityScale = 1;
    }
}
