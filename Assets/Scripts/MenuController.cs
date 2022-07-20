using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour
{
    public Slider volumeSlider;
    public Canvas menu;

    private bool cursorVisible;
    private CursorLockMode lockMode;

    private static MenuController _instance;
    public static MenuController Instance
    {
        get { return _instance; }
    }
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void SetVolume(System.Single volume)
    {
        AudioListener.volume = volume;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menu.isActiveAndEnabled)
            {
                HideMenu();
            }
            else
            {
                ShowMenu();
            }
        }
    }

    public void ShowMenu()
    {
        lockMode = Cursor.lockState;
        cursorVisible = Cursor.visible;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        menu.gameObject.SetActive(true);
    }

    public void HideMenu()
    {
        Cursor.lockState = lockMode;
        Cursor.visible = cursorVisible;
        menu.gameObject.SetActive(false);
    }

    public void ToTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Resume()
    {
        HideMenu();
    }

}
