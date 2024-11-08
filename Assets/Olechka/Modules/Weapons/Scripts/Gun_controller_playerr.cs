using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using NaughtyAttributes;

namespace Olechka
{
    public class Gun_controller_playerr : Gun_controller
    {


        [Tooltip("Камера")]
        [SerializeField]
        Camera Cam = null;

        [Tooltip("Ивент говорящий с какой стороны от персонажа находится курсов")]
        [SerializeField]
        UnityEvent<bool> Cursor_detect_right_event = new UnityEvent<bool>();

        bool Cursor_detect_right_bool = true;

        private void Update()
        {
            Cursor_detect();
        }

        void Cursor_detect()
        {
            Vector3 mouse_position = Input.mousePosition;

            mouse_position.z = Vector3.Distance(transform.position, Cam.transform.position);

            Vector3 position_target = Cam.ScreenToWorldPoint(mouse_position);

            if (position_target.x < transform.position.x && Cursor_detect_right_bool || position_target.x > transform.position.x && !Cursor_detect_right_bool)
            {
                Cursor_detect_right_bool = !Cursor_detect_right_bool;

                Cursor_detect_right_event.Invoke(Cursor_detect_right_bool);
            }

            if (Roration_bool)
                Rotation_towards_target(position_target);
                
        }
    }
}
