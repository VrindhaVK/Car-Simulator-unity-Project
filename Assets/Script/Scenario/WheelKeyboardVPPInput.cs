using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using VehiclePhysics;
using VehiclePhysics.UI;

[DisallowMultipleComponent]
public class WheelKeyboardVPPInput : MonoBehaviour
{
    [Header("Vehicle")]
    public VehicleBase vehicle;

    [Header("Keyboard Backup")]
    public KeyCode throttleKey = KeyCode.W;
    public KeyCode brakeKey = KeyCode.S;
    public KeyCode steerLeftKey = KeyCode.A;
    public KeyCode steerRightKey = KeyCode.D;
    public KeyCode handbrakeKey = KeyCode.Space;
    public KeyCode ignitionKey = KeyCode.Return;

    [Header("Keyboard Gears")]
    public KeyCode reverseGearKey = KeyCode.R;
    public KeyCode neutralGearKey = KeyCode.N;
    public KeyCode driveGearKey = KeyCode.F;

    [Header("HORI Wheel Buttons")]
    public string startEngineButton = "buttonSouth";   // usually X
    public string stopEngineButton = "buttonWest";     // usually Square
    public string handbrakeButton = "buttonEast";      // usually Circle
    public float buttonPressPoint = 0.5f;

    [Header("HORI Wheel / Pedals")]
    public bool useInputSystemFallback = true;
    public string preferredWheelDeviceName = "HORI";
    public bool invertSteerAxis = false;
    public bool invertThrottleAxis = false;
    public bool invertBrakeAxis = false;
    [Range(0f, 0.5f)] public float axisDeadZone = 0.05f;

    [Header("Vehicle Start")]
    public bool startWithIgnitionOn = true;
    public bool startInDrive = true;

    [Header("Debug - Turn OFF for final VR demo")]
    public bool showInputDebugOnStart = true;
    public bool showInputTestChecklist = true;
    public KeyCode toggleInputDebugKey = KeyCode.F9;

    private bool ignitionOn;
    private bool showInputDebug;
    private InputDevice inputSystemWheel;

    private bool startButtonWasHeld;
    private bool stopButtonWasHeld;
    private bool handbrakeButtonWasHeld;

    public float LastSteerInput { get; private set; }
    public float LastThrottleInput { get; private set; }
    public float LastBrakeInput { get; private set; }
    public float LastHandbrakeInput { get; private set; }
    public string ActiveWheelDeviceLabel { get; private set; } = "None";

    void Awake()
    {
        if (vehicle == null)
            vehicle = GetComponent<VehicleBase>();

        ignitionOn = startWithIgnitionOn;
        showInputDebug = showInputDebugOnStart;

        ApplyAutomaticGearbox();

        if (startInDrive)
            SetAutomaticGear(4);
    }

    void Update()
    {
        if (vehicle == null)
            return;

        if (Input.GetKeyDown(toggleInputDebugKey))
            showInputDebug = !showInputDebug;

        HandleIgnition();
        HandleGearKeys();

        ReadWheelInputs(out float wheelSteer, out float wheelThrottle, out float wheelBrake, out float wheelHandbrake);

        float keyboardSteer = ReadKeyboardSteer();
        float keyboardThrottle = Input.GetKey(throttleKey) ? 1f : 0f;
        float keyboardBrake = Input.GetKey(brakeKey) ? 1f : 0f;
        float keyboardHandbrake = Input.GetKey(handbrakeKey) ? 1f : 0f;

        float steer = Mathf.Abs(wheelSteer) > axisDeadZone ? wheelSteer : keyboardSteer;
        float throttle = Mathf.Max(wheelThrottle, keyboardThrottle);
        float brake = Mathf.Max(wheelBrake, keyboardBrake);
        float handbrake = Mathf.Max(wheelHandbrake, keyboardHandbrake);

        LastSteerInput = steer;
        LastThrottleInput = throttle;
        LastBrakeInput = brake;
        LastHandbrakeInput = handbrake;

        SetInput(InputData.Steer, steer);
        SetInput(InputData.Throttle, throttle);
        SetInput(InputData.Brake, brake);
        SetInput(InputData.Handbrake, handbrake);
        SetInput(InputData.Clutch, 0f);

        vehicle.data.Set(Channel.Input, InputData.Key, ignitionOn ? 1 : -1);
    }

    void HandleIgnition()
    {
        if (Input.GetKeyDown(ignitionKey))
            ignitionOn = !ignitionOn;

        if (IsInputSystemButtonDown(startEngineButton, ref startButtonWasHeld))
        {
            ignitionOn = true;
            SetAutomaticGear(4);
        }

        if (IsInputSystemButtonDown(stopEngineButton, ref stopButtonWasHeld))
        {
            ignitionOn = false;
            SetAutomaticGear(3);
        }
    }

    void HandleGearKeys()
    {
        if (Input.GetKeyDown(reverseGearKey))
            SetAutomaticGear(2);

        if (Input.GetKeyDown(neutralGearKey))
            SetAutomaticGear(3);

        if (Input.GetKeyDown(driveGearKey))
            SetAutomaticGear(4);
    }

    void ApplyAutomaticGearbox()
    {
        if (vehicle == null) return;

        VPVehicleController controller = vehicle.GetComponent<VPVehicleController>();

        if (controller != null)
        {
            controller.gearbox.type = Gearbox.Type.Automatic;
            vehicle.data.Set(Channel.Settings, SettingsData.AutoShiftOverride, 0);
        }
    }

