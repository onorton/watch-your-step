using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StabilityMeter : MonoBehaviour
{
    private RectTransform _arrow;

    private Image _leftArrow;
    private Image _rightArrow;
    private Color _unActivatedColor;
    private RectTransform _rectTransform;
    private Tightrope _tightrope;


    // Start is called before the first frame update
    private void Start()
    {
        _arrow = transform.Find("Arrow").GetComponent<RectTransform>();
        _leftArrow = transform.Find("Left Arrow").GetComponent<Image>();
        _rightArrow = transform.Find("Right Arrow").GetComponent<Image>();
        _unActivatedColor = _leftArrow.color;
        _rectTransform = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (_tightrope)
        {
            var pixelToStability = (_rectTransform.rect.width / 2.0f) / _tightrope.MaxStabilityOffset;

            _arrow.localPosition = new Vector3(_tightrope.Stability * pixelToStability, _arrow.localPosition.y, _arrow.localPosition.z);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _leftArrow.color = Color.white;
        }
        else
        {
            _leftArrow.color = _unActivatedColor;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            _rightArrow.color = Color.white;
        }
        else
        {
            _rightArrow.color = _unActivatedColor;
        }
    }

    public void SetTightrope(Tightrope tightrope)
    {
        _tightrope = tightrope;
        gameObject.SetActive(true);
    }

    public void RemoveTightrope()
    {
        _tightrope = null;
        gameObject.SetActive(false);
    }
}
