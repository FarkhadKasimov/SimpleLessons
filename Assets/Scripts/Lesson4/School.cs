using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class School : MonoBehaviour
{
    private Student _student = new Student();

    private void Start()
    {
        _student.CourseCount = 2;
    }
}

public class Student
{
    public string Name;
    public int Age;
    public int CourseCount;
}
