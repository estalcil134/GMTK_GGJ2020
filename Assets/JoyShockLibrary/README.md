# JoyShockLibrary
The Sony PlayStation DualShock 4, Nintendo Switch JoyCons (used in pairs), and Nintendo Switch Pro Controller have much in common. They have many of the features expected of modern game controllers. They also have an incredibly versatile and underutilised input that their biggest rival (Microsoft's Xbox One controller) doesn't have: the gyro.

My goal with JoyShockLibrary is to enable game developers to support DS4, JoyCons, and Pro Controllers natively in PC games. I've compiled the library for Windows, but it uses platform-agnostic tools, and my hope is that other developers would be able to get it working on other platforms (such as Linux or Mac) without too much trouble.

## Contents
* **[Releases](#releases)**
* **[Reference](#reference)**
  * **[Structs](#structs)**
  * **[Functions](#functions)**
* **[Known and Perceived Issues](#known-and-perceived-issues)**
* **[Credits](#credits)**
* **[Helpful Resources](#helpful-resources)**
* **[License](#license)**

## Releases
The latest version of JoyShockLibrary can always be found [here](https://github.com/JibbSmart/JoyShockLibrary/releases). Included is a 64-bit dll and a 32-bit dll, both for Windows, and JoyShockLibrary.h and JoyShockLibrary.cs for using the dll in C/C++ and C\# (Unity), respectively.

## Reference
*JoyShockLibrary.h* has everything you need to use the library, but here's a breakdown of everything in there.

### Structs
**struct JOY\_SHOCK\_STATE** - This struct contains the state for all the sticks, buttons, and triggers on the controller. If you're just using JoyShockLibrary to be able to use JoyCons, Pro Controllers, and DualShock 4s similarly to how you'd use other devices, this has everything you need to know.
* **int buttons** contains the states of all the controller's buttons with the following masks:
  * ```0x00001``` - d-pad ```up```
  * ```0x00002``` - d-pad ```down```
  * ```0x00004``` - d-pad ```left```
  * ```0x00008``` - d-pad ```right```
  * ```0x00010``` - ```+``` on Nintendo devices, ```Options``` on DS4
  * ```0x00020``` - ```-``` on Nintendo devices, ```Share``` on DS4
  * ```0x00040``` - ```left-stick click``` on Nintendo devices, ```L3``` on DS4
  * ```0x00080``` - ```right-stick click``` on Nintendo devices, ```R3``` on DS4
  * ```0x00100``` - ```L``` on Nintendo devices, ```L1``` on DS4
  * ```0x00200``` - ```R``` on Nintendo devices, ```R1``` on DS4
  * ```0x00400``` - ```ZL``` on Nintendo devices, ```L2``` on DS4
  * ```0x00800``` - ```ZR``` on Nintendo devices, ```R2``` on DS4
  * ```0x01000``` - the South face-button: ```B``` on Nintendo devices, ```⨉``` on DS4
  * ```0x02000``` - the East face-button: ```A``` on Nintendo devices, ```○``` on DS4
  * ```0x04000``` - the West face-button: ```Y``` on Nintendo devices, ```□``` on DS4
  * ```0x08000``` - the North face-button: ```X``` on Nintendo devices, ```△``` on DS4
  * ```0x10000``` - ```Home``` on Nintendo devices, ```PS``` on DS4
  * ```0x20000``` - ```Capture``` on Nintendo devices, ```touchpad click``` on DS4
  * ```0x40000``` - ```SL``` on Nintendo JoyCons
  * ```0x80000``` - ```SR``` on Nintendo JoyCons
* **float lTrigger** - how far has the left trigger been pressed? This will be 1 or 0 on Nintendo devices, which don't have analog triggers
* **float rTrigger** - how far has the right trigger been pressed? This will be 1 or 0 on Nintendo devices, which don't have analog triggers
* **float stickLX, stickLY** - left-stick X axis and Y axis, respectively, from -1 to 1
* **float stickRX, stickRX** - right-stick X axis and Y axis, respectively, from -1 to 1

**struct IMU_STATE** - Each supported device contains an IMU which has a 3-axis accelerometer and a 3-axis gyroscope. IMU\_STATE is where you find that info.
* **float accelX, accelY, accelZ** - accelerometer X axis, Y axis, and Z axis, respectively, in g (g-force).
* **float gyroX, gyroY, gyroZ** - gyroscope angular velocity X, Y, and Z, respectively, in dps (degrees per second), when correctly calibrated.

### Functions

All these functions *should* be thread-safe, and none of them should cause any harm if given the wrong handle. If they do, please report this to me as an isuse.

**int JslConnectDevices()** - Register any connected devices. Returns the number of devices connected, which is helpful for getting the handles for those devices with the next function.

**int JslGetConnectedDeviceHandles(int\* deviceHandleArray, int size)** - Fills the array *deviceHandleArray* of size *size* with the handles for all connected devices, up to the length of the array. Use the length returned by *JslConnectDevices* to make sure you've got all connected devices' handles.

**void JslDisconnectAndDisposeAll()** - Disconnect devices, no longer polling them for input.

**JOY\_SHOCK\_STATE JslGetSimpleState(int deviceId)** - Get the latest button + trigger + stick state for the controller with the given id.

**IMU\_STATE JslGetIMUState(int deviceId)** - Get the latest accelerometer + gyroscope state for the controller with the given id.

**int JslGetButtons(int deviceId)** - Get the latest button state for the controller with the given id. If you want more than just the buttons, it's more efficient to use JslGetSimpleState.

**float JslGetLeftX/JslGetLeftY/JslGetRightX/JslGetRightY(int deviceId)** - Get the latest stick state for the controller with the given id. If you want more than just a single stick axis, it's more efficient to use JslGetSimpleState.

**float JslGetLeftTrigger/JslGetRightTrigger(int deviceId)** - Get the latest trigger state for the controller with the given id. If you want more than just a single trigger, it's more efficient to use JslGetSimpleState.

**float JslGetGyroX/JslGetGyroY/JslGetGyroZ(int deviceId)** - Get the latest angular velocity for a given gyroscope axis. If you want more than just a single gyroscope axis velocity, it's more efficient to use JslGetIMUState.

**float JslGetAccelX/JslGetAccelY/JslGetAccelZ(int deviceId)** - Get the latest acceleration for a given axis. If you want more than just a accelerometer axis, it's more efficient to use JslGetIMUState.

**float JslGetStickStep(int deviceId)** - Different devices use different size data types and different ranges on those data types when reporting stick axes. For some calculations, it may be important to know the limits of the current device and work around them in different ways. This gives the smallest step size between two values for the given device's analog sticks.

**float JslGetTriggerStep(int deviceId)** - Some devices have analog triggers, some don't. For some calculations, it may be important to know the limits of the current device and work around them in different ways. This gives the smallest step size between two values for the given device's triggers, or 1.0 if they're actually just binary inputs.

**float JslGetTriggerStep(int deviceId)** - Some devices have analog triggers, some don't. For some calculations, it may be important to know the limits of the current device and work around them in different ways. This gives the smallest step size between two values for the given device's triggers, or 1.0 if they're actually just binary inputs.

**float JslGetPollRate(int deviceId)** - Different devices report back new information at different rates. For the given device, this gives how many times one would usually expect the device to report back per second.

**void JslResetContinuousCalibration(int deviceId)** - JoyShockLibrary has helpful functions for calibrating the gyroscope by averaging out its input over time. This deletes all calibration data that's been accumulated, if any, this session.

**void JslStartContinuousCalibration(int deviceId)** - Start collecting gyro data, recording the ongoing average and using that to offset gyro output.

**void JslPauseContinuousCalibration(int deviceId)** - Stop collecting gyro data, but don't delete it.

**void JslGetCalibrationOffset(int deviceId, float& xOffset, float& yOffset, float& zOffset)** - Get the calibrated offset value for the given device's gyro. You don't have to use it; all gyro output for this device is already being offset by this vector before leaving JoyShockLibrary.

**void JslSetCalibrationOffset(int deviceId, float xOffset, float yOffset, float zOffset)** - Manually set the calibrated offset value for the given device's gyro.

**void JslSetCallback(void(\*callback)(int, JOY\_SHOCK\_STATE, JOY\_SHOCK\_STATE, IMU\_STATE, IMU\_STATE, float))** - Set a callback function by which JoyShockLibrary can report the current state for each device. This callback will be given the *deviceId* for the reporting device, its current button + trigger + stick state, its previous button + trigger + stick state, its current accelerometer + gyro state, its previous accelerometer + gyro state, and the amount of time since the last report for this device (in seconds).

**int JslGetControllerType(int deviceId)** - What type of controller is this device?
  1. Left JoyCon
  2. Right JoyCon
  3. Switch Pro Controller
  4. DualShock 4

**int JslGetControllerSplitType(int deviceId)** - Is this a half-controller or full? If half, what kind?
  1. Left half
  2. Right half
  3. Full controller

**int JslGetControllerColour(int deviceId)** - Get the colour of the controller. Only Nintendo devices support this. Others will report white.

**void JslSetLightColour(int deviceId, int colour)** - Set the light colour on the given controller. Only DualShock 4s support this. Players will often prefer to be able to disable the light, so make sure to give them that option, but when setting players up in a local multiplayer game, setting the light colour is a useful way to uniquely identify different controllers.

**void JslSetPlayerNumber(int deviceId, int number)** - Set the lights that indicate player number. This only works on Nintendo devices.

**void JslSetRumble(int deviceId, int smallRumble, int bigRumble)** - DualShock 4s have two types of rumble, and they can be set at the same time with different intensities. These can be set from 0 to 255. Nintendo devices support rumble as well, but totally differently. They call it "HD rumble", and it's a great feature, but JoyShockLibrary doesn't yet support it.

## Known and Perceived Issues
### Bluetooth connectivity
JoyShockLibrary doesn't yet support setting rumble and light colour for the DualShock 4 via Bluetooth.

JoyCons and Pro Controllers can only be connected by Bluetooth. Some Bluetooth adapters can't keep up with these devices, resulting in laggy input. This is especially common when more than one device is connected (such as when using a pair of JoyCons). There is nothing JoyShockMapper or JoyShockLibrary can do about this.

### Gyro poll rate on Nintendo devices
The Nintendo devices report every 15ms, but their IMUs actually report every 5ms. Every 15ms report includes the last 3 gyro and accelerometer reports. When creating the latest IMU state for Nintendo devices, JoyShockLibrary averages out those 3 gyro and accelerometer reports, so that it can best include all that information in a sensible format. For things like controlling a cursor on a plane, this should be of little to no consequence, since the result is the same as adding all 3 reports separately over shorter time intervals. But for representing real 3D rotations of the controller, this causes the Nintendo devices to be *slightly* less accurate than they could be, because we're combining 3 rotations in a simplistic way.

In a future version I hope to either combine the 3 rotations in a way that works better in 3D, or to add a way for a single controller event to report several IMU events at the same time.

## Credits
I'm Jibb Smart, and I made JoyShockLibrary.

JoyShockLibrary uses substantial portions of mfosse's [JoyCon-Driver](https://github.com/mfosse/JoyCon-Driver), a [vJoy](http://vjoystick.sourceforge.net/site/) feeder for most communication with Nintendo devices, building on it with info from dekuNukem's [Nintendo Switch Reverse Engineering](https://github.com/dekuNukem/Nintendo_Switch_Reverse_Engineering/) page in order to (for example) unpack all gyro and accelerometer samples from each report.

JoyShockLibrary's DualShock 4 support would likely not be possible without the info available on [PSDevWiki](https://www.psdevwiki.com/ps4/Main_Page) and [Eleccelerator Wiki](http://eleccelerator.com/wiki/index.php?title=DualShock_4). chrippa's [ds4drv](https://github.com/chrippa/ds4drv) was also a handy reference for getting rumble and lights working right away.

This software also relies on and links signal11's [HIDAPI](https://github.com/signal11/hidapi) to connect to USB and Bluetooth devices. Since HIDAPI is linked statically, .objs are included. Since .objs may need to be compiled with the same compiler version as the dll itself, HIDAPI itself is included in a .zip.

## Helpful Resources
* [GyroWiki](http://gyrowiki.jibbsmart.com) - All about good gyro controls for games:
  * Why gyro controls make gaming better;
  * How developers can do a better job implementing gyro controls;
  * How to use JoyShockLibrary;
  * How gamers can play any PC game with gyro controls using [JoyShockMapper](https://github.com/JibbSmart/JoyShockMapper).

## License
JoyShockLibrary is licensed under the MIT License - see [LICENSE.md](LICENSE.md).
