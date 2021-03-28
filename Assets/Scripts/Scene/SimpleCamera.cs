using UnityEngine;

public class SimpleCamera : MonoBehaviour
{
    public float zAxis = -30f;
    public float deltaX = 1f;
    public float deltaY = 1f;
    public float followSpeed = 2f;

    private GameObject player = null;

    void Update()
    {
        if (player == null)
        {
            GameObject[] list = FindObjectsOfType<GameObject>();
            for (int t = 0; t < list.Length; t++)
                if (list[t].CompareTag("Player"))
                {
                    player = list[t];
                    break;
                }
        }
        else
        {
            Vector3 newPosition = player.transform.position;
            newPosition.z = zAxis;
            /*if (Mathf.Abs(newPosition.x - transform.position.x) < deltaX)
                newPosition.x = transform.position.x;
            if (Mathf.Abs(newPosition.y - transform.position.y) < deltaY)
                newPosition.y = transform.position.y;
            transform.position = Vector3.Slerp(transform.position, newPosition, followSpeed * Time.deltaTime);*/
            transform.position = newPosition;
        }
    }
}