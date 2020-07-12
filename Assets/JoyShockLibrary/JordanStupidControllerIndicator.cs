using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JordanStupidControllerIndicator : MonoBehaviour
{
    public int controllerIndex = 0;
    public Transform orientationIndicator;
    public float rotationScalar = 1;
    public float movementScalar = 1;
    public bool cumulativeRotation = false;
    public bool cumulativeMovement = true;
    public int rotCutoff = 1000;

    public ControllerManager jtc;

    // Update is called once per frame
    void Update()
    {
        if (jtc.controllers.Count > controllerIndex)
        {
            // then display the orientation and acceleration of the first one!
            JSL.IMU_STATE imuState = JSL.JslGetIMUState(jtc.controllers[0].joystickHandle);
            Vector3 rot = new Vector3(imuState.gyroX, imuState.gyroY, imuState.gyroZ);
            if (rot.magnitude > rotCutoff)
            {
                if (cumulativeRotation)
                {
                    orientationIndicator.Rotate(imuState.gyroX * rotationScalar, imuState.gyroY * rotationScalar, imuState.gyroZ * rotationScalar);
                }
                else
                {
                    orientationIndicator.rotation = Quaternion.Euler(imuState.gyroX * rotationScalar, imuState.gyroY * rotationScalar, imuState.gyroZ * rotationScalar);
                }
            }
            if (cumulativeMovement)
            {
                orientationIndicator.localPosition += new Vector3(imuState.accelX, imuState.accelY, imuState.accelZ) * movementScalar;
            }
            else
            {
                orientationIndicator.localPosition = new Vector3(imuState.accelX, imuState.accelY, imuState.accelZ) * movementScalar;
            }
            
        }
    }
}
