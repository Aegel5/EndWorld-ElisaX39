using UnityEngine;
using TinkerWorX.AccidentalNoiseLibrary;
using Assets.scripts.Generator1;

public class Generator : MonoBehaviour {

	// Adjustable variables for Unity Inspector
	[SerializeField]
	int Width = 512;
	[SerializeField]
	int Height = 512;
	[SerializeField]
	int TerrainOctaves = 6;
	[SerializeField]
	double TerrainFrequency = 1.25;
	[SerializeField]
	float DeepWater = 0.2f;
	[SerializeField]
	float ShallowWater = 0.4f;	
	[SerializeField]
	float Sand = 0.5f;
	[SerializeField]
	float Grass = 0.7f;
	[SerializeField]
	float Forest = 0.8f;
	[SerializeField]
	float Rock = 0.9f;

	// private variables
	ImplicitFractal HeightMap;
	MapData HeightData;
	Tile[,] Tiles;
    float[,] heightNormalized;

	// Our texture output gameobject
	//MeshRenderer HeightMapRenderer;

	void Start()
	{
		Initialize ();
		GetData (HeightMap, ref HeightData);
		LoadTiles ();
        var texture = TextureGenerator.GetTexture(Width, Height, Tiles);

        var HeightMapRenderer = GetComponent<MeshRenderer>();
        var terrain = GetComponent<Terrain>();

        if (terrain != null)
        {
            //terrain.terrainData.size = new Vector3(Width, Width, Height);
            //terrain.terrainData.heightmapResolution = Width;
            terrain.terrainData.SetHeights(0, 0, heightNormalized);
        }
        else if (HeightMapRenderer != null)
        {
            HeightMapRenderer.materials[0].mainTexture = texture;
        }

	}

	private void Initialize()
	{
		// Initialize the HeightMap Generator
		HeightMap = new ImplicitFractal (
            FractalType.Multi, 
		    BasisType.Simplex, 
		    InterpolationType.Quintic, 
		    TerrainOctaves, 
		    TerrainFrequency, 
		    UnityEngine.Random.Range (0, int.MaxValue));
	}
	
	// Extract data from a noise module
	private void GetData(ImplicitModuleBase module, ref MapData mapData)
	{
		mapData = new MapData (Width, Height);

		// loop through each x,y point - get height value
		for (var x = 0; x < Width; x++)
		{
			for (var y = 0; y < Height; y++)
			{
				//Sample the noise at smaller intervals
				float x1 = x / (float)Width;
				float y1 = y / (float)Height;

				float value = (float)HeightMap.Get (x1, y1);

				//keep track of the max and min values found
				if (value > mapData.Max) mapData.Max = value;
				if (value < mapData.Min) mapData.Min = value;

				mapData.Data[x,y] = value;
			}
		}	
	}

	// Build a Tile array from our data
	private void LoadTiles()
	{
        heightNormalized = new float[Width, Height];
        Tiles = new Tile[Width, Height];
		
		for (var x = 0; x < Width; x++)
		{
			for (var y = 0; y < Height; y++)
			{
				Tile t = new Tile();
				t.X = x;
				t.Y = y;
				
				float value = HeightData.Data[x, y];
				value = (value - HeightData.Min) / (HeightData.Max - HeightData.Min);

                heightNormalized[x, y] = value * 0.1f;


                t.HeightValue = value;
				
				//HeightMap Analyze
				if (value < DeepWater)  {
					t.HeightType = HeightType.DeepWater;
				}
				else if (value < ShallowWater)  {
					t.HeightType = HeightType.ShallowWater;
				}
				else if (value < Sand) {
					t.HeightType = HeightType.Sand;
				}
				else if (value < Grass) {
					t.HeightType = HeightType.Grass;
				}
				else if (value < Forest) {
					t.HeightType = HeightType.Forest;
				}
				else if (value < Rock) {
					t.HeightType = HeightType.Rock;
				}
				else  {
					t.HeightType = HeightType.Snow;
				}
				
				Tiles[x,y] = t;
			}
		}
	}


}
