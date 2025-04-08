using System;
using UnityEngine;

namespace GameTK.Framework_Feeling2D {

    internal static class FFWLog {

        // 0: None, 1: Error, 2: Warn, 3: Info, 4: Debug, 5: Verbose
        public static int logLevel = 5;

        public static void Log(string msg) {
            if (logLevel >= 4) {
                Debug.Log($"[Feeling2DFramework] {msg}");
            }
        }

        public static void LogWarning(string msg) {
            if (logLevel >= 3) {
                Debug.LogWarning($"[Feeling2DFramework] {msg}");
            }
        }

        public static void LogError(string msg) {
            if (logLevel >= 2) {
                Debug.LogError($"[Feeling2DFramework] {msg}");
            }
        }

    }
}