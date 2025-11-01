using UnityEngine;

public class Goal : MonoBehaviour
{
    public float allowedTime = 0.5f;
    public float unallowedTime = 0.5f;
    public float timer = 0f;
    public bool isAllowed = false;
    public SpriteRenderer spriteRenderer;

    public void Awake()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }
    public virtual void CheckWin(Bullet bullet)
    {
        switch (isAllowed)
        {
            case true:
                break;
            case false:
                return;
        }

        print("Thang");

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
         if (collision.gameObject.CompareTag("Bullet"))
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            CheckWin(bullet);
        }
    }

    public void Update()
    {
        timer += Time.deltaTime;
        switch(isAllowed)
        {
            case true:
                if (timer >= allowedTime)
                {
                    isAllowed = false;
                    timer = 0f;
                }
                spriteRenderer.color = Color.green;
                break;
            case false:
                if (timer >= unallowedTime)
                {
                    isAllowed = true;
                    timer = 0f;
                }
                spriteRenderer.color = Color.red;
                break;
        }
    }
}

