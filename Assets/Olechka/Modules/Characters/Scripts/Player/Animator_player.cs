using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator_player : MonoBehaviour
{

    [Tooltip("Аниматор")]
    [SerializeField]
    Animator Anim = null;

    public void Grounded(bool _active)
    {
        Anim.SetBool("Grounded", _active);
    }

    public void Attack()
    {
        Anim.CrossFade("Attack", 0f, 1, 0);
    }

    public void Jump()
    {
        Anim.CrossFade("Jump", 0f, 0, 0);
    }


    public void Move(float _speed)
    {
        if(_speed == 0)
            Anim.SetFloat("Move", 0);
        else
            Anim.SetFloat("Move", 1);
    }

}
