using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] GameObject menu;
    GameObject activeWindow;
    GameObject activeFirst;

    [Header("Pause Menu")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject pauseMenuFirstSelected;

    [Header("Character Menu")]
    [SerializeField] GameObject characterMenu;
    [SerializeField] GameObject characterMenuFirstSelected;

    PlayerMainStateManager pmsm;

    [Header("Inputs")]
    [SerializeField] string pauseButton1;
    [SerializeField] string pauseButton2;

    

    void Start()
    {
        pmsm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMainStateManager>();
    }

    void Update()
    {
        if (Input.GetButtonDown(pauseButton1) || Input.GetButtonDown(pauseButton2) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (menu.activeInHierarchy)
            {
                ChangeWindow("Pause");
                UnPause();
            }
            else
            {
                pmsm.paused = true;

                SetNewStartPoint(pauseMenuFirstSelected);
                activeWindow = pauseMenu;

                menu.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    public void SetNewStartPoint(GameObject ga)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(ga);
    }

    public void UnPause()
    {
        pmsm.paused = false;

        menu.SetActive(false);
        Time.timeScale = 1;

        pmsm.RestartAnimator();
    }

    public void ExitGame()
    {
        // save

        Application.Quit();
    }

    public void ChangeWindow(string window)
    {
        activeWindow.SetActive(false);
        switch (window)
        {
            case "Pause":
                activeWindow = pauseMenu;
                activeFirst = pauseMenuFirstSelected;
                break;

            case "Inventory":
                break;

            case "Character":
                activeWindow = characterMenu;
                activeFirst = characterMenuFirstSelected;
                break;

            case "Settings":
                break;

            default:
                break;
        }
        activeWindow.SetActive(true);
        SetNewStartPoint(activeFirst);
    }


}
