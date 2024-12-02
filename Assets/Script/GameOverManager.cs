using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class GameOverManager : MonoBehaviour
{
    public Text difficultyText; // 난이도를 표시할 Text UI
    public Text scoreText;      // 점수를 표시할 Text UI
    public AudioSource Buttonsound; // 버튼 클릭 소리를 재생할 AudioSource
    public GameObject incorrectQuestionPanel; // 틀린 문제를 보여줄 패널
    public Transform incorrectQuestionContainer; // 틀린 문제들을 배치할 컨테이너
    public Button closeButton; // 패널을 닫을 버튼
    void Start()
    {
        // PlayerPrefs에서 저장된 난이도와 점수를 가져옴
        string selectedDifficulty = PlayerPrefs.GetString("Difficulty", "Easy"); // 저장된 난이도 가져오기
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0); // 저장된 점수 가져오기

        difficultyText.text = "난이도: " + selectedDifficulty; // 난이도 표시
        scoreText.text = "최종 점수: " + finalScore;           // 점수 표시
        
        // 틀린 문제 불러오기
        string incorrectQuestionsJson = PlayerPrefs.GetString("IncorrectQuestions", "");
        if (!string.IsNullOrEmpty(incorrectQuestionsJson))
        {
            // JSON 문자열을 QuestionDataList로 변환
            QuestionDataList incorrectQuestionData = JsonUtility.FromJson<QuestionDataList>(incorrectQuestionsJson);
            DisplayIncorrectQuestions(incorrectQuestionData.questionList); // 틀린 문제 표시
            // 데이터 로드 확인을 위한 Debug.Log 추가
            Debug.Log("Loaded incorrect questions: " + incorrectQuestionData.questionList.Count);

        }
        else
        {
            Debug.Log("No incorrect questions found in PlayerPrefs.");
        }
        // 버튼에 클릭 이벤트 연결
        closeButton.onClick.AddListener(HideIncorrectQuestions);

    }
    // 틀린 문제를 UI에 표시하는 함수
    void DisplayIncorrectQuestions(List<QuestionData> incorrectQuestions)
    {
        // 틀린 문제들을 최대 3개까지만 출력
        int questionIndex = 0;
        foreach (QuestionData question in incorrectQuestions)
        {
            if (questionIndex >= 3) break; // 3개까지만 표시

            // Debug: 각 질문과 정답을 출력
            Debug.Log("Displaying incorrect question: " + question.question);
            Debug.Log("Answers: " + string.Join(", ", question.answers));
            
            // 기존 Text UI에 데이터 설정
            // incorrectQuestionContainer 안에 Text UI가 3개까지 있다고 가정
            Text questionText = incorrectQuestionContainer.GetChild(questionIndex).GetComponent<Text>();

            // Debug: Text UI에 설정되는 내용 확인
            Debug.Log("Setting text for Text UI " + questionIndex);
            questionText.text = "Question: " + question.question + "\nAnswer: " + question.answers[question.correctAnswerIndex];

            questionIndex++;
        }

        // Debug: 전체 출력된 틀린 문제 개수
        Debug.Log("Total incorrect questions displayed: " + questionIndex);
    }


    public void ShowIncorrectQuestions()
    {
        incorrectQuestionPanel.SetActive(true); // 패널을 보이게 함
    }
    public void HideIncorrectQuestions()
    {
        incorrectQuestionPanel.SetActive(false); // 패널을 숨김
    }
    
    public void ReturnToDifficultySelection()
    {
        // 버튼 소리를 재생한 후 난이도 선택 씬으로 돌아가기
        StartCoroutine(PlayButtonClickAndLoadScene("DifficultySelection"));
    }

    public void QuitGame()
    {
        // 버튼 소리를 재생한 후 게임 종료
        StartCoroutine(PlayButtonClickAndQuit());
    }

    private IEnumerator PlayButtonClickAndLoadScene(string sceneName)
    {
        PlayButtonClickSound(); // 버튼 소리 재생
        yield return new WaitForSeconds(Buttonsound.clip.length); // 소리가 끝날 때까지 대기
        SceneManager.LoadScene(sceneName); // 씬 전환
    }

    private IEnumerator PlayButtonClickAndQuit()
    {
        PlayButtonClickSound(); // 버튼 소리 재생
        yield return new WaitForSeconds(Buttonsound.clip.length); // 소리가 끝날 때까지 대기
        Debug.Log("게임이 종료됩니다.");
        Application.Quit(); // 게임 종료
    }

    private void PlayButtonClickSound()
    {
        if (Buttonsound != null)
        {
            Buttonsound.Play(); // 버튼 클릭 소리 재생
        }
    }
}
