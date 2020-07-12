using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    public List<JSLController> controllers = new List<JSLController>();

    // Start is called before the first frame update
    void Start()
    {
        ConnectDevices();
    }

    [ContextMenu("Connect Devices")]
    public void ConnectDevices()
    {
        int numDevices = JSL.JslConnectDevices();
        Debug.Log(numDevices + " Joysticks Connected");
        if (numDevices > 0)
        {
            int[] handles = new int[numDevices];
            JSL.JslGetConnectedDeviceHandles(handles, numDevices);
            List<int> unusedHandles = new List<int>();
            foreach (JSLController jslc in controllers)
            {
                unusedHandles.Add(jslc.joystickHandle);
            }
            for (int i = 0; i < handles.Length; i++)
            {
                if (unusedHandles.Contains(handles[i]))
                {
                    unusedHandles.Remove(handles[i]); // we're good no need to make a new controller
                }
                else
                {
                    JSLController newController = new JSLController();
                    newController.Initialize(handles[i]);
                    controllers.Add(newController);
                    JSL.JslStartContinuousCalibration(handles[i]);
                }
            }
            // now remove the disconnected controllers:
            for (int i = 0; i < unusedHandles.Count; i++)
            {
                Debug.Log("Disconnected handle: " + unusedHandles[i]);
                for (int j = controllers.Count - 1; j >= 0; j--)
                {
                    JSLController jslc = controllers[j];
                    if (jslc.joystickHandle == unusedHandles[i])
                    {
                        controllers.Remove(jslc);
                        continue;
                    }
                }
            }
        }
    }

    private void Update()
    {
        foreach (JSLController c in controllers)
        {
            c.Update();
        }
    }

    private void OnDestroy()
    {
        JSL.JslDisconnectAndDisposeAll();
    }
}

[System.Serializable]
public class JSLController
{
    #region Joystick State

    public int joystickHandle = -1;
    public JSL.JOY_SHOCK_STATE lastState;
    public JSL.JOY_SHOCK_STATE currentState;

    #endregion

    #region Setup and Maintenence Functions
    public void Initialize(int handle)
    {
        joystickHandle = handle;
    }

    public void Update()
    {
        // call this once per frame! That way it gets the joy shock mask and can deal with stuff!
        lastState = currentState;
        currentState = JSL.JslGetSimpleState(joystickHandle);
    }
    #endregion

    #region Get Joystick Buttons and Axes
    // pass in something like JSL.ButtonMaskDown to get whether it's pressed!
    public bool GetButton(int buttonMask)
    {
        // if it's pressed this frame
        return (currentState.buttons & buttonMask) != 0;
    }

    public bool GetButtonDown(int buttonMask)
    {
        // if last frame it wasn't pressed but this frame it is
        return (currentState.buttons & buttonMask) != 0 && (lastState.buttons & buttonMask) == 0;
    }

    public bool GetButtonUp(int buttonMask)
    {
        // if last frame it was pressed but this frame it isn't
        return (currentState.buttons & buttonMask) == 0 && (lastState.buttons & buttonMask) != 0;
    }

    public float GetLeftTrigger()
    {
        return currentState.lTrigger;
    }

    public float GetRightTrigger()
    {
        return currentState.rTrigger;
    }

    public Vector2 GetLeftStick()
    {
        return new Vector2(currentState.stickLX, currentState.stickLY);
    }

    public Vector2 GetRightStick()
    {
        return new Vector2(currentState.stickRX, currentState.stickRY);
    }

    public JSL.IMU_STATE GetGyroState()
    {
        return JSL.JslGetIMUState(joystickHandle);
    }

    public JSL.JOY_SHOCK_STATE GetJoyShockState()
    {
        return JSL.JslGetSimpleState(joystickHandle);
    }
    #endregion // get joystick buttons and axes
}