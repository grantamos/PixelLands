using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {
	public int width, height, seed, spawnChance, targetBlocksMined, minimumMiners;
	public bool renderTexture = false;
	private int totalMined = 0, activeMiners = 0;
	private ArrayList miners;
	public int[,] map;
	public Rect mapRect;
	
	Texture2D texture;
	
	// Use this for initialization
	public void Start () {
		if(renderTexture){
			texture = new Texture2D(width, height);
			renderer.material.SetTexture("_MainTex", texture);
		}
		
		map = new int[width, height];
		miners = new ArrayList();
		
		miners.Add(new IntVector2(width/2, height/2));
		map[width/2, height/2] = 1;
		totalMined = 1;
		activeMiners = 1;
		
		mapRect.x = width;
		mapRect.width = 0;
		mapRect.y = height;
		mapRect.height = 0;
		
		//while(totalMined < targetBlocksMined)
			//generate();
	}
	
	// Update is called once per frame
	void Update () {
		generate(false);
		if(Input.GetMouseButtonDown(1))
			Debug.Log(totalMined + ", " + targetBlocksMined + ", " + miners.Count);
	}
	
	public void generate(bool oneShot){
		do{
			if(totalMined < targetBlocksMined){
				for(int i = miners.Count-1; i >= 0; i--){
					IntVector2 m = (IntVector2)miners[i];
					IntVector2 mMoved = getNewPosition(m);
					
					miners[i] = mMoved;
					markPosition(mMoved);
					
					if(m == mMoved){
						if(miners.Count <= minimumMiners){
							while(m == getNewPosition(m)){
								m = randomDirection(m);
							}
							miners[i] = getNewPosition(m);
							markPosition((IntVector2)miners[i]);
						}
						else
							miners.RemoveAt(i);
						continue;
					}
					else if(Random.Range(0, 100) < spawnChance){
						IntVector2 newMiner = getNewPosition((IntVector2)miners[i]);
						markPosition(newMiner);
						miners.Add(newMiner);
					}
				}
				
				if(totalMined >= targetBlocksMined){
					Debug.Log("DONE");
					Debug.Log(mapRect);
				}
			}
		}while(oneShot && totalMined < targetBlocksMined);
	}
	
	void fill(){
		
	}
	
	void markPosition(IntVector2 m){
		mapRect.x = (mapRect.x > m.x)? m.x : mapRect.x;
		mapRect.y = (mapRect.y > m.y)? m.y : mapRect.y;
		
		mapRect.xMax = (mapRect.xMax < m.x)? m.x : mapRect.xMax;
		mapRect.yMax = (mapRect.yMax < m.y)? m.y : mapRect.yMax;
		
		totalMined++;
		map[m.x, m.y] = 1;
		if(texture){
			texture.SetPixel(m.x, m.y, Color.red);
			texture.Apply();
		}
	}
	
	IntVector2 getNewPosition(IntVector2 pos){
		IntVector2 newPos = pos;
		int start = Random.Range(0, 4);
		int dir = (start + 1) % 4;
		
		while(start != dir){
			switch(dir){
			case 0:
				newPos.x = pos.x + 1;
				break;
			case 1:
				newPos.y = pos.y + 1;
				break;
			case 2:
				newPos.x = pos.x - 1;
				break;
			case 3:
				newPos.y = pos.y - 1;
				break;
			}
			
			dir = (dir + 1) % 4;
			bool v = validPosition(newPos);
			
			if(validPosition(newPos) && map[newPos.x, newPos.y] == 0)
				return newPos;
			
			newPos = pos;
		}
		
		if(start == dir)
			newPos = pos;
		
		return newPos;
	}
	
	IntVector2 randomDirection(IntVector2 pos){
		int dir = Random.Range(0, 4);
		IntVector2 newPos = pos;
		
		switch(dir){
		case 0:
			newPos.x = pos.x + 1;
			break;
		case 1:
			newPos.y = pos.y + 1;
			break;
		case 2:
			newPos.x = pos.x - 1;
			break;
		case 3:
			newPos.y = pos.y - 1;
			break;
		}
		return newPos;
	}
	
	bool validPosition(IntVector2 p){
		bool v = p.x >= 0 && p.y >= 0 && p.x < width && p.y < height;
		return v;
	}
	
	public struct IntVector2{
	    public int x, y;
		
		public IntVector2(int x, int y){this.x = x; this.y = y;}
	 
	    int sqrMagnitude
	    {
	        get { return x * x + y * y; }
	    }
		
		public bool Equals(IntVector2 p)
        {
            return (x == p.x) && (y == p.y);
        }
		
		public override bool Equals(object obj)
        {
            if (obj is IntVector2)
            {
                return this.Equals((IntVector2)obj);
            }
            return false;
        }
		
		public static bool operator ==(IntVector2 lhs, IntVector2 rhs)
        {
            return lhs.Equals(rhs);
        }
		
		public static bool operator !=(IntVector2 lhs, IntVector2 rhs)
        {
            return !lhs.Equals(rhs);
        }
	}
}
