using Unity.VisualScripting;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    LineRenderer lr;

    public void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, this.transform.position);
    }

    public void SetColor(Color color)
    {
        // ensure alpha is valid
        lr.material.color = color;
    }

    public void SetPosition( Vector3 end)
    {
        lr.SetPosition(1, end);
    }
}
