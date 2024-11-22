using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // 코루틴을 사용하기 위해 필요

public class DifficultySelection : MonoBehaviour
{
    
    public AudioSource Buttonsound; // 버튼 클릭 소리를 재생할 AudioSource
    // 이지 난이도 선택 시 호출되는 함수
    public void SelectEasy()
    {
        PlayerPrefs.SetInt("NumberOfAnswers", 3); // Easy 난이도는 3지선다형
        PlayerPrefs.SetString("Difficulty", "Easy"); // 난이도 텍스트 저장
        StartCoroutine(PlayButtonClickAndLoadScene("GameScene", 3)); // 코루틴을 통해 소리 재생 후 씬 전환
    }

    public void SelectNormal()
    {
            PlayerPrefs.SetInt("NumberOfAnswers", 4); // Normal 난이도는 4지선다형
            PlayerPrefs.SetString("Difficulty", "Normal"); // 난이도 텍스트 저장
            StartCoroutine(PlayButtonClickAndLoadScene("GameScene", 4)); // 코루틴을 통해 소리 재생 후 씬 전환
    }

    public void SelectHard()
    {
        PlayerPrefs.SetInt("NumberOfAnswers", 5); // Hard 난이도는 5지선다형
        PlayerPrefs.SetString("Difficulty", "Hard"); // 난이도 텍스트 저장
        StartCoroutine(PlayButtonClickAndLoadScene("GameScene", 5)); // 코루틴을 통해 소리 재생 후 씬 전환
    }

    private IEnumerator PlayButtonClickAndLoadScene(string sceneName, int numberOfAnswers)
    {
        PlayButtonClickSound(); // 클릭 소리 재생
        PlayerPrefs.SetInt("NumberOfAnswers", numberOfAnswers); // 선택된 난이도에 맞게 정답 개수 설정
        yield return new WaitForSeconds(Buttonsound.clip.length); // 소리가 끝날 때까지 대기
        SceneManager.LoadScene(sceneName); // 씬 전환
    }

    public void BackToMainMenu()
    {
        StartCoroutine(PlayButtonClickAndLoadScene("시작화면")); // 코루틴을 통해 소리 재생 후 초기 화면으로 전환
    }

    private IEnumerator PlayButtonClickAndLoadScene(string sceneName)
    {
        PlayButtonClickSound();
        yield return new WaitForSeconds(Buttonsound.clip.length); // 소리가 끝날 때까지 대기
        SceneManager.LoadScene(sceneName); // 씬 전환
    }
     private void PlayButtonClickSound()
    {
        if (Buttonsound != null)
        {
            Buttonsound.PlayOneShot(Buttonsound.clip); // 한 번만 재생
        }
    }
}

