using UnityEngine;
using System.Collections;

public class gradual : MonoBehaviour {

	private Color red = Color.red;
	private Color eyan;
	private Color yellow;
	private Mesh curMesh;
	private int vertexCount;

	//保留10个mesh
	public int gradualmeshs = 10;
	public int eyan_end = 180;
	
	public int red_end1 = 20;
	//限制最小速度
	public int red_end2 = 240;
	
	public int yellow_end1 = 60;
	public int yellow_end2 = 220;

	//mesh顶点的颜色
	private Color[] colors;

	// Use this for initialization
	void Start () {
		curMesh = GetComponent<MeshFilter>().mesh;
		vertexCount = curMesh.vertexCount;
		eyan = new Color (0.2f, 0.94f, 0.98f, 1.0f);
		yellow = new Color (1.0f,0.77f,0,1.0f);
		print(curMesh.colors.Length);
		print(curMesh.vertices.Length);
		print(curMesh.vertexCount);
	}
	
	
	// Update is called once per frame
	void Update () {
		
		Vector3[] vertices = curMesh.vertices;
		colors = new Color[vertices.Length];
		
		for (int i = 0; i < vertexCount; i++)
		{
			//开始和结尾红色的部分
			if(i<=(red_end1-gradualmeshs)||i>=(red_end2-gradualmeshs)){
				colors[i] = red;
			}
			//红色和黄色的过渡
			if(i>(red_end1-gradualmeshs)&&i<=(yellow_end1-gradualmeshs)){
				colors[i] = grandualColors(red,yellow,i,red_end1-gradualmeshs);
			}
			//黄色和青色的过渡
			if(i>(yellow_end1-gradualmeshs)&&i<=(eyan_end-gradualmeshs)){
				colors[i] = grandualColors(yellow,eyan,i,yellow_end1-gradualmeshs);
			}
			//青色到黄色的过渡
			if(i>(eyan_end-gradualmeshs)&&i<=(yellow_end2-gradualmeshs)){
				colors[i] = grandualColors(eyan,yellow,i,eyan_end-gradualmeshs);
			}
			//黄色到红色的过渡
			if(i>(yellow_end2-gradualmeshs)&&i<=(red_end2-gradualmeshs)){
				colors[i] = grandualColors(yellow,red,i,yellow_end2-gradualmeshs);
			}

		}
		
		curMesh.colors = colors;
		
	}

	private Color grandualColors(Color start_color,Color end_color,int position,int start_postion){
		float grandual = ((position-start_postion)*1.0f/(gradualmeshs*2));
		return Color.Lerp(start_color,end_color,grandual);
	}

	//改变arrow的颜色
	public Color GetMeshColor(Vector3 postion){
		if (colors != null) {
				float min = 10.0f;
				int meshid = 1;
				for (int i = 0; i < vertexCount; i++)
				{
				//计算向量的模最小
					if(Vector3.Magnitude(transform.TransformPoint(curMesh.vertices[i])-postion)<min){
					min = Vector3.Magnitude(transform.TransformPoint(curMesh.vertices[i])-postion);
						meshid = i;
					}
				}
		//	Debug.Log("mesh id is "+meshid);
				return colors [meshid];
		}
		return red;
	}

	public void setColor(int red_end1,int red_end2,int yellow_end1,int yellow_end2,int eyan_end){
		this.red_end1 = red_end1<20?20:red_end1;
		this.red_end2 = red_end2;
		this.yellow_end1 = yellow_end1;
		this.yellow_end2 = yellow_end2;
		this.eyan_end = eyan_end;
	}
}
