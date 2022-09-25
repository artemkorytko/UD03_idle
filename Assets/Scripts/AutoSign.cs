using UnityEditor;
using UnityEngine;

namespace Idle
{
#if UNITY_EDITOR
    
    [InitializeOnLoad]
    public static class AutoSign
    {
        static AutoSign()
        {
            PlayerSettings.Android.keyaliasName = "idle";
            PlayerSettings.Android.keystoreName = "keystore.keystore";
            PlayerSettings.keyaliasPass = "mypass";
            PlayerSettings.keystorePass = "mypass";
        }
    }
#endif
}