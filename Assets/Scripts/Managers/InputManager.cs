using UnityEngine;
using Actors;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        private Camera camera;

        private static InputManager instance;
        public static InputManager Instance => instance;

        private void InitSingleton()
        {
            if (instance != null && instance != this) Destroy(gameObject);
            else instance = this;
        }

        private void Awake()
        {
            InitSingleton();
            camera = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 position = camera.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10f);
                MeshCutter.Instance.UpdateThePath(position);
            }
        }

    }
}
