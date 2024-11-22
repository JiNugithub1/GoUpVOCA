using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public Text difficultyText; // 난이도를 표시할 Text UI
    public Text scoreText;      // 점수를 표시할 Text UI
    public AudioSource Buttonsound; // 버튼 클릭 소리를 재생할 AudioSource

    void Start()
    {
        // PlayerPrefs에서 저장된 난이도와 점수를 가져옴
        string selectedDifficulty = PlayerPrefs.GetString("Difficulty", "Easy"); // 저장된 난이도 가져오기
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0); // 저장된 점수 가져오기

        difficultyText.text = "난이도: " + selectedDifficulty; // 난이도 표시
        scoreText.text = "최종 점수: " + finalScore;           // 점수 표시
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
