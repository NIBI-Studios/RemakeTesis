using UnityEngine;

public class User : MonoBehaviour
{
    public static string UserName;
    public static string UserId;
    public static string CourseName;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
