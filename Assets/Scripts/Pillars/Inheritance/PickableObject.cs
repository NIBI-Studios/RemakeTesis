using UnityEngine;

public class PickableObject : MonoBehaviour
{
    private Vector3 startPosition;

    public string parentClass;
    private void Start()
    {
        startPosition = transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(parentClass))
        {
            InheritanceGameManager.Instance.Correct();
            Destroy(this);
        }
        else
        {
            InheritanceGameManager.Instance.Incorrect();
            transform.position = startPosition;
        }
    }
}
