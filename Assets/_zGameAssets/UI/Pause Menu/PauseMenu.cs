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

    private void Awake()
    {
        DoClothes();
    }

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

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
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

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


        pmsm.RestartAnimator();
    }

    public void ExitGame()
    {
        // save
        SaveDataGatherer sdg = new SaveDataGatherer();
        SaveSystem.SavePlayer(sdg);

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

    void DoClothes()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        Transform player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>().transform;

        SkinnedMeshRenderer[] bodyParts = new SkinnedMeshRenderer[0];
        bodyParts = player.GetComponentsInChildren<SkinnedMeshRenderer>();

        List<SkinnedMeshRenderer> heads = new List<SkinnedMeshRenderer>();
        List<SkinnedMeshRenderer> body = new List<SkinnedMeshRenderer>();
        List<SkinnedMeshRenderer> legs = new List<SkinnedMeshRenderer>();
        List<SkinnedMeshRenderer> feet = new List<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer part in bodyParts)
        {
            if (part.name.Contains("Head"))
            {
                heads.Add(part);
            }
            else if (part.name.Contains("Body"))
            {
                body.Add(part);
            }
            else if (part.name.Contains("Legs"))
            {
                legs.Add(part);
            }
            else if (part.name.Contains("Feet"))
            {
                feet.Add(part);
            }
            part.gameObject.SetActive(false);
        }

        int[] currentBodyParts = new int[4];
        try
        {
            currentBodyParts = data.clothes;

            heads[currentBodyParts[0]].gameObject.SetActive(true);
            body[currentBodyParts[1]].gameObject.SetActive(true);
            legs[currentBodyParts[2]].gameObject.SetActive(true);
            feet[currentBodyParts[3]].gameObject.SetActive(true);
        }
        catch
        {
            currentBodyParts = new int[4] { 0, 0, 0, 0 };

            heads[currentBodyParts[0]].gameObject.SetActive(true);
            body[currentBodyParts[1]].gameObject.SetActive(true);
            legs[currentBodyParts[2]].gameObject.SetActive(true);
            feet[currentBodyParts[3]].gameObject.SetActive(true);
        }

        foreach (SkinnedMeshRenderer parts in bodyParts)
        {
            if (parts.gameObject == null || parts.gameObject.activeSelf == false)
            {
                Destroy(parts.gameObject);
            }
        }
    }


}
