using System;
using UnityEditor;
using UnityEditor.ShaderGraph;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class DrawOnScreen : MonoBehaviour
{
    private LineRenderer line;
    private Vector3 previousPosition;
    private Vector3 currentPosition;
    private Vector3[] linePositions;
    public Camera mainCamera;
    public GameObject mousePositionObject;
    public static event Action<bool> onDraw;
    public static event Action<float> drawSpeed;
    [SerializeField] private float speed;
    [SerializeField] private float raycastAngleY = -50;
    [SerializeField] private float minimumLineDrawingDistance = 0.001f;



    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 1;
        previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5f));
        mousePositionObject.transform.position = currentPosition;

        if (Input.GetMouseButton(0))
        {
            onDraw?.Invoke(true);

            Draw();
        }

        if (Input.GetMouseButtonUp(0))
        {
            onDraw?.Invoke(false);

            CalculateDrawnCentroid();

            line.positionCount = 0;
        }

    }

    private void Draw()
    {
        if (previousPosition == transform.position)
        {
            line.SetPosition(0, currentPosition);
        }
        float distance = Vector3.Distance(currentPosition, previousPosition);

        if (distance > minimumLineDrawingDistance)
        {
            line.positionCount++;
            line.SetPosition(line.positionCount - 1, currentPosition);
            previousPosition = currentPosition;

            speed = distance / Time.deltaTime;
            drawSpeed?.Invoke(speed);
        }
    }

    private void CalculateDrawnCentroid()
    {
        linePositions = new Vector3[line.positionCount];
        line.GetPositions(linePositions);

        int arrayLength = linePositions.Length;

        Vector3 totalVectorAmount = new Vector3();
        Vector3 centroid;

        for (int i = 0; i < arrayLength; i++)
        {
            Vector3 position = linePositions[i];

            totalVectorAmount = totalVectorAmount + position;
        }

        centroid = totalVectorAmount / arrayLength;

        CreateRaycastHit(centroid);
    }

    private void CreateRaycastHit(Vector3 centroid)
    {
        RaycastHit hit;

        Vector3 rayDirection = new Vector3(0, raycastAngleY, 100);

        Physics.Raycast(centroid, rayDirection, 100f);
        Debug.DrawRay(centroid, rayDirection, Color.red, 5f);
    }

}
