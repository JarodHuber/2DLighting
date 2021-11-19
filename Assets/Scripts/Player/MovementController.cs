using UnityEngine;
using NaughtyAttributes;

public class MovementController : MonoBehaviour
{
    [SerializeField] KinematicPlayerMotor motor = null;
    [SerializeField] Transform sprite = null;
    [SerializeField] Animator anim = null;

    [HideInInspector]
    public bool isPaused = false;

    [SerializeField, ReadOnly]
    private bool onLadder = false;

    private void Update()
    {
        if (isPaused)
        {
            if (motor.body.Velocity != new Vector2())
            {
                motor.MoveInput(new Vector2());
                anim.SetBool("moving", false);
                sprite.GetChild(0).localRotation = Quaternion.identity;
            }

            return;
        }

        float hor = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");

        motor.MoveInput(new Vector2(hor, (onLadder ? vert : 0)));

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            onLadder = true;
            motor.body.useGravity = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (onLadder && collision.CompareTag("Ladder"))
        {
            onLadder = false;
            motor.body.useGravity = true;
        }
    }
}
