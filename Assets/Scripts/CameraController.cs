using System.Linq;
using UnityEngine;
using Managers;

public class CameraController : MonoBehaviour
{
    public float smooth = 0.5f;
    public float offset;

    private Vector3 velocity;

    private Transform GetLowestBall
    {
        get => (from t in BallManager.Instance.ballList
                orderby t.position.y
                select t
                ).ToList()[0];
    }

    private void LateUpdate()
    {
        if (GetLowestBall != null)
        {
            Vector3 vector = new Vector3(0f, GetLowestBall.position.y + offset, -15f);

            if (vector.y < transform.position.y)
            {
                transform.position = Vector3.SmoothDamp(transform.position,
                                                        vector,
                                                        ref velocity,
                                                        smooth );
            }
        }
    }
    
}
