using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] KinematicPlayerMotor motor = null;
    [SerializeField] Transform sprite = null;
    [SerializeField] Animator anim = null;

    [HideInInspector]
    public bool isPaused = false;

    private void Update()
    {
        if (isPaused)
            return;

        float hor = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");

        motor.MoveInput(new Vector2(hor, 0));

        if (hor > 0)
            sprite.rotation = Quaternion.identity;
        else if (hor < 0)
            sprite.rotation = Quaternion.AngleAxis(180, Vector3.up);

        if (vert == 0)
            sprite.GetChild(0).localRotation = Quaternion.identity;
        else
            sprite.GetChild(0).localRotation = Quaternion.AngleAxis(45 * vert, Vector3.forward);

        if (Input.GetButtonDown("Jump"))
            motor.JumpInput();

        anim.SetBool("moving", hor != 0);
        anim.SetBool("grounded", motor.Grounded);
        if (motor.JumpTrigger())
            anim.SetTrigger("Jump");
        anim.SetFloat("VerSpeed", motor.body.Velocity.y);
    }
}
