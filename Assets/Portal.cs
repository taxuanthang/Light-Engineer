using System.Drawing;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal linkedPortal;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        //bullet.transform.position = linkedPortal.transform.position;
    }
}
