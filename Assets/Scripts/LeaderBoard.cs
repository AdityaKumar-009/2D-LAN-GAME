using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderBoard : MonoBehaviour
{

    public void Exit()
    {
        Debug.Log("Main Menu Btn Clicked!");
        SceneManager.LoadScene("Menu");
    }

}