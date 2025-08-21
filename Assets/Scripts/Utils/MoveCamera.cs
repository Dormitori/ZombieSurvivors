using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public GameObject player;

    void Update()
    {
        Vector3 pos = player.transform.position;
        transform.position = new Vector3(pos.x, pos.y, -10);
    }
}
