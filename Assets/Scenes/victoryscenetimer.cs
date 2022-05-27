using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class victoryscenetimer : MonoBehaviour
{

    float timer;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 30 || Input.GetButtonDown("Cross") || Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
