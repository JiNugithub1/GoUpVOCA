using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 씬 전환을 위해 필요
using System.Collections.Generic;
using System.Collections; // 코루틴에 필요한 네임스페이스


[System.Serializable]
public class QuestionData
{
    public string question;
    public string[] answers;
    public int correctAnswerIndex;
}

[System.Serializable]
public class QuestionDataList
{
    public List<QuestionData> questionList;
}

public class GameManager : MonoBehaviour
{
    public Text questionText;
    public Text scoreText;
    public Button[] answerButtons;
    public Image[] heartImages; // 하트 이미지 배열로 목숨 UI 관리
    public int remainingLives = 3;
    private int currentQuestionIndex = 0;
    private int score = 0;
    private int currentPlatformIndex = 0;
    private List<Vector3> platformPositions;
    private Animator animator;
    public AudioClip correctAnswerSound; // 맞힌 소리
    public AudioClip incorrectAnswerSound; // 틀린 소리
    public AudioClip gameoverSound;
    public AudioClip gameClearSound;
    private AudioSource audioSource; // AudioSource 컴포넌트

    private List<QuestionData> questions;          // JSON에서 로드한 전체 문제 리스트
    private List<QuestionData> selectedQuestions;  // 난이도에 맞춰 선택된 문제 리스트
    private int numberOfAnswers;                   // 현재 난이도에서의 답안 개수
    
    void Start()
    {
        // PlayerPrefs에서 numberOfAnswers 값을 불러오기 (기본값은 3)
        numberOfAnswers = PlayerPrefs.GetInt("NumberOfAnswers", 3);
        string selectedDifficulty = PlayerPrefs.GetString("Difficulty", "Easy"); // 기본값 Easy
        UpdateLivesUI(); // 목숨 UI 초기화
        LoadQuestionsFromJSON();
        SelectRandomQuestions(); // 난이도에 맞는 문제 개수 선택
        DisplayNextQuestion();
        PlatformManager platformManager = FindObjectOfType<PlatformManager>();
        platformPositions = platformManager.GetPlatformPositions();
        animator = GameObject.Find("JumpCat").GetComponent<Animator>(); // "Player" GameObject에서 Animator를 찾음
        
        // AudioSource 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        // 첫 번째 발판 위치로 초기화
        if (platformPositions.Count > 0)
        {
            GameObject jumpCat = GameObject.Find("JumpCat");
            if (jumpCat != null)
            {
                // 첫 번째 발판 위치 가져오기
                Vector3 platformPosition = platformPositions[0];
                
                // 고양이 위치를 첫 번째 발판 위로 이동하고 Y 오프셋 적용
                Vector3 startPosition = platformPosition + new Vector3(0, 0.8f, -1); // Y 축으로 0.8f 올림
                jumpCat.transform.position = startPosition; // JumpCat 오브젝트 위치 설정

                Debug.Log("JumpCat 초기 위치 설정: " + startPosition);
            }
            else
            {
                Debug.LogError("JumpCat 오브젝트를 찾을 수 없습니다!");
            }
        }
    }
    void SaveGameData()
    {
        PlayerPrefs.SetInt("FinalScore", score); // 최종 점수 저장
        PlayerPrefs.Save(); // PlayerPrefs 저장
    }

