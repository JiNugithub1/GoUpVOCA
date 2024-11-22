using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackgroundMusicManager : MonoBehaviour
{
    private static BackgroundMusicManager instance = null;
    private AudioSource audioSource;
    private bool isMusicOn = true;

    public Image musicToggleButtonImage;
    public Sprite musicOnIcon;
    public Sprite musicOffIcon;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);  // 씬 전환 간에도 오브젝트를 유지합니다.

        audioSource = GetComponent<AudioSource>();

        // 처음 씬이 로드될 때 버튼 초기화
        UpdateMusicButtonReference();
    }

    void Start()
    {
        // 게임 시작 시 버튼 상태 업데이트
        UpdateMusicIcon();
    }

    void OnEnable()
    {
        // 씬이 로드될 때마다 SceneLoaded 이벤트 호출
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // 오브젝트가 비활성화될 때 이벤트 제거
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 로드될 때마다 버튼 참조를 업데이트
        UpdateMusicButtonReference();
    }

    void UpdateMusicButtonReference()
    {
        // 현재 씬에서 'MusicToggleButton' 오브젝트 찾기
        if (musicToggleButtonImage == null) // 버튼이 아직 초기화되지 않았다면
        {
            GameObject buttonObject = GameObject.Find("MusicToggleButton");
            if (buttonObject != null)
            {
                musicToggleButtonImage = buttonObject.GetComponent<Image>();

                // 버튼 클릭 이벤트 추가
                Button toggleButton = buttonObject.GetComponent<Button>();
                toggleButton.onClick.RemoveAllListeners();
                toggleButton.onClick.AddListener(ToggleMusic);

                UpdateMusicIcon();
            }
        }
    }

    public void ToggleMusic()
    {
        if (isMusicOn)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.Play();
        }
        isMusicOn = !isMusicOn;

        UpdateMusicIcon();
    }

    private void UpdateMusicIcon()
    {
        if (musicToggleButtonImage != null)
        {
            musicToggleButtonImage.sprite = isMusicOn ? musicOnIcon : musicOffIcon;
        }
    }
}
