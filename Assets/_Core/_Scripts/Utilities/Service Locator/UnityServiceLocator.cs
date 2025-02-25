
using UnityEngine;

namespace UnityServiceLocator
{
    public interface ILocalization
    {
        string GetLocalizedWord(string key);
    }
    public interface ISerializer
    {
        void Serialize();
    }
    public class MockSerializer: ISerializer
    {
        public void Serialize() {
            Debug.Log(" MockSerializer.Serialize() called");
        }
    }
    
}