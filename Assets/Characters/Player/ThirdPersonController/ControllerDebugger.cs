using System;
using UnityEngine;

public class ControllerDebugger : MonoBehaviour
{
    [SerializeField] bool debugLogMode;

    Vector2 leftStickInput = Vector2.zero;
    Vector2 rightStickInput = Vector2.zero;

    Vector2 dPadInput = Vector2.zero;
    [SerializeField] bool dPadOneTouchOneOut;
    bool leftDown, rightDown, upDown, downDown = false;

    bool focused = false;
    float analogL2 = -1;
    float analogR2 = -1;

    private void Start()
    {
        focused = false;
    }

    void Update()
    {
        if (focused)
        {
            if (Input.GetButtonDown("Cross"))
            {
                if (debugLogMode) Debug.Log("Cross");
            }
            if (Input.GetButtonDown("Square"))
            {
                if (debugLogMode) Debug.Log("Square");
            }
            if (Input.GetButtonDown("Circle"))
            {
                if (debugLogMode) Debug.Log("Circle");
            }
            else if (Input.GetButtonDown("Triangle"))
            {
                if (debugLogMode) Debug.Log("Triangle");
            }
            if (Input.GetButtonDown("R1"))
            {
                if (debugLogMode) Debug.Log("R1");
            }
            if (Input.GetButtonDown("L1"))
            {
                if (debugLogMode) Debug.Log("L1");
            }
            if (Input.GetButtonDown("L3"))
            {
                if (debugLogMode) Debug.Log("L3");
            }
            if (Input.GetButtonDown("R3"))
            {
                if (debugLogMode) Debug.Log("R3");
            }
            if (Input.GetButtonDown("Share"))
            {
                if (debugLogMode) Debug.Log("Share");
            }
            if (Input.GetButtonDown("Options"))
            {
                if (debugLogMode) Debug.Log("Start");
            }
            if (Input.GetButtonDown("PS"))
            {
                if (debugLogMode) Debug.Log("PS");
            }
            if (Input.GetButtonDown("TouchPad"))
            {
                if (debugLogMode) Debug.Log("TouchPad");
            }

            leftStickInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (Mathf.Abs(leftStickInput.x) > 0 || Mathf.Abs(leftStickInput.y) > 0)
            {
                if (debugLogMode) Debug.Log("Left Stick: " + leftStickInput);
            }
            rightStickInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            if (Mathf.Abs(rightStickInput.x) > 0 || Mathf.Abs(rightStickInput.y) > 0)
            {
                if (debugLogMode) Debug.Log("Right Stick: " + rightStickInput);
            }

            DpadInputs();

            analogL2 = Input.GetAxis("L2");
            if (analogL2 > -1)
            {
                if (debugLogMode && Application.isFocused) Debug.Log("L2: " + Math.Round(analogL2, 2));
            }
            analogR2 = Input.GetAxis("R2");
            if (analogR2 > -1)
            {
                if (debugLogMode && Application.isFocused) Debug.Log("R2: " + Math.Round(analogR2, 2));
            }
        }
    }

    void DpadInputs()
    {
        dPadInput = new Vector2(Input.GetAxis("Dpad X"), Input.GetAxis("Dpad Y"));
        if (dPadInput.y > 0)
        {
            if (!dPadOneTouchOneOut)
            {
                if (debugLogMode) Debug.Log("Dpad Up");
            }
            else
            {
                downDown = false;
                if (!upDown)
                {
                    upDown = true;
                    if (debugLogMode) Debug.Log("Dpad Up");
                }
            }

        }
        else if (dPadInput.y < 0)
        {
            if (!dPadOneTouchOneOut)
            {
                if (debugLogMode) Debug.Log("Dpad Down");
            }
            else
            {
                upDown = false;
                if (!downDown)
                {
                    downDown = true;
                    if (debugLogMode) Debug.Log("Dpad Down");
                }
            }
        }
        else
        {
            upDown = false;
            downDown = false;
        }

        if (dPadInput.x > 0)
        {
            if (!dPadOneTouchOneOut)
            {
                if (debugLogMode) Debug.Log("Dpad Right");
            }
            else
            {
                leftDown = false;
                if (!rightDown)
                {
                    rightDown = true;
                    if (debugLogMode) Debug.Log("Dpad Right");
                }
            }
        }
        else if (dPadInput.x < 0)
        {
            if (!dPadOneTouchOneOut)
            {
                if (debugLogMode) Debug.Log("Dpad Left");
            }
            else
            {
                rightDown = false;
                if (!leftDown)
                {
                    leftDown = true;
                    if (debugLogMode) Debug.Log("Dpad Left");
                }
            }
        }
        else
        {
            leftDown = false;
            rightDown = false;
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            focused = true;
        }
        else
        {
            focused = false;
        }
    }
}
