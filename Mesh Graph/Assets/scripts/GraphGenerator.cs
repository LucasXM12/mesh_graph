using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GraphGenerator: MonoBehaviour {

	[Range(1, 1000000)]
	public float size;
	[Range(1, 10000)]
	public int resolution;

	private Func<float, float, float> func =
		(x, z) => Mathf.Sqrt(x * z);
	private Func<int, int, int, int> matrixToListIndex;

	private Mesh mesh;

	// Use this for initialization
	void Start() {
		this.matrixToListIndex = (i, j, s) => i * s + j;
		this.mesh = GetComponent<MeshFilter>().mesh;

		this.mesh.Clear();

		List<Vector3> vertices = GenerateVertices();
		List<int> triangles = GenerateTriangles(vertices);

		this.mesh.vertices = vertices.ToArray();
		this.mesh.triangles = triangles.ToArray();

		this.mesh.RecalculateNormals();
	}

	List<Vector3> GenerateVertices() {
		List<Vector3> ret = new List<Vector3>();

		float cX = -this.size / 2, cZ,  //Current x and z
			step = this.size / this.resolution; //Size of each cell

		for (int i = 0; i < this.resolution; i++) {
			cZ = -this.size / 2;

			for (int j = 0; j < this.resolution; j++) {
				ret.Add(new Vector3(cX, this.func(cX, cZ), cZ));
				cZ += step;
			}

			cX += step;
		}

		return ret;
	}

	List<int> GenerateTriangles(List<Vector3> vertices) {
		List<int> ret = new List<int>();

		for (int i = 0; i < this.resolution - 1; i++)
			for (int j = 0; j < this.resolution - 1; j++) {
				ret.Add(this.matrixToListIndex(i, j, this.resolution));
				ret.Add(this.matrixToListIndex(i, j + 1, this.resolution));
				ret.Add(this.matrixToListIndex(i + 1, j, this.resolution));

				ret.Add(this.matrixToListIndex(i, j + 1, this.resolution));
				ret.Add(this.matrixToListIndex(i + 1, j + 1, this.resolution));
				ret.Add(this.matrixToListIndex(i + 1, j, this.resolution));
			}

		return ret;
	}
}