    void LoadQuestionsFromJSON()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("questions");
        if (jsonFile != null)
        {
            QuestionDataList questionDataList = JsonUtility.FromJson<QuestionDataList>(jsonFile.text);
            questions = questionDataList.questionList;
        }
        else
        {
            Debug.LogError("Failed to load questions.json");
        }
    }

    void UpdateScoreUI()
    {
        scoreText.text = "점수: " + score;
    }

    void SelectRandomQuestions()
    {
        // 모든 문제 리스트를 랜덤으로 섞음
        if (questions == null || questions.Count == 0)
        {
            Debug.LogError("Question list is empty or null!");
            return;
        }

        // 선택된 질문이 없거나 모든 질문을 다 사용했을 경우 다시 섞기
        if (selectedQuestions == null || selectedQuestions.Count == 0)
        {
            selectedQuestions = new List<QuestionData>(questions); // 전체 질문 리스트 복사
            ShuffleQuestions(selectedQuestions); // 질문 랜덤 섞기
        }

        // 무작위로 문제를 가져옴 (목숨이 끝날 때까지 반복적으로)
        currentQuestionIndex = 0; // 새로 섞인 질문 리스트의 첫 번째 질문부터 시작
    }
    void ShuffleQuestions(List<QuestionData> questionList)
    {
        for (int i = 0; i < questionList.Count; i++)
        {
            int randomIndex = Random.Range(0, questionList.Count);
            QuestionData temp = questionList[i];
            questionList[i] = questionList[randomIndex];
            questionList[randomIndex] = temp;
        }
    }

    void DisplayNextQuestion()
    {
        // 모든 문제를 사용했을 경우 다시 섞어서 새로운 질문 준비
        if (currentQuestionIndex >= selectedQuestions.Count)
        {
            ShuffleQuestions(selectedQuestions);
            currentQuestionIndex = 0; // 다시 첫 번째 문제로 초기화
        }

        // 현재 질문 표시
        QuestionData currentQuestion = selectedQuestions[currentQuestionIndex];
        questionText.text = currentQuestion.question;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < numberOfAnswers && i < currentQuestion.answers.Length)
            {
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].GetComponentInChildren<Text>().text = currentQuestion.answers[i];
                int index = i; // 로컬 변수로 저장
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }

        Debug.Log("Displayed Question: " + currentQuestion.question);
    }

    void UpdateLivesUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].enabled = i < remainingLives; // 남은 목숨만큼 하트 이미지를 활성화
        }
    }

  void OnAnswerSelected(int index)
    {
        if (index == selectedQuestions[currentQuestionIndex].correctAnswerIndex)
        {
            Debug.Log("Correct Answer!");
            score += 10; // 점수 증가
            UpdateScoreUI(); // 점수 UI 업데이트
            JumpToNextPlatform(); // 점프 호출
            // 문제 맞힌 소리 재생
            if (correctAnswerSound != null)
            {
                audioSource.PlayOneShot(correctAnswerSound);
            }
            else
            {
                Debug.LogWarning("CorrectAnswerSound is not assigned!");
            }
            
        }
        else
        {
            remainingLives--; // 목숨 감소
            UpdateLivesUI(); // UI 업데이트

            if (remainingLives <= 0)
            {
                Debug.Log("Game Over!");
                SaveGameData(); // 데이터 저장
                StartCoroutine(PlayGameOverAndLoadScene());
                return;
            }
            else
            {
                Debug.Log("Incorrect Answer. Remaining Lives: " + remainingLives);
                // 문제 틀린 소리 재생
                if (incorrectAnswerSound != null)
                {
                    audioSource.PlayOneShot(incorrectAnswerSound);
                }
                else
                {
                    Debug.LogWarning("IncorrectAnswerSound is not assigned!");
                }
            }
        }

        // 다음 문제로 전환
        currentQuestionIndex++; // 다음 문제로 이동
        DisplayNextQuestion();  // 다음 문제 표시
    }
    private IEnumerator PlayGameOverAndLoadScene()
    {
        if (gameoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(gameoverSound); // 게임 오버 사운드 재생
            yield return new WaitForSeconds(gameoverSound.length); // 사운드 길이만큼 대기
        }

        SceneManager.LoadScene("GameOver"); // GameOver 씬으로 이동
    }
        void JumpToNextPlatform()
    {
        if (currentPlatformIndex < platformPositions.Count - 1)
        {
            currentPlatformIndex++;
            Vector3 targetPosition = platformPositions[currentPlatformIndex];
            // Y 오프셋 추가
            targetPosition += new Vector3(0, 0.8f, -1); // Y 축으로 1 단위 올림
            // 점프 후 위치 이동
            GameObject jumpCat = GameObject.Find("JumpCat"); // JumpCat 오브젝트 참조
            if (jumpCat != null)
            {
                // 점프 애니메이션 재생
                Animator animator = jumpCat.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.SetTrigger("JumpTrigger"); // 점프 트리거 발동
                }
                StartCoroutine(MoveToPosition(jumpCat.transform, targetPosition));
                SpriteRenderer spriteRenderer = jumpCat.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    // 현재 위치와 목표 위치를 비교하여 방향 설정
                    Vector3 currentPosition = jumpCat.transform.position;

                    if (targetPosition.x > currentPosition.x)
                    {
                        // 오른쪽으로 이동 중 (flipX = false)
                        spriteRenderer.flipX = false;
                    }
                    else if (targetPosition.x < currentPosition.x)
                    {
                        // 왼쪽으로 이동 중 (flipX = true)
                        spriteRenderer.flipX = true;
                    }
                }
            }
            else
            {
                Debug.LogError("JumpCat 오브젝트를 찾을 수 없습니다!");
            }
        }
        // GameClear 조건 확인
        if (currentPlatformIndex >= 44 && score >= 440) // 0부터 시작하므로 45개일 때는 index 44
        {
            Debug.Log("GameClear 조건 충족!");
            SaveGameData(); // 데이터 저장
            StartCoroutine(PlayGameClearAndLoadScene()); // 코루틴 호출
        }
    }
    // GameClear 조건 충족 시 실행되는 Coroutine
    IEnumerator PlayGameClearAndLoadScene()
    {
        if (audioSource != null && gameClearSound != null)
        {
            audioSource.PlayOneShot(gameClearSound); // 소리 재생
            yield return new WaitForSeconds(gameClearSound.length); // 소리가 끝날 때까지 대기
        }
        SceneManager.LoadScene("GameFinal"); // GameClear 씬으로 전환
    }


    IEnumerator MoveToPosition(Transform objectTransform, Vector3 targetPosition)
    {
        float moveSpeed = 5f; // 이동 속도
        float jumpHeight = 2f; // 점프 높이
        float elapsedTime = 0f;
        Vector3 startPosition = objectTransform.position;

        // 점프 지속 시간
        float jumpDuration = Vector3.Distance(startPosition, targetPosition) / moveSpeed;
        Animator animator = objectTransform.GetComponent<Animator>(); // Animator 가져오기

        while (elapsedTime < jumpDuration)
        {
            elapsedTime += Time.deltaTime;

            // 부드럽게 이동 (곡선을 그리며 이동)
            float progress = elapsedTime / jumpDuration;
            float height = Mathf.Sin(Mathf.PI * progress) * jumpHeight; // 포물선 효과
            objectTransform.position = Vector3.Lerp(startPosition, targetPosition, progress) + new Vector3(0, height, 0);

            yield return null;
        }

        // 최종 위치를 정확히 설정
        objectTransform.position = targetPosition;
        // 애니메이션 상태를 Idle로 복귀
        if (animator != null)
        {
            animator.SetTrigger("IdleTrigger"); // Idle 상태로 복귀
        }

    }

}
