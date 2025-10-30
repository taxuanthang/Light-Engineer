using UnityEngine;
using UnityEngine.EventSystems;

public class OrbitObjectTouchControler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    // phải có EventSystem
    // phải có collider ở vật tiếp nhận
    public Transform orbitUI;
    public GameObject GO;
    public float rotationSpeed = 100f;

    //private Vector2 startTouchPos;
    private bool isDragging = false;

    void Start()
    {
        if (orbitUI != null)
        {
            orbitUI.gameObject.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        orbitUI.gameObject.SetActive(true);
        orbitUI.position = GO.transform.position;
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        orbitUI.gameObject.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // event data sẽ liên quan đến screen space chứ ko phải world space nếu lấy eventData.position thì sẽ tính gốc là từ bottom left của screen

        if (!isDragging) return;

        //Vector2 dir = eventData.position - (Vector2)shooter.body.transform.position;

        //  lấy vị trí của chuột trên screen space
        Vector3 screenPos = eventData.position;
        // lây z của body so với camera
        float z = Camera.main.WorldToScreenPoint(GO.transform.position).z;
        // chuyển từ screen space sang world space
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, z));

        // Cập nhật vị trí handAim

        float angle = Vector2.SignedAngle(Vector2.up, worldPos - GO.transform.position);
        GO.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }


}
