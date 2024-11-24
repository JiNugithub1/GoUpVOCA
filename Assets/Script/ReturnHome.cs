using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnHome : MonoBehaviour
{
    public AudioSource Buttonsound; // 버튼 클릭 소리를 재생할 AudioSource
    // Start is called before the first frame update
    public void BackToMainMenu()
    {
        StartCoroutine(PlayButtonClickAndLoadScene("DifficultySelection")); // 코루틴을 통해 소리 재생 후 초기 화면으로 전환
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
