using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour
{
    

    private void Awake()
    {
        Time.timeScale = 0;
        
    }

    public void PlayButton(GameObject _menuPanel)
    {
        _menuPanel.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Instructions(GameObject _menuPanel)
    {
        _menuPanel.SetActive(true);
    }
    public void MainMenu(GameObject _menuPanel)
    {
        _menuPanel.SetActive(false);
    }

    public void EndGame()
    {
        SceneManager.LoadScene("TafeScene");
    }

    public void QuitButton()
    {
        Application.Quit();
    }


}
