using UnityEngine;

public class AbstractionBullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Correcto"))
        {
            AbstractionGameManager.Instance.Correct();
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Incorrecto"))
        {
            AbstractionGameManager.Instance.Incorrect();
            Destroy(other.gameObject);
        }
    }
}
