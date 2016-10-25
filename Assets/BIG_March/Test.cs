using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

    public Color c0 = Color.red;
    public Color c1 = Color.blue;
    public Color c2 = Color.yellow;
    public Color c3 = Color.green;

    private Mesh curMesh;
    private int vertexCount;

	// Use this for initialization
	void Start () {
        curMesh = GetComponent<MeshFilter>().mesh;
        vertexCount = curMesh.vertexCount;

        print(curMesh.colors.Length);
        print(curMesh.vertices.Length);
        print(curMesh.vertexCount);
		for (int i = 0; i<curMesh.vertices.Length; i++) {
		//	print("ver "+i+" : "+curMesh.vertices[i][0]+","+curMesh.vertices[i][1]+","+curMesh.vertices[i][2]);
			print (transform.TransformPoint(curMesh.vertices[i]));
		}
	}

	
	// Update is called once per frame
	void Update () {

        Vector3[] vertices = curMesh.vertices;
        Color[] colors = new Color[vertices.Length];

        for (int i = 0; i < vertexCount; i++)
        {
            if (i == 0)
                colors[i] = c0;
            else if (i == 1)
                colors[i] = c1;
            else if (i == 2)
                colors[i] = c2;
            else if (i == 3)
                colors[i] = c3;
        }

        curMesh.colors = colors;

	}
}
