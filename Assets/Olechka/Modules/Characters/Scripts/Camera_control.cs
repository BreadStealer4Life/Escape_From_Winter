using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Olechka
{
    public class Camera_control : MonoBehaviour
    {
        #region Variables

        [Tooltip("Чувствительность мыши")]
        [SerializeField]
        float Sensitivity = 200f;

        [Tooltip("Камера которую будем крутить")]
        [SerializeField]
        CinemachineVirtualCamera Virtual_camera = null;

        float Sensitivity_active = 10f;

        float Control_Sensitivity = 1.0f;

        bool Activity_bool = true;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region MonoBehaviour Methods
        private void OnEnable()
        {
            Sensitivity_active = Sensitivity;

            if (Setting_menu.Singleton_Instance)
            {
                Setting_menu.Singleton_Instance.Mouse_sensitivity_d += Change_Sensitivity;
            }
        }


        private void Update()
        {
            if (Activity_bool)
                Rotation_camera();
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Methods
        void Change_Sensitivity(float value)
        {
            Control_Sensitivity = value;

            Sensitivity_active = Sensitivity * Control_Sensitivity;
        }

        void Rotation_camera()
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            Vector3 direction = new Vector3(-mouseY, mouseX, 0) * Sensitivity_active * Time.deltaTime;

            Virtual_camera.transform.eulerAngles += direction;
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

            #endregion

            ///////////////////////////////////////////////////////////////////////////////////////////////////


    }
}