using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    [SerializeField] float destroyTime;
    float time;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > destroyTime)
        {
            Destroy(this.gameObject);
        }



    }
}
