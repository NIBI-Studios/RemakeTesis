using System;

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