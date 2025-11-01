using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public Vector2 dir;
    public Quaternion rotate;
    public Color color;
    Rigidbody2D rb;
    SpriteRenderer sr;

    public GameObject lightPrefabs;
    public LightManager currentLight;
    public List<LightManager> lightsList;


    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        // Angle chi tra ve goc tu 0->180 vaf ko biết quay theo chiều nào
        // SignedAngle thì trả về góc xịn coi góc form là gốc và lấy theo góc quay của hệ tọa độ
        // calculate góc theo giữa hướng bay của đạn và vector normal

        currentLight = Instantiate(lightPrefabs, transform.position, Quaternion.identity).GetComponent<LightManager>();
        lightsList.Add(currentLight);

    }
    public void Update()
    {
        // make the bullet face the direction it's moving
        float angle = Vector2.SignedAngle(Vector2.up, dir);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Giữ tốc độ cố định theo dir (tránh dùng AddForce mỗi frame)
        if (rb != null)
            rb.linearVelocity = dir.normalized * speed;

        currentLight.SetPosition(this.transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Mirror"))
        {
            Transform mirrorTransform = collision.collider.transform;
            foreach (var contact in collision.contacts)
            {
                // angle between contact point vector normal and Mirror above face
                float angleAboveFaceAndPoint = Vector2.SignedAngle(mirrorTransform.up, contact.normal); 
                
                // check if hit above Head
                if (angleAboveFaceAndPoint <= 45 && angleAboveFaceAndPoint >= -45)
                {
                    // check góc giữa hướng bay của vector normal của contacePoint với đạn 

                    // reflet góc bay đến với mặt phẳng thông qua vector normal
                    Vector2 newDir = Vector2.Reflect(dir, contact.normal);

                    //print(Vector2.SignedAngle(dir, -newDir));

                    // xoay đạn quanh một trục z đi qua điểm contact.normal
                    transform.RotateAround(new Vector3(contact.point.x,contact.point.y,0),Vector3.forward, Vector2.SignedAngle(dir, -newDir));
                    dir = newDir.normalized;

                    // tạo lineRender mới
                    currentLight = Instantiate(lightPrefabs, transform.position, Quaternion.identity).GetComponent<LightManager>();
                    currentLight.SetColor(this.color);
                    lightsList.Add(currentLight);
                }

            }
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
        else if(collision.gameObject.CompareTag("Goal"))
        {
            Destroy(gameObject);
        }
    }

    public void UpdateColor(Color color)
    {
        this.color = color;
        sr.color = color;
    }

}
