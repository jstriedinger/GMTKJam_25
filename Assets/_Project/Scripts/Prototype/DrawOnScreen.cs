using System;
using UnityEditor;
using UnityEngine;

public class DrawOnScreen : MonoBehaviour
{
    private LineRenderer line;
    private Vector3 previousPosition;
    [SerializeField] private Vector3 currentPosition;
    private Vector3[] linePositions;
    private bool isDrawing;
    private bool previousHasRun;
    public Camera mainCamera;
    public GameObject mousePositionObject;
    // Event when drawing has started and stopped
    public static event Action<bool> onDraw;
    // Event for the drawing speed (used for audio)
    public static event Action<float> drawSpeed;
    [SerializeField] private float speed;
    // Distance of line from camera
    [SerializeField] private float lineZSpace = 2.5f;
    // y angle to point the raycast towards
    [SerializeField] private float raycastAngleY = -50;
    // Minimum distance before adding a Line Renderer position
    private float minimumLineDrawingDistance = 0.001f;



    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 1;
        previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            EditorApplication.isPaused = true;
        }
        currentPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, lineZSpace));
        mousePositionObject.transform.position = currentPosition;

        if (Input.GetMouseButton(0))
        {
            isDrawing = true;

            Draw();

            if (!previousHasRun)
            {
                InvokeDraw();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            RaycastFromLinePoints();
            //put the points on the surface
           /* Vector3[] groundPoints3D = new Vector3[line.positionCount];
            for (int i = 0; i < line.positionCount; i++)
            {
                groundPoints3D[i] = ProjectToGround(line.GetPosition(i));
                //line.SetPosition(i, ProjectToGround(line.GetPosition(i)));
            }
            EnemyManager.Instance?.CheckLinedrawHit(groundPoints3D);
            groundPoints3D = Array.Empty<Vector3>();*/
            isDrawing = false;

            //CalculateDrawnCentroid();

            line.positionCount = 0;

            InvokeDraw();
            previousHasRun = false;
            
        }
    }

    // Send out event when drawing. previousHasRun is used so it doesn't repeat in the update loop
    private void InvokeDraw()
    {
        if (isDrawing == true)
        {
            onDraw?.Invoke(true);
            previousHasRun = true;
        }
        if (isDrawing == false)
        {
            onDraw?.Invoke(false);
            previousHasRun = true;
        }
    }

    // Draw the line based on current mouse position
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
        }

        speed = distance / Time.deltaTime;
        drawSpeed?.Invoke(speed);
    }

    private void RaycastFromLinePoints()
    {
        linePositions = new Vector3[line.positionCount];
        line.GetPositions(linePositions);
        foreach (Vector3 pos in linePositions)
        {
            CreateRaycastHit(pos);
        }
    }

    // Find the centre of all the points created by the line renderer
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
    
    // Create a raycast from centroid. Use this to detect enemies
    private void CreateRaycastHit(Vector3 pos)
    {

        //Vector3 rayDirection = new Vector3(0, raycastAngleY, 100);
        //Debug.Log(rayDirection);

        //Physics.Raycast(centroid, rayDirection, 100f);
        Vector3 screenPoint = mainCamera.WorldToScreenPoint(pos); 

        Ray ray = mainCamera.ScreenPointToRay(screenPoint);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 50f, LayerMask.GetMask("Enemy")))
        {
            Collider hitCollider = hit.collider;
            EnemyChaseAI  enemy = hitCollider.GetComponent<EnemyChaseAI>();
            if(enemy != null && !enemy.isDead) 
            {
                enemy.OnHitByLinedraw();
            }
        }
        Debug.DrawRay(pos, ray.direction * 50, Color.red, 5f);
    }
    
    Vector3 ProjectToGround(Vector3 worldPoint)
    {
        Ray ray = new Ray(mainCamera.transform.position, (worldPoint - mainCamera.transform.position).normalized);
        float distance;
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // Y=0 plane
        if (groundPlane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance); // Point on Y=0
        }
        return worldPoint; // Fallback
    }
}