    void SetAutomaticGear(int gear)
    {
        if (vehicle == null) return;
        vehicle.data.Set(Channel.Input, InputData.AutomaticGear, Mathf.Clamp(gear, 0, 5));
    }

    float ReadKeyboardSteer()
    {
        float steer = 0f;

        if (Input.GetKey(steerLeftKey))
            steer -= 1f;

        if (Input.GetKey(steerRightKey))
            steer += 1f;

        return steer;
    }

    void ReadWheelInputs(out float steer, out float throttle, out float brake, out float handbrake)
    {
        steer = 0f;
        throttle = 0f;
        brake = 0f;
        handbrake = 0f;

        if (!useInputSystemFallback)
        {
            ActiveWheelDeviceLabel = "Input System disabled";
            return;
        }

        InputDevice device = ResolveWheelDevice();

        if (device == null)
        {
            ActiveWheelDeviceLabel = "No HORI wheel detected";
            return;
        }

        ActiveWheelDeviceLabel = device.displayName;

        if (device is Gamepad gamepad)
        {
            steer = gamepad.leftStick.x.ReadValue();
            throttle = gamepad.rightTrigger.ReadValue();
            brake = gamepad.leftTrigger.ReadValue();

            if (invertSteerAxis)
                steer = -steer;

            if (invertThrottleAxis)
                throttle = 1f - throttle;

            if (invertBrakeAxis)
                brake = 1f - brake;

            if (Mathf.Abs(steer) < axisDeadZone)
                steer = 0f;

            if (throttle < axisDeadZone)
                throttle = 0f;

            if (brake < axisDeadZone)
                brake = 0f;

            if (IsInputSystemButtonHeld(handbrakeButton))
                handbrake = 1f;

            return;
        }

        AxisControl steerAxis = device.TryGetChildControl<AxisControl>("x");

        if (steerAxis != null)
        {
            steer = steerAxis.ReadValue();

            if (invertSteerAxis)
                steer = -steer;

            if (Mathf.Abs(steer) < axisDeadZone)
                steer = 0f;
        }
    }

    InputDevice ResolveWheelDevice()
    {
        if (inputSystemWheel != null && inputSystemWheel.added)
            return inputSystemWheel;

        inputSystemWheel = null;

        string preferred = preferredWheelDeviceName.ToLowerInvariant();

        foreach (InputDevice device in InputSystem.devices)
        {
            if (device == null || !device.added)
                continue;

            string id = $"{device.displayName} {device.name} {device.layout}".ToLowerInvariant();

            if (id.Contains("quest") || id.Contains("oculus") || id.Contains("meta") || id.Contains("touch"))
                continue;

            if (!string.IsNullOrEmpty(preferred) && id.Contains(preferred))
            {
                inputSystemWheel = device;
                Debug.Log($"HORI wheel detected: {device.displayName} / {device.layout}");
                return device;
            }

            if (id.Contains("hori") || id.Contains("racing wheel") || id.Contains("wheel"))
            {
                inputSystemWheel = device;
                Debug.Log($"Wheel detected: {device.displayName} / {device.layout}");
                return device;
            }

            if (device is Gamepad && inputSystemWheel == null)
            {
                inputSystemWheel = device;
            }
        }

        return inputSystemWheel;
    }

    bool IsInputSystemButtonDown(string controlName, ref bool wasHeld)
    {
        if (string.IsNullOrWhiteSpace(controlName))
        {
            wasHeld = false;
            return false;
        }

        InputDevice device = ResolveWheelDevice();

        if (device == null)
        {
            wasHeld = false;
            return false;
        }

        ButtonControl button = device.TryGetChildControl<ButtonControl>(controlName.Trim());

        if (button == null)
        {
            wasHeld = false;
            return false;
        }

        bool held = button.ReadValue() >= buttonPressPoint;
        bool down = held && !wasHeld;

        wasHeld = held;
        return down;
    }

    bool IsInputSystemButtonHeld(string controlName)
    {
        if (string.IsNullOrWhiteSpace(controlName))
            return false;

        InputDevice device = ResolveWheelDevice();

        if (device == null)
            return false;

        ButtonControl button = device.TryGetChildControl<ButtonControl>(controlName.Trim());

        if (button == null)
            return false;

        return button.ReadValue() >= buttonPressPoint;
    }

    void SetInput(int input, float value)
    {
        if (vehicle == null) return;

        vehicle.data.Set(
            Channel.Input,
            input,
            Mathf.RoundToInt(Mathf.Clamp(value, -1f, 1f) * 10000f)
        );
    }

    void OnGUI()
    {
        if (!showInputDebug)
            return;

        GUI.Box(new Rect(20, 20, 520, 190), "");

        GUI.Label(new Rect(35, 35, 500, 25), "HORI Wheel / VPP Input Test");
        GUI.Label(new Rect(35, 60, 500, 25), $"Device: {ActiveWheelDeviceLabel}");
        GUI.Label(new Rect(35, 85, 500, 25), $"Steer: {LastSteerInput:0.00}");
        GUI.Label(new Rect(35, 110, 500, 25), $"Throttle: {LastThrottleInput:0.00}");
        GUI.Label(new Rect(35, 135, 500, 25), $"Brake: {LastBrakeInput:0.00}");
        GUI.Label(new Rect(35, 160, 500, 25), $"Handbrake: {LastHandbrakeInput:0.00}");
    }
}