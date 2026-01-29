using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FitPointsInCameraOnce : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    [Tooltip("Отступ от края (в долях viewport: 0.05 = 5%)")] [Range(0f, 0.2f)]
    public float padding = 0.05f;

    [Tooltip("Макс. итераций подбора FOV")]
    public int maxIterations = 10;

    [Tooltip("Шаг увеличения FOV (в градусах на итерацию)")]
    public float fovStep = 5f;

    [Tooltip("Макс. допустимый FOV (защита от 'рыбьего глаза')")]
    public float maxFov = 120f;

    private Camera _cam;

    void Start()
    {
        _cam = GetComponent<Camera>();
        if (!_cam.orthographic)
        {
            FitFovForPoints(_cam, pointA.position, pointB.position, padding, maxIterations, fovStep, maxFov);
        }
        else
        {
            Debug.LogWarning("Этот скрипт поддерживает только перспективную камеру.");
        }
    }


    public static void FitFovForPoints(
        Camera cam,
        Vector3 p1,
        Vector3 p2,
        float padding = 0.05f,
        int maxIter = 10,
        float step = 5f,
        float maxFov = 120f)
    {
        if (cam == null || cam.orthographic) return;

        float originalFov = cam.fieldOfView;
        float currentFov = originalFov;

        for (int i = 0; i < maxIter; i++)
        {
            cam.fieldOfView = currentFov;

            Vector3 v1 = cam.WorldToViewportPoint(p1);
            Vector3 v2 = cam.WorldToViewportPoint(p2);


            bool inside =
                v1.x >= padding && v1.x <= 1f - padding &&
                v1.y >= padding && v1.y <= 1f - padding &&
                v2.x >= padding && v2.x <= 1f - padding &&
                v2.y >= padding && v2.y <= 1f - padding;

            if (inside)
            {
                return;
            }


            currentFov += step;

            if (currentFov >= maxFov)
            {
                cam.fieldOfView = maxFov;

                return;
            }
        }


        cam.fieldOfView = currentFov;
    }
}