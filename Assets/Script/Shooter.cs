using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] GameObject bulletGO;

    public GameObject handAim;
    public GameObject body;

    [Header("Shooting Parameters")]
    [SerializeField] float speed ;
    [SerializeField] float maxShootRadius =5f;

    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Color lazerColor = Color.red;
    [SerializeField] float lineWidth = 0.05f;
    [SerializeField] LayerMask reflectLayerRaycast;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        SetUp();
    }

    void SetUp()
    {
        lineRenderer.startColor = lazerColor;
        lineRenderer.endColor = lazerColor;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }

    // Update is called once per frame
    void Update()
    {

        KeepAimInCircle();

        ShowLazer();

        if (Input.GetKeyDown(KeyCode.E))
        {
            ShootLight();
        }
    }

    void ShootLight()
    {
        Bullet bullet = Instantiate(bulletGO, handAim.transform.position, Quaternion.identity).GetComponent<Bullet>() ;

        bullet.speed =  speed;
        bullet.dir = (handAim.transform.position -  body.transform.position).normalized;
    }

    void KeepAimInCircle()
    {
        if (Vector2.Distance(handAim.transform.position, body.transform.position) > maxShootRadius)
        {
            handAim.transform.position = body.transform.position + (handAim.transform.position - body.transform.position).normalized * maxShootRadius;
        }
    }

    void ShowLazer()
    {
        // nếu muốn vẽ tia sáng trong game thì có thể dùng lineRenderer (runtime)
        // nếu muốn vẽ trong editor thì dùng Debug.DrawLine hoặc OnDrawGizmos(Gizmos.DrawLine

        if(lineRenderer == null || handAim == null || body == null) return;

        // mỗi khi va chạm sẽ add 1 point to points list

        int maxReflections = 10;
        float maxDistance = 1000f;
        List<Vector3> points = new List<Vector3>();



        Vector3 currentStartPos = new Vector3();
        Vector3 currentEndPos = new Vector3();
        Vector3 currentDir = new Vector3();


        currentStartPos = handAim.transform.position;
        currentDir = handAim.transform.position - body.transform.position;

        points.Add(currentStartPos);

        int count = 0;
        for (int i = 0; i < maxReflections ; i++)
        {
            count++;
            RaycastHit2D hit = Physics2D.Raycast(currentStartPos, currentDir, maxDistance, reflectLayerRaycast);

            if (hit.transform != null)
            {
                currentEndPos = hit.point;

                if (points.Contains(currentEndPos))
                {
                    break;
                }
                points.Add(currentEndPos);

                currentStartPos = currentEndPos;
                Debug.DrawLine(currentEndPos, currentEndPos + (Vector3)hit.normal *100, lazerColor);
                currentDir = Vector3.Reflect(currentDir, hit.normal);

            }
            else
            {

                currentEndPos = currentStartPos + currentDir.normalized * maxDistance;
                //print(currentEndPos);
                points.Add(currentEndPos);
                break;
            }

        }

        lineRenderer.positionCount = points.Count;

        for (int i = 0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, points[i]);
        }

    }
}
