using UnityEngine;

namespace Components.LevelManagement
{
    public class ExitGameComponent : MonoBehaviour
    {
        public void Exit()
        {
            Application.Quit();
        }
    }
}