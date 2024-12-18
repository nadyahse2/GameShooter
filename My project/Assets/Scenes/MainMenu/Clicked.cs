using UnityEngine.SceneManagement;
using UnityEngine;

public class Clicked : MonoBehaviour
{

    private string NextScene;
    public void OnCliked(string Name)
    {

        if (Name != "")
        {
            NextScene = Name;
            LoadNextScene();
        }
    }
    
    private void LoadNextScene()
    {
        SceneManager.LoadScene(NextScene);
    }

    public void QuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public GameObject PausePanel;

    public void OpenMenu()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ContinueButtonPressed()
    {
        
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PausePanel.SetActive(false);
        this.GetComponentInParent<Player>().Panel_Active = false;
    }
    public void RestartButtonPressed()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void BackToMenuButtonPressed()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Levels");
    }
}
