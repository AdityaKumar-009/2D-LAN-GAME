using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using UnityEngine.UI;

public class SceneManage : MonoBehaviour
{
    public static SceneManage instance;
    public Button Start;
    public Button Join;
    public Button Exit;

    public int OnBtnClicked;

    private void Awake()
    {
        Start.onClick.AddListener(OnStartClick);
        Join.onClick.AddListener(OnJoinClick);
        Exit.onClick.AddListener(OnQuitButton);

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OnStartClick()
    {
        OnBtnClicked = 0;
        SceneManager.LoadScene("Level");
    }

    public void OnJoinClick()
    {
        OnBtnClicked = 1;
        SceneManager.LoadScene("Level");
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
