using UnityEngine;

public class BackgroundRepeater : MonoBehaviour
{
    public Transform player; // 카메라나 플레이어를 따라갈 대상
    public float speed = 2f; // 배경 이동 속도
    public float backgroundWidth; // 배경의 너비

    private Vector3 startPosition; // 배경의 시작 위치
    private float offset; // 반복을 위한 오프셋

    void Start()
    {
        // 플레이어가 할당되지 않았다면, 자동으로 카메라를 할당
        if (player == null)
        {
            player = Camera.main.transform;
        }

        // 배경의 너비 계산 (SpriteRenderer를 통해 자동으로 너비 가져오기)
        backgroundWidth = GetComponent<SpriteRenderer>().bounds.size.x;

        // 배경의 시작 위치 저장
        startPosition = transform.position;
    }

    void Update()
    {
        // 배경을 움직이는 함수 호출
        MoveBackground();
    }

    void MoveBackground()
    {
        // 배경의 이동 (카메라 또는 플레이어의 위치에 따라)
        transform.position = new Vector3(startPosition.x + player.position.x * speed, startPosition.y, startPosition.z);

        // 배경이 화면을 벗어나면 위치를 다시 조정
        if (transform.position.x <= player.position.x - backgroundWidth)
        {
            // 배경을 맨 앞쪽으로 옮겨서 반복
            RepositionBackground();
        }
    }

    void RepositionBackground()
    {
        // 배경이 끝을 지나면 시작 위치로 재배치
        transform.position = new Vector3(player.position.x + backgroundWidth, startPosition.y, startPosition.z);
    }
}
