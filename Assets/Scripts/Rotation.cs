using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    GameObject go;
    float rotateSpeed = 10;
    private void Start()
    {
        go = this.gameObject; 
    }

    private void Update()
    {
        RotateUV();
    }
    public void RotateUV()
    {
        List<Vector2> uvs = new List<Vector2>(go.GetComponent<MeshFilter>().mesh.uv);


        float speed = 0;
            speed = rotateSpeed * Mathf.Deg2Rad * Time.deltaTime;


        for (int i = 0; i < uvs.Count; i++)
        {
            Vector2 uv = uvs[i] - new Vector2(0.5f, 0.5f);
            uv = new Vector2(uv.x * Mathf.Cos(speed) - uv.y * Mathf.Sin(speed),
                     uv.x * Mathf.Sin(speed) + uv.y * Mathf.Cos(speed));
            uv += new Vector2(0.5f, 0.5f);
            uvs[i] = uv;
        }
        go.GetComponent<MeshFilter>().mesh.SetUVs(0, uvs);
    }
}
