using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TileMapVisual;

public class TileSelectButton : MonoBehaviour
{
    public Button inputButton;
    private Mesh mesh;
    private Dictionary<TileMapSprite, UVCoords> uvCoordsDictionary;
    public TileMapSprite tileMapSprite;

    // Start is called before the first frame update
    public void Init(TileMapUV uv)
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        uvCoordsDictionary = new Dictionary<TileMapSprite, UVCoords>();

        Texture texture = GetComponent<MeshRenderer>().material.mainTexture;
        float textureWidth = texture.width;
        float textureHeight = texture.height;

        uvCoordsDictionary[uv.TileMapSprite] = new UVCoords
        {
            uv00 = new Vector2(uv.uv00Pixels.x / textureWidth, uv.uv00Pixels.y / textureHeight),
            uv11 = new Vector2(uv.uv11Pixels.x / textureWidth, uv.uv11Pixels.y / textureHeight),
        };

        tileMapSprite = uv.TileMapSprite;

        MeshUtils.CreateEmptyMeshArrays(4, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles);
        MeshUtils.AddToMeshArrays(vertices, uvs, triangles, 1, transform.position - new Vector3(-27, 110), 0f, new Vector3(25, 25), uvCoordsDictionary[uv.TileMapSprite].uv00, uvCoordsDictionary[uv.TileMapSprite].uv11);

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
    }
}
