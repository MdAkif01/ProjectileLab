using UnityEngine;

public class ReflectionRefraction : MonoBehaviour
{
    [Header("Reflection Settings")]
    public LineRenderer reflectionLine;
    public float rayLength = 10f;

    [Header("Refraction Settings")]
    public LineRenderer refractionLine;
    public float refractiveIndex = 1.33f; // Water = 1.33, Glass = 1.5, Air = 1.0

    void Start()
    {
        // Line Renderer setup agar Inspector se assign nahi kiya
        if (reflectionLine == null)
        {
            GameObject reflectObj = new GameObject("ReflectionRay");
            reflectionLine = reflectObj.AddComponent<LineRenderer>();
            SetupLine(reflectionLine, Color.red);
        }

        if (refractionLine == null)
        {
            GameObject refractObj = new GameObject("RefractionRay");
            refractionLine = refractObj.AddComponent<LineRenderer>();
            SetupLine(refractionLine, Color.cyan);
        }
    }

    void SetupLine(LineRenderer lr, Color color)
    {
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = color;
        lr.endColor = color;
        lr.positionCount = 2;
    }

    void Update()
    {
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = transform.forward; // Light ray ki direction

        RaycastHit hit;

        // Incoming ray draw karo hamesha
        Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.yellow);

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, rayLength))
        {
            // ===== REFLECTION =====
            Vector3 reflectDir = Vector3.Reflect(rayDirection, hit.normal);
            
            reflectionLine.SetPosition(0, hit.point);
            reflectionLine.SetPosition(1, hit.point + reflectDir * 3f);

            // ===== REFRACTION (Snell's Law) =====
            Vector3 refractDir = Refract(rayDirection, hit.normal, 1.0f, refractiveIndex);

            refractionLine.SetPosition(0, hit.point);
            refractionLine.SetPosition(1, hit.point + refractDir * 3f);

            // Incoming ray bhi LineRenderer se dikhana ho toh:
            Debug.DrawLine(rayOrigin, hit.point, Color.yellow);
        }
    }

    // Snell's Law based refraction calculation
    Vector3 Refract(Vector3 incident, Vector3 normal, float n1, float n2)
    {
        float ratio = n1 / n2;
        float cosI = -Vector3.Dot(normal, incident);
        float sinT2 = ratio * ratio * (1.0f - cosI * cosI);

        if (sinT2 > 1.0f)
        {
            // Total internal reflection ho gaya - is case mein reflect kar do
            return Vector3.Reflect(incident, normal);
        }

        float cosT = Mathf.Sqrt(1.0f - sinT2);
        Vector3 refracted = ratio * incident + (ratio * cosI - cosT) * normal;
        return refracted.normalized;
    }
}