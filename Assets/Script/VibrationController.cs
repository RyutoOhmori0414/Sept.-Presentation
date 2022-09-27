using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VibrationController : MonoBehaviour
{
    /// <summary>
    /// �o�C�u���[�V�������X�^�[�g������
    /// </summary>
    public void StartVibration()
    {
        StartCoroutine(Vibration());
    }
    IEnumerator Vibration()
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
