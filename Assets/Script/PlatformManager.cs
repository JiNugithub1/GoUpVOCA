using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public GameObject platformPrefab; // 발판 프리팹
    public int numberOfPlatforms = 10; // 생성할 발판 개수
    public float platformSpacing = 1.5f; // 발판 간의 거리
    public float zigzagOffset = 1.0f; // x축 지그재그 간격

    private List<Vector3> platformPositions = new List<Vector3>();

    void Start()
    {
        GeneratePlatforms();
    }

    void GeneratePlatforms()
    {
        Vector3 startPos = new Vector3(0, -4, 0); // 시작 위치
        bool isLeft = true; // 지그재그 방향을 결정하는 플래그

        for (int i = 0; i < numberOfPlatforms; i++)
        {
            // x축은 왼쪽(-zigzagOffset)과 오른쪽(+zigzagOffset)으로 번갈아 배치
            float xOffset = isLeft ? -zigzagOffset : zigzagOffset;
            Vector3 newPosition = startPos + new Vector3(xOffset, i * platformSpacing, 0);

            // 발판 생성 및 위치 저장
            Instantiate(platformPrefab, newPosition, Quaternion.identity);
            platformPositions.Add(newPosition);

            // 방향 토글
            isLeft = !isLeft;
        }
    }

    public List<Vector3> GetPlatformPositions()
    {
        return platformPositions;
    }
}
