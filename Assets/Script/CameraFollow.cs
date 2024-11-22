using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 따라갈 대상 (고양이)
    public float smoothSpeed = 0.125f; // 카메라 이동의 부드러움
    public Vector3 offset; // 카메라와 대상 사이의 거리

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogError("CameraFollow: Target is not assigned!");
            return;
        }

        // 목표 위치 계산 (고양이 위치 + 오프셋)
        Vector3 desiredPosition = target.position + offset;

        // Z축 고정
        desiredPosition.z = transform.position.z;

        // 부드럽게 카메라 이동
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 카메라 위치 갱신
        transform.position = smoothedPosition;
    }

}
