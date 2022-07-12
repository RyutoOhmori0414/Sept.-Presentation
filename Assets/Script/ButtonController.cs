using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonController : MonoBehaviour
{
    [SerializeField] float _movePower = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current == null)
        {
            return;
        }
    }

    public void MoveController()
    {
        Gamepad.current.SetMotorSpeeds(_movePower, _movePower);
        Invoke("MotorStop", 2f);
    }

    void MotorStop()
    {
        Gamepad.current.SetMotorSpeeds(0, 0);
    }
}
