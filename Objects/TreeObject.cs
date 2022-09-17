using UnityEngine;

public class TreeObject : MonoBehaviour
{
    private void Start()
    {
        float orgScale = transform.localScale.x;

        float scale = Random.Range(transform.localScale.x - 0.3f, transform.localScale.x + 0.3f);
        float rot = Random.Range(-5f, 5f);

        transform.localScale = new Vector2(scale, scale);

        transform.position -= new Vector3(0f, (orgScale - scale) / 2f);

        transform.localEulerAngles = new Vector3(0f, 0f, rot);
        transform.position += new Vector3(0f, -0.1f);
        //transform.position -= new Vector3(0f, Mathf.Sin(rot * Mathf.Deg2Rad) * (100.09375f - 99.90625f) * transform.localScale.x / 2f);
    }
}