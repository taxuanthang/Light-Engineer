using UnityEngine;

public class ColorMirror: Mirror
{
    public Color color = Color.red;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        bullet.UpdateColor(color);
    }
}