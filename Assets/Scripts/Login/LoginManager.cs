using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private GameObject errorMessage;
    [SerializeField] private GameObject loadingCircle;
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TMP_InputField passwordField;
    public void Login()
    {
        StartCoroutine(nameof(LoginCoroutine));
    }
    public void ContinueWithoutLogin()
    {
        SceneManager.LoadScene(Constants.TUTORIAL_SCENE_INDEX);
    }
    private IEnumerator LoginCoroutine()
    {
        errorMessage.SetActive(false);
        var json = JsonUtility.ToJson(
            new AuthRequest()
            {
                username = usernameField.text,
                pin = int.Parse(passwordField.text)
            });
        Debug.Log(json);
        UnityWebRequest request = UnityWebRequest.Post($"{Constants.BASE_URI}auth/studentLogin", json, "application/json");
        loadingCircle.SetActive(true);
        yield return request.SendWebRequest();
        loadingCircle.SetActive(false);
        if (request.downloadHandler.text.Contains("\"token\":"))
        {
            var responseJson = request.downloadHandler.text;
            var response = JsonUtility.FromJson<AuthResponse>(responseJson);
            string token = response.token;
            PlayerPrefs.SetString("auth_token", token);
            var jwtPayload = DecodeJWT(token);
            string userId = jwtPayload.id;
            UnityWebRequest userReq = UnityWebRequest.Get($"{Constants.BASE_URI}student/{userId}");
            yield return userReq.SendWebRequest();
            if (userReq.result == UnityWebRequest.Result.ConnectionError)
            {
                errorMessage.SetActive(true);
                yield break;
            }
            else
            {
                var userRes = userReq.downloadHandler.text;
                var user = JsonUtility.FromJson<Student>(userRes);
                string username = $"{user.firstName} {user.lastName}";
                User.UserName = username;
                User.UserId = user.id;
                if (user.course != null)
                {
                    User.CourseName = user.course.name;
                }
                else
                {
                    User.CourseName = "Curso no registrado";
                }
                PlayerPrefs.SetString("userName", User.UserName);
            }
            SceneManager.LoadScene(Constants.TUTORIAL_SCENE_INDEX);
        }
        else
        {
            errorMessage.SetActive(true);
        }
    }
    private JWT_Payload DecodeJWT(string token)
    {
        var parts = token.Split('.');
        var payload = parts[1];
        payload = payload.Replace('-', '+').Replace('_', '/');
        switch (payload.Length % 4)
        {
            case 2: payload += "=="; break;
            case 3: payload += "="; break;
        }
        Debug.Log(payload);
        var payloadJson = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(payload));
        Debug.Log(payloadJson);
        return JsonUtility.FromJson<JWT_Payload>(payloadJson);
    }
}

[Serializable]
class AuthRequest
{
    public string username;
    public int pin;
}
[Serializable]
class AuthResponse
{
    public string token;
}
[Serializable]
class JWT_Payload
{
    public string id;
}
[Serializable]
class Student
{
    public string id;
    public string firstName;
    public string lastName;
    public Course course;
}
class Course
{
    public string name;
}
