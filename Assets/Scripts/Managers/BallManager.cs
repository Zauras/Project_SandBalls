using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class BallManager : MonoBehaviour
    {
        public Transform ballHolder;
        public List<Transform> ballList = new List<Transform>();

        public int scoreBallsCount = 0;
        public int lostBallsCount = 0;

        private Camera camera;

        private static BallManager instance;
        public static BallManager Instance => instance;

        private void InitSingleton()
        {
            if (instance != null && instance != this) Destroy(gameObject);
            else instance = this;
        }

        private void Awake()
        {
            InitSingleton();
            camera = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();

            foreach (Transform ball in ballHolder)
            {
                ballList.Add(ball);
            }

        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Ball") && !ballList.Contains(col.transform))
            {
                col.gameObject.SetActive(false);
            }
        }

        public void AddScoreBall() => scoreBallsCount++;

        public void ResetGame()
        {
            scoreBallsCount = 0;
            lostBallsCount = 0;
            foreach (Transform ball in ballList)
            {
                ball.gameObject.SetActive(true);
            }
        }

        private bool IsObjectInView(GameObject Object)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
            return GeometryUtility.TestPlanesAABB(planes,
                Object.GetComponent<PolygonCollider2D>().bounds);
        }

        private void CheckIfAnyBallLost()
        {
            foreach (Transform ball in ballList)
            {
                if (!IsObjectInView(ball.gameObject))
                {
                    ball.gameObject.SetActive(false);
                    lostBallsCount++;
                }
            }
        }

        private void Update()
        {
            CheckIfAnyBallLost();

            if (lostBallsCount + scoreBallsCount == ballList.Count)
            {
                // end of the game
                UIManager.Instance.ShowWinPopUp();
            }

            // if (scoreBallsCount == ballList.Count)
            // {
            //     // you win
            // }
            // else if (IsObjectInView())
            // {
            //     // you loose
            // }
        }
        
    }
}
