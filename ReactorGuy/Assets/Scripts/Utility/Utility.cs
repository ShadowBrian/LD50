using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class Utility
    {
        public static void LockCursor(bool isLocked)
        {
            if(isLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
            Cursor.visible = !isLocked;
        }
    }
}