using System;

namespace App.Scripts.Libs.CustomExtensions
{
    public class IncorrectLevelException : Exception
    {
        public IncorrectLevelException()
        {
        
        }
    
        public IncorrectLevelException(string message) : base(message)
        {
        
        }
    
    }
}
