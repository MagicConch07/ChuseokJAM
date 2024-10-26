using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public enum ZoomMode
{
    Out,
    In
}

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _followCam;
    [SerializeField] private float _followCamUpSize = 30;
    [SerializeField] private float _duration = 0.3f;
    private float _followOriginalSize = 0;

    [SerializeField] private float _offsetX = 5f;
    [SerializeField] private float _offsetDuration = 0.2f;
    private CinemachineTransposer _transposer;

    private ZoomMode _currentMode = ZoomMode.In;
    private bool _isZoom = false;

    private void Awake()
    {
        _transposer = _followCam.GetCinemachineComponent<CinemachineTransposer>();
        _followOriginalSize = _followCam.m_Lens.OrthographicSize;
    }

    private void Start()
    {
        _followCam.m_Lens.OrthographicSize = _followCamUpSize;
    }

    public void CamOffset(Action action)
    {
        StartCoroutine(CamOffsetLerp(_offsetX, action));
    }

    private IEnumerator CamOffsetLerp(float size, Action action)
    {
        float time = 0;
        float startValue = _transposer.m_FollowOffset.x;

        while (time < _offsetDuration)
        {
            _transposer.m_FollowOffset.x = Mathf.Lerp(startValue, size, time / _offsetDuration);

            time += Time.deltaTime;
            yield return null;
        }

        _transposer.m_FollowOffset.x = _offsetX;
        action?.Invoke();
    }

    /// <summary>
    /// Camera Zoom
    /// </summary>
    /// <param name="zoom">Zoom Mode</param>
    public void Zoom(ZoomMode zoom)
    {
        if (_currentMode != zoom)
            StopAllCoroutines();

        _currentMode = zoom;

        switch (zoom)
        {
            case ZoomMode.Out:
                StartCoroutine(CamLerp(_followCamUpSize));
                break;
            case ZoomMode.In:
                StartCoroutine(CamLerp(_followOriginalSize));
                break;
        }
    }

    /// <summary>
    /// Custom Duration Camera Zoom
    /// </summary>
    /// <param name="zoom">Zoom Mode</param>
    /// <param name="duration">Custom Duration</param>
    public void Zoom(ZoomMode zoom, float duration)
    {
        if (_currentMode != zoom)
            StopAllCoroutines();

        _currentMode = zoom;

        switch (zoom)
        {
            case ZoomMode.Out:
                StartCoroutine(CamLerp(_followCamUpSize, duration));
                break;
            case ZoomMode.In:
                StartCoroutine(CamLerp(_followOriginalSize, duration));
                break;
        }
    }

    /// <summary>
    /// Cam Zoom Lerp
    /// </summary>
    /// <param name="size">Cam Zoom Size</param>
    /// <returns></returns>
    private IEnumerator CamLerp(float size)
    {
        _isZoom = true;

        float time = 0;
        float startValue = _followCam.m_Lens.OrthographicSize;

        while (time < _duration)
        {
            _followCam.m_Lens.OrthographicSize = Mathf.Lerp(startValue, size, time / _duration);

            time += Time.deltaTime;
            yield return null;
        }
        _followCam.m_Lens.OrthographicSize = size;
        _isZoom = false;
    }

    /// <summary>
    /// CustomDuration Lerp
    /// </summary>
    /// <param name="size">Cam Zoom Size</param>
    /// <param name="duration">Cam Zoom Duration</param>
    /// <returns></returns>
    private IEnumerator CamLerp(float size, float duration)
    {
        _isZoom = true;

        float time = 0;
        float startValue = _followCam.m_Lens.OrthographicSize;

        while (time < duration)
        {
            _followCam.m_Lens.OrthographicSize = Mathf.Lerp(startValue, size, time / duration);

            time += Time.deltaTime;
            yield return null;
        }
        _followCam.m_Lens.OrthographicSize = size;
        _isZoom = false;
    }

    public void FollowTarget(Transform target)
    {
        _followCam.Follow = target;
    }

    public void ZoomAndFollow(ZoomMode zoom, float duration = 0, Transform target = null)
    {
        if (_currentMode != zoom)
            StopAllCoroutines();

        _currentMode = zoom;

        if (duration <= 0)
            duration = _duration;

        switch (zoom)
        {
            case ZoomMode.Out:
                StartCoroutine(CamZoomAndFollowLerp(_followCamUpSize, duration, target));
                break;
            case ZoomMode.In:
                StartCoroutine(CamZoomAndFollowLerp(_followOriginalSize, duration, target));
                break;
        }
    }

    private IEnumerator CamZoomAndFollowLerp(float size, float duration, Transform target)
    {
        _isZoom = true;

        float time = 0;
        float startValue = _followCam.m_Lens.OrthographicSize;

        FollowTarget(target);

        while (time < duration)
        {
            _followCam.m_Lens.OrthographicSize = Mathf.Lerp(startValue, size, time / duration);

            time += Time.deltaTime;
            yield return null;
        }
        _followCam.m_Lens.OrthographicSize = size;
        _isZoom = false;
    }
}
