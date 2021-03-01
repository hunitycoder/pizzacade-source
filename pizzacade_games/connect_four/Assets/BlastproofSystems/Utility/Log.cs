using System.Runtime.InteropServices;
using UnityEngine;

namespace Blastproof
{
    /*
        This class is used to display logs and has more control for toggling them on/off
    */
    public static class Log 
    {
        // The maximum allowed log level.
        // On release, set this to int.MaxValue and this will guarantee no logs will pass through
        private static int _logLevel = 0;

        [DllImport("__Internal")]
        private static extern void LogMessageExternal(string log);

        [DllImport("__Internal")]
        private static extern void LogWarningExternal(string log);

        [DllImport("__Internal")]
        private static extern void LogErrorExternal(string log);

        // This method displays a message in the console, if the debug level is appropriate
        public static void Message(string message, string prefix = "", int level = 0)
        {
            if(level >= _logLevel)
                Debug.Log($"{prefix}: {message}");
        }

        // This method displays a message in the console, if the debug level is appropriate
        public static void Message(string message, string prefix = "", int level = 0, params object[] args)
        {
            Message(string.Format($"{prefix}: {message}", args), prefix, level);
        }

        // This method displays a warning in the console, if the debug level is appropriate 
        public static void Warning(string message, string prefix = "", int level = 0)
        {
            if (level >= _logLevel)
                Debug.LogWarning($"{prefix}: {message}");
        }

        // This method displays an error in the console, if the debug level is appropriate
        public static void Error(string message, string prefix = "", int level = 0)
        {
            if (level >= _logLevel)
                Debug.LogError($"{prefix}: {message}");
        }

        public static void TemporaryCode(int level = 0)
        {
            if (level >= _logLevel) Message("This feature is still Work in Progress!");
        }
    }
}