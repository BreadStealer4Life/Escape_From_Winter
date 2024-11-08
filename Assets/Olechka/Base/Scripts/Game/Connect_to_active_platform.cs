//Коннектит к поверхности движущейся платформы объекты
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Olechka
{
    [AddComponentMenu("Olechka scripts / Game / Movement / Connect to active platform")]
    [DisallowMultipleComponent]
    public class Connect_to_active_platform : MonoBehaviour
    {

        [Tooltip("Конроллер")]
        [SerializeField]
        CharacterController CharacterController_component = null;

        [Tooltip("Слой для взаимодействия с платформами")]
        [SerializeField]
        LayerMask Mask_platform = 8;

        Transform Active_platform;

        Vector3 Move_direction = Vector3.zero;

        Vector3 Active_global_platform_point = Vector3.zero;

        Vector3 Active_local_platform_point = Vector3.zero;

        Quaternion Active_global_platform_rotation = Quaternion.identity;

        Quaternion Active_local_platform_rotation = Quaternion.identity;

        void Update()
        {
            Connected_platform();

            if (Active_platform != null)
            {
                Vector3 new_global_platform_point = Active_platform.TransformPoint(Active_local_platform_point);
                Move_direction = new_global_platform_point - Active_global_platform_point;
                if (CharacterController_component && Move_direction.magnitude > 0.01f)
                {
                    CharacterController_component.Move(Move_direction);
                }
                if (Active_platform)
                {
                    // Поддержка вращения подвижной платформы
                    Quaternion newGlobalPlatformRotation = Active_platform.rotation * Active_local_platform_rotation;
                    Quaternion rotationDiff = newGlobalPlatformRotation * Quaternion.Inverse(Active_global_platform_rotation);
                    // Прекратить вращение локального вектора вверх
                    rotationDiff = Quaternion.FromToRotation(rotationDiff * Vector3.up, Vector3.up) * rotationDiff;
                    transform.rotation = rotationDiff * transform.rotation;
                    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

                    Update_moving_platform();
                }
            }
            else
            {
                if (Move_direction.magnitude > 0.01f)
                {
                    Move_direction = Vector3.Lerp(Move_direction, Vector3.zero, Time.deltaTime);

                    if (CharacterController_component)
                        CharacterController_component.Move(Move_direction);
                }
            }
        }

        void Connected_platform()
        {
            RaycastHit hit;
            
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f, Mask_platform, QueryTriggerInteraction.Ignore))
            {
                
                if (Active_platform != hit.collider.transform)
                {
                    Active_platform = hit.collider.transform;
                    Update_moving_platform();
                }
            }
            else
            {
                Active_platform = null;
            }

        }


        void Update_moving_platform()
        {
            Active_global_platform_point = transform.position;
            Active_local_platform_point = Active_platform.InverseTransformPoint(transform.position);
            // Вращает вместе с платформой
            Active_global_platform_rotation = transform.rotation;
            Active_local_platform_rotation = Quaternion.Inverse(Active_platform.rotation) * transform.rotation;
        }
    }
}