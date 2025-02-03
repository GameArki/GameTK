using System;
using UnityEditor.PackageManager;
using UnityEngine;

namespace NJM.Framework_Feeling2D {

    internal static class FFWLog {

        public static LogLevel logLevel = LogLevel.Verbose;

        public static void Log(string msg) {
            if ((int)logLevel >= (int)LogLevel.Verbose) {
                Debug.Log($"[Feeling2DFramework] {msg}");
            }
        }

        public static void LogWarning(string msg) {
            if ((int)logLevel >= (int)LogLevel.Warn) {
                Debug.LogWarning($"[Feeling2DFramework] {msg}");
            }
        }

        public static void LogError(string msg) {
            if ((int)logLevel >= (int)LogLevel.Error) {
                Debug.LogError($"[Feeling2DFramework] {msg}");
            }
        }

    }
}