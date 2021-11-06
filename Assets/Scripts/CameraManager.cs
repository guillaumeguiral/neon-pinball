using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Vector3 startPosition;

    Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();

        IntroAnimation();
    }

    void IntroAnimation()
    {
        mainCamera.transform.DOMove(startPosition, 3f).SetEase(Ease.InOutCubic).From();
    }

    public void Tilt(Vector3 direction)
    {
        mainCamera.DOShakePosition(0.25f, direction / 4f);
    }
}
