using System;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerSettings 
{
    #region Player

    [Serializable]
    public class PlayerSettingsModel
    {
        [Header("View Settings")]
        public float ViewSensitivityX;
        public float ViewSensitivityY;

        public bool ViewIntertedX;
        public bool ViewIntertedY;

        public float minCameraRotationX;
        public float maxCameraRotationX;

        [Header("Movement Settings")]
        public float WalkingFowardSpeed;
        public float WalkingStrafeSpeed;
        public float WalkingBackwardSpeed;
    }


    #endregion
}
