using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using NaughtyAttributes;

namespace Olechka
{

    enum Laser_enum
    {
        Two_targets,
        Ray
    }

    public class Laser : MonoBehaviour
    {
        #region Variables

        [Tooltip("Толщина триггера наносищий урон лазером")]
        [SerializeField]
        float Thickness = 0.5f;

        [Tooltip("Рендер линии")]
        [SerializeField]
        LineRenderer LeneRenderer_component = null;

        [Tooltip("Тип лазера")]
        [SerializeField]
        Laser_enum Type = Laser_enum.Two_targets;

        [Tooltip("Точка старта лазера")]
        [SerializeField]
        Transform Target_start = null;

        [HideIf(nameof(Type), Laser_enum.Ray)]
        [Tooltip("Точка конца лазера")]
        [SerializeField]
        Transform Target_end = null;

        [HideIf(nameof(Type), Laser_enum.Two_targets)]
        [Tooltip("Длина лазера")]
        [SerializeField]
        float Length = 4f;

        [HideIf(nameof(Type), Laser_enum.Two_targets)]
        [Tooltip("Слои взаимодействия")]
        [SerializeField]
        LayerMask Mask = 1;

        [Tooltip("Триггер")]
        [SerializeField]
        Transform Trigger = null;

        bool Game_start = false;

        Vector3 End_position_target = Vector3.zero;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region MonoBehaviour Methods
        private void Start()
        {
            LeneRenderer_component.positionCount = 2;

            Game_start = true;
        }

        private void Update()
        {
            New_position_point();
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        bool Check
        {
            get
            {
                bool result = true;

                if (LeneRenderer_component && Target_start)
                {
                    if(LeneRenderer_component.GetPosition(0) != Target_start.position)
                        result = false;
                    else if (LeneRenderer_component.GetPosition(1) != Get_End_position)
                        result = false;
                    else if (Trigger.localScale.x != Thickness)
                        result = false;
                }
                else
                {
                    result = false;
                }

                return result;
            }
        }

        Vector3 Get_End_position
        {
            get
            {
                Vector3 result = Vector3.zero;

                if (Type == Laser_enum.Two_targets)
                {
                    result = Target_end != null ? Target_end.position : Vector3.zero;
                }

                else if (Type == Laser_enum.Ray)
                {
                    Ray ray = new Ray(transform.position, Target_start.forward);

                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, Length, Mask, QueryTriggerInteraction.Ignore))
                    {
                        result = hit.point;
                    }
                    else
                    {
                        result = ray.origin + ray.direction * Length;
                    }
                }


                return result;
            }
        }

        #region Methods
        void New_position_point()
        {
            if (!Check)
            {

                Vector3 pos_start = Vector3.zero;

                if (Target_start)
                    pos_start = Target_start.position;

                LeneRenderer_component.SetPosition(0, pos_start);

                End_position_target = Get_End_position;

                LeneRenderer_component.SetPosition(1, End_position_target);

                if (Trigger != null)
                {
                    Vector3 center_point = Vector3.Lerp(pos_start, End_position_target, 0.5f);

                    Vector3 direction = Vector3.Normalize(pos_start - End_position_target);

                    Vector3 scale = Vector3.one * Thickness;

                    scale.z = Vector3.Distance(pos_start, End_position_target);

                    Trigger.position = center_point;

                    if(direction != Vector3.zero)
                        Trigger.rotation = Quaternion.LookRotation(direction);

                    Trigger.transform.localScale = scale;
                }
            }
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Additionally

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Gizmos

        private void OnDrawGizmosSelected()
        {
            if (LeneRenderer_component.positionCount > 1)
            {
                for (int x = 1; x < LeneRenderer_component.positionCount; x++)
                {
                    


                }
            }
        }

        private void OnDrawGizmos()
        {
            if(!Game_start)
                    New_position_point();
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////


    }
}