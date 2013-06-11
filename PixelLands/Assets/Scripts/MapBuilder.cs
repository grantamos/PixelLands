using UnityEngine;
using System.Collections;

public class MapBuilder : MonoBehaviour {

	// Use this for initialization
	void Start () {
		MapGenerator mGenerator = new MapGenerator();
		mGenerator.width = 400;
		mGenerator.height = 400;
		mGenerator.targetBlocksMined = 5000;
		mGenerator.spawnChance = 10;
		mGenerator.minimumMiners = 1;
		
		mGenerator.Start();
		mGenerator.generate(true);
		
		for(int x = 0; x < mGenerator.mapRect.width; x++){
			for(int y = 0; y < mGenerator.mapRect.height; y++){
				if(mGenerator.map[(int)mGenerator.mapRect.x + x, (int)mGenerator.mapRect.y + y] > 0){
					GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
					cube.transform.parent = transform;
					var p = cube.transform.position;
					p.x = x;
					p.z = y;
					cube.transform.position = p;
				}
			}
		}
		
		gameObject.AddComponent("CombineChildren");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
