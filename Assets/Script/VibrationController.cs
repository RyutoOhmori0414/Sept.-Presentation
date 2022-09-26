using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VibrationController : MonoBehaviour
{
    public IEnumerator Vibration()
    {
        //�Q�[���p�b�h���q�����Ă��Ȃ�������break
        if (Gamepad.current == null)
        {
            yield break;
        }
        //�U���I�I
        Gamepad.current.SetMotorSpeeds(1f, 1f);

        yield return new WaitForSeconds(0.5f);

        Gamepad.current.SetMotorSpeeds(0f, 0f);
    }
}
