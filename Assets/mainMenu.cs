using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenuPrefab;
    [SerializeField] GameObject mainMenuFirstHighlighted;

    [SerializeField] GameObject controlsMenu;
    [SerializeField] GameObject controlsFirstHighlighted;

    [SerializeField] CanvasGroup fadeObj;
    bool fade;

    private void Start()
    {
        SetNewStartPoint(mainMenuFirstHighlighted);
    }

    private void Update()
    {
        if (fade)
        {
            fadeObj.alpha = Mathf.Lerp(fadeObj.alpha, 1.1f, Time.deltaTime * 2);
            if (fadeObj.alpha >= 1)
            {
                SceneManager.LoadScene("Demo 1");
            }
        }
    }

    public void SetNewStartPoint(GameObject ga)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(ga);
    }

    public void NewGame()
    {
        SaveSystem.DeletePlayer();

        fade = true;
    }

    public void LoadGame()
    {
        fade = true;
    }

    public void Controls()
    {
        if (!controlsMenu.activeSelf)
        {
            //mainMenuPrefab.SetActive(false);
            controlsMenu.SetActive(true);
            SetNewStartPoint(controlsFirstHighlighted);
        }
        else
        {
            controlsMenu.SetActive(false);
            //mainMenuPrefab.SetActive(true);
            SetNewStartPoint(mainMenuFirstHighlighted);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
