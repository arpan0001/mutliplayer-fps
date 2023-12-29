using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceObjectToCamera : MonoBehaviour
{
    void Update()
    {
        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
        }
        else
        {
            Debug.LogError("Main camera not found. Make sure there is a camera with the 'MainCamera' tag in the scene.");
        }
    }
}
