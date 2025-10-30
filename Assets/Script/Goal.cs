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
    public void CheckWin()
    {
        switch (isAllowed)
        {
            case true:
                print("Thang");
                break;
            case false:

                break;
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
