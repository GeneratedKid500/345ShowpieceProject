using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lorefadeinout : MonoBehaviour
{
    [SerializeField] CanvasGroup controller;
    [SerializeField] CanvasGroup lore;

    float timer;

    private void Start()
    {
        lore.alpha = 0;
        controller.alpha = 0.2f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetButtonDown("Options") || Input.GetButtonDown("Cross") || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButton(0))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }

        if (timer < 6)
        {
            lore.alpha = 0;

            controller.alpha = Mathf.Lerp(controller.alpha, 1, Time.deltaTime*2);
        }
        else if (timer < 13)
        {
            controller.alpha = Mathf.Lerp(controller.alpha, 0, Time.deltaTime * 3);

            lore.alpha = Mathf.Lerp(lore.alpha, 1, Time.deltaTime * 2);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
}
