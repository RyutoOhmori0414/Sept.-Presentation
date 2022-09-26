using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VibrationController : MonoBehaviour
{
    public IEnumerator Vibration()
    {
        //ゲームパッドが繋がっていなかったらbreak
        if (Gamepad.current == null)
        {
            yield break;
        }
        //振動！！
        Gamepad.current.SetMotorSpeeds(1f, 1f);

        yield return new WaitForSeconds(0.5f);

        Gamepad.current.SetMotorSpeeds(0f, 0f);
    }
}
