using UnityEngine;

namespace Nevelson.Utils
{
    public interface IMockInterface
    {
        void Test();
    }

    public class MockComponent : MonoBehaviour, IMockInterface
    {
        public string MockStringField = "Mock String";
        public string MockStringGetterSetter { get; set; } = "Mock Strang";
        private string _mockString = "Muck Stwing";
        public string MockStringGetterSetterWithRef
        {
            get => _mockString;
            set => _mockString = value;
        }

        public void Test()
        {
            Debug.Log("I am a test interface :D");
        }
    }
}