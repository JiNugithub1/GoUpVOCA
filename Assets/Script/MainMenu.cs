using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환을 위해 필요
using System.Collections; // 코루틴을 사용하기 위해 필요

public class MainMenu : MonoBehaviour
{
    public AudioSource Buttonsound; // 버튼 클릭 소리를 재생할 AudioSource

    // 시작 버튼 클릭 시 호출되는 함수
    public void StartGame()
    {
        StartCoroutine(PlayButtonClickAndLoadScene("DifficultySelection")); // 코루틴을 통해 소리 재생 후 씬 전환
    }

    // 종료 버튼 클릭 시 호출되는 함수
    public void ExitGame()
    {
        StartCoroutine(PlayButtonClickAndQuit()); // 코루틴을 통해 소리 재생 후 게임 종료
    }

    private IEnumerator PlayButtonClickAndLoadScene(string sceneName)
    {
        PlayButtonClickSound();
        yield return new WaitForSeconds(Buttonsound.clip.length); // 소리가 끝날 때까지 대기
        SceneManager.LoadScene(sceneName); // 씬 전환
    }

    private IEnumerator PlayButtonClickAndQuit()
    {
        PlayButtonClickSound();
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
