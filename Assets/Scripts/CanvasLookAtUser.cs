using UnityEngine;

public class CanvasLookAtUser : MonoBehaviour
{
    private Transform cameraTransform;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        Vector3 direction = cameraTransform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
