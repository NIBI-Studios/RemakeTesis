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
        loadingCircle.SetActive(true);
        var json = JsonUtility.ToJson(
            new AuthRequest()
            {
                username = usernameField.text,
                pin = int.Parse(passwordField.text)
            });
        using UnityWebRequest request = UnityWebRequest.Post($"{Constants.BASE_URI}/auth/studentLogin", json, "application/json");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
        {
            errorMessage.SetActive(true);
            yield break;
        }
        else
        {
            var responseJson = request.downloadHandler.text;
            var response = JsonUtility.FromJson<AuthResponse>(responseJson);
            string token = response.token;
            PlayerPrefs.SetString("auth_token", token);
            var jwtPayload = DecodeJWT(token);
            string userId = jwtPayload.id;
            using UnityWebRequest userRequest = UnityWebRequest.Get($"{Constants.BASE_URI}/student/{userId}");
            yield return userRequest.SendWebRequest();
            if (userRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                errorMessage.SetActive(true);
                yield break;
            }
            else
            {
                var userRes = userRequest.downloadHandler.text;
                Debug.Log(userRes);
                var user = JsonUtility.FromJson<Student>(userRes);
                string username = $"{user.firstName} {user.lastName}";
                User.UserName = username;
                User.UserId = user.id;
                if (user.course != null)
                {
                    User.CourseId = user.course.id;
                    User.CourseName = user.course.name;
                }
                else
                {
                    User.CourseName = "Curso no registrado";
                }
                PlayerPrefs.SetString("userName", User.UserName);
            }
            loadingCircle.SetActive(false);
            SceneManager.LoadScene(Constants.TUTORIAL_SCENE_INDEX);
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
        var payloadJson = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(payload));
        return JsonUtility.FromJson<JWT_Payload>(payloadJson);
    }
}
