using UnityEngine;
using UnityEngine.InputSystem;

public class GamePad : MonoBehaviour
{
    void Update()
    {
        // �Q�[���p�b�h���ڑ�����Ă��Ȃ���null�ɂȂ�B
        if (Gamepad.current == null)
        {
            return;
        }
        if (Gamepad.current.buttonEast.wasPressedThisFrame)
        {
            Debug.Log("Button North�������ꂽ�I");
            Gamepad.current.SetMotorSpeeds(0f, 1.0f);
        }

        if (Gamepad.current.buttonEast.wasReleasedThisFrame)
        {
            Debug.Log("Button North�������ꂽ�I");
            Gamepad.current.SetMotorSpeeds(0f, 0f);
        }

        if (Gamepad.current.buttonWest.wasPressedThisFrame)
        {
            Debug.Log("Button North�������ꂽ�I");
            Gamepad.current.SetMotorSpeeds(1.0f, 0f);
        }

        if (Gamepad.current.buttonWest.wasReleasedThisFrame)
        {
            Debug.Log("Button North�������ꂽ�I");
            Gamepad.current.SetMotorSpeeds(0f, 0f);
        }

        if (Gamepad.current.buttonSouth.wasReleasedThisFrame)
        {
            Debug.Log("Button South�������ꂽ�I");
        }
    }

    void OnGUI()
    {
        if (Gamepad.current == null) return;

        GUILayout.Label($"leftStick: {Gamepad.current.leftStick.ReadValue()}");
        GUILayout.Label($"buttonNorth: {Gamepad.current.buttonNorth.isPressed}");
        GUILayout.Label($"buttonSouth: {Gamepad.current.buttonSouth.isPressed}");
        GUILayout.Label($"buttonEast: {Gamepad.current.buttonEast.isPressed}");
        GUILayout.Label($"buttonWest: {Gamepad.current.buttonWest.isPressed}");
        GUILayout.Label($"leftShoulder: {Gamepad.current.leftShoulder.ReadValue()}");
        GUILayout.Label($"leftTrigger: {Gamepad.current.leftTrigger.ReadValue()}");
        GUILayout.Label($"rightShoulder: {Gamepad.current.rightShoulder.ReadValue()}");
        GUILayout.Label($"rightTrigger: {Gamepad.current.rightTrigger.ReadValue()}");
    }
}
