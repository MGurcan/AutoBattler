using UnityEngine;

public class RotateAroundItself : MonoBehaviour
{
    public float rotationSpeed = 10.0f; // Dönme hızı derece/saniye cinsinden

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
