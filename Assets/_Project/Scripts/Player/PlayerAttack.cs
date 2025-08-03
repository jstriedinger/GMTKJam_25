using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int maxLinePoints = 50;
    private float _minPointDistance = 0.0001f;
    private List<Vector3> _linePositions;
    
    [SerializeField] private Animator charAnimator;
    private LineRenderer _line;
    private Vector3 _previousPosition;
    [SerializeField] private Vector3 currentPosition;
    private bool _isDrawing;
    private bool _previousHasRun;
    private Camera _mainCamera;
    public GameObject mousePositionObject;
    // Event when drawing has started and stopped
    public static event Action<bool> OnDraw;
    // Event for the drawing speed (used for audio)
    public static event Action<float> drawSpeed;
    [SerializeField] private float speed;
    // Distance of line from camera
    [SerializeField] private float lineZSpace = 2.5f;
    // Minimum distance before adding a Line Renderer position



    void Start()
    {
        _mainCamera = Camera.main;
        _line = GetComponent<LineRenderer>();
        _line.positionCount = 0;
        _linePositions = new  List<Vector3>();
        _previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
        {
            EditorApplication.isPaused = true;
        }
#endif
        currentPosition = _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, lineZSpace));
        mousePositionObject.transform.position = currentPosition;

        if (Input.GetMouseButton(0))
        {
            _isDrawing = true;

            Draw();

            if (!_previousHasRun)
            {
                InvokeDraw();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            RaycastFromLinePoints();
            
            _isDrawing = false;

            //CalculateDrawnCentroid();

            _line.positionCount = 0;
            _linePositions = new List<Vector3>();

            InvokeDraw();
            _previousHasRun = false;
        }
    }

    // Send out event when drawing. previousHasRun is used so it doesn't repeat in the update loop
    private void InvokeDraw()
    {
        if (_isDrawing == true)
        {
            OnDraw?.Invoke(true);
            charAnimator.SetBool("Attacking", true);
            _previousHasRun = true;

        }
        if (_isDrawing == false)
        {
            OnDraw?.Invoke(false);
            charAnimator.SetBool("Attacking", false);
            _previousHasRun = true;
        }
    }

    // Draw the line based on current mouse position
    private void Draw()
    {
        float distance = 0;
        // Only add if moved enough
        if (_linePositions.Count == 0)
        {
            AddPoint(currentPosition);
            
        }
        else
        {
            distance = Vector3.Distance(currentPosition, _linePositions[_linePositions.Count - 1]);
            if (distance > _minPointDistance)
            {
                AddPoint(currentPosition);
            }
        }

        // Update the LineRenderer
        _line.positionCount = _linePositions.Count;
        _line.SetPositions(_linePositions.ToArray());
      
        speed = distance / Time.deltaTime;
        drawSpeed?.Invoke(speed);
    }
    
    void AddPoint(Vector3 point)
    {
        // Add new point
        _linePositions.Add(point);

        // If we exceed max points, remove the oldest
        if (_linePositions.Count > maxLinePoints)
        {
            _linePositions.RemoveAt(0); // Removes first element
        }
    }

    private void RaycastFromLinePoints()
    {
        //_linePositions = new Vector3[_line.positionCount];
        //_line.GetPositions(_linePositions);
        foreach (Vector3 pos in _linePositions)
        {
            CreateRaycastHit(pos);
        }
    }

   

    // Create a raycast from centroid. Use this to detect enemies
    private void CreateRaycastHit(Vector3 pos)
    {

        Vector3 screenPoint = _mainCamera.WorldToScreenPoint(pos);

        Ray ray = _mainCamera.ScreenPointToRay(screenPoint);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 50f, LayerMask.GetMask("Enemy")))
        {
            Collider hitCollider = hit.collider;
            EnemyBaseAI enemy = hitCollider.GetComponent<EnemyBaseAI>();
            if (enemy != null)
            {
                Health health = enemy.GetComponent<Health>();
                if (health != null && !health.IsDeath())
                {
                    enemy.OnHitByLinedraw();
                }
            }
        }
        Debug.DrawRay(pos, ray.direction * 50, Color.red, 5f);
    }

    Vector3 ProjectToGround(Vector3 worldPoint)
    {
        Ray ray = new Ray(_mainCamera.transform.position, (worldPoint - _mainCamera.transform.position).normalized);
        float distance;
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // Y=0 plane
        if (groundPlane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance); // Point on Y=0
        }
        return worldPoint; // Fallback
    }
}