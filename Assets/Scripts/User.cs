using UnityEngine;

public class User : MonoBehaviour
{
    public static string UserName;
    public static string UserId;
    public static string CourseName;
    public static string CourseId;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
