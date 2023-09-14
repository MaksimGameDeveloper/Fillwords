using System;
using UnityEngine;

namespace App.Scripts.Libs.CustomLogger
{
   public static class CustomLogger
   {
      public static void LogMessage(object message)
      {
         Debug.Log(message);
      }
   
      public static void LogError<T>(string message) where T : Exception, new()
      {
         throw (T)Activator.CreateInstance(typeof(T), message);
      }
   }
}
