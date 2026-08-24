using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float speed = 5f;

    public LineRenderer reflectionRay;
    public LineRenderer refractionRay;

    // Glass ka refractive index
    public float refractiveIndex = 1.5f;

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 hitPoint = collision.contacts[0].point;
        Vector3 normal = collision.contacts[0].normal;

        // Incoming direction
        Vector3 incomingDirection = collision.relativeVelocity.normalized;

        // =========================
        // REFLECTION
        // =========================

        Vector3 reflectedDirection =
            Vector3.Reflect(incomingDirection, normal);

        // Scene view me red reflection ray
        Debug.DrawRay(
            hitPoint,
            reflectedDirection * 5f,
            Color.red,
            2f
        );

        // ReflectionRay Line Renderer
        if (reflectionRay != null)
        {
            reflectionRay.positionCount = 2;

            reflectionRay.SetPosition(
                0,
                hitPoint
            );

            reflectionRay.SetPosition(
                1,
                hitPoint + reflectedDirection * 5f
            );
        }

        // =========================
        // REFRACTION
        // =========================

        Vector3 refractedDirection;

        float eta = 1f / refractiveIndex;

        float cosI = -Vector3.Dot(normal, incomingDirection);

        float sinT2 = eta * eta * (1f - cosI * cosI);

        if (sinT2 <= 1f)
        {
            float cosT = Mathf.Sqrt(1f - sinT2);

            refractedDirection =
                eta * incomingDirection +
                (eta * cosI - cosT) * normal;

            refractedDirection.Normalize();

            // Scene view me cyan refraction ray
            Debug.DrawRay(
                hitPoint,
                refractedDirection * 5f,
                Color.cyan,
                2f
            );

            // RefractionRay Line Renderer
            if (refractionRay != null)
            {
                refractionRay.positionCount = 2;

                refractionRay.SetPosition(
                    0,
                    hitPoint
                );

                refractionRay.SetPosition(
                    1,
                    hitPoint + refractedDirection * 5f
                );
            }
        }

        // =========================
        // MOVE SPHERE IN REFLECTED DIRECTION
        // =========================

        transform.rotation =
            Quaternion.LookRotation(reflectedDirection);

        GetComponent<Rigidbody>().linearVelocity =
            reflectedDirection * speed;
    }
}