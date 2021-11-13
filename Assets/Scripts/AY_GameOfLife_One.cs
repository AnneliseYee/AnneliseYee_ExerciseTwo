using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AY_GameOfLife_One : MonoBehaviour
{
    [SerializeField]
    private Vector3Int _gridDimensions = new Vector3Int(10, 1, 10);

    private VoxelGrid _grid;
    // Start is called before the first frame update
    void Start()
    {
        //creates voxel grid
        _grid = new VoxelGrid(_gridDimensions);

        // Loops through every voxel in grid
        for (int x = 0; x < _gridDimensions.x; x++)
        {
            for (int y = 0; y < _gridDimensions.y; y++)
            {
                for (int z = 0; z < _gridDimensions.z; z++)
                {
                    //randomize which voxels are alive or dead
                    Vector3Int voxelIndex = new Vector3Int(x, y, z);                    //vector holding specific coordinates
                    Voxel currentVoxel = _grid.GetVoxelByIndex(voxelIndex);             //find the voxel using those coordinates
                    if (Random.value < 0.3f)                                            //randomly choose whether voxel is alive or dead
                    {
                        currentVoxel.Alive = false;
                    }
                        //turn off all voxel layers above 0 
                        Vector3Int voxelYIndex = new Vector3Int(x, y, z);               //creates new index of voxels 
                        Voxel currentYVoxel = _grid.GetVoxelByIndex(voxelYIndex);       //gets current voxel using coordinates
                        if (y > 0)                                                      //If the Y value is greater than 0 then voxels are not alive (only for the start)
                        {
                            currentYVoxel.Alive = false;
                        }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        PerformRaycast();
        PerformGameOfLifeIteration();
        //CopyLayersToAbove();
    }

    private void PerformRaycast()
    {
        //left(0) to do something
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))                                          //casting a ray of light, if light hits something then do something
            {
                if (hit.transform.tag == "Voxel")                                      //if light hits voxel, do something
                {
                    GameObject hitObject = hit.transform.gameObject;
                    var voxel = hitObject.GetComponent<VoxelTrigger>().AttachedVoxel;

                    //left click to toggle voxel dead or alive
                    voxel.Alive = !voxel.Alive;
                }
            }
        }
    }

    private void PerformGameOfLifeIteration()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //if press space do something
        {
            DoGameOfLifeIteration();
            //CopyLayersToAbove();
        }
    }

    public void Next() 
    {
        CopyLayer();
        DoGameOfLifeIteration();
        //CopyLayersToAbove();
    }

	private void CopyLayer()
	{
		for (int y = _gridDimensions.y - 2; y >= 0; y--)
		{
			for (int x = 0; x < _gridDimensions.x; x++)
			{
				for (int z = 0; z < _gridDimensions.z; z++)
				{
                    Vector3Int voxelIndex = new Vector3Int(x, y, z);                 //create vector holding specific coordinates
                    Voxel dumbVoxel = _grid.GetVoxelByIndex(voxelIndex);
                    Vector3Int voxelIndexAbove = new Vector3Int(x, y + 1, z);
                    print(voxelIndexAbove);
					Voxel aboveVoxel = _grid.GetVoxelByIndex(voxelIndexAbove);
					aboveVoxel.Alive = dumbVoxel.Alive;
				}
			}
		}
	}

	private void DoGameOfLifeIteration()
    {
        for (int y = 0; y < _gridDimensions.y; y++)
        {
            for (int x = 0; x < _gridDimensions.x; x++)
            {
                for (int z = 0; z < _gridDimensions.z; z++)
                {
                    Vector3Int voxelIndex = new Vector3Int(x, y, z);                 //create vector holding specific coordinates
                    Voxel currentVoxel = _grid.GetVoxelByIndex(voxelIndex);         //find the voxel using those coordinates and store in currentVoxel
					if (y < 1)                                                      //only apply GOL rules to 1st layer of voxels
                    {
                        List<Voxel> neighbours = currentVoxel.GetNeighbourList();   //find neighbours of specific voxel in currentVoxel
                        int livingNeighbours = 0;
                        foreach (Voxel neighbour in neighbours)                     //for each voxel neighbor in the list neighbors
                        {
                            if (neighbour.Alive)                                    //if  voxel is alive, add to list of neighbours
                            {
                                livingNeighbours++;
                            }
                        }

                        if (currentVoxel.Alive)
                        {
                            if (livingNeighbours == 2 || livingNeighbours == 3)     //If voxel has 2 or 3 living neighbours then voxel alive
                            {
                                currentVoxel.Alive = true;
                            }
                            else                                                    //If not, voxel is dead
                            {
                                currentVoxel.Alive = false;
                            }
                        }
                        else                                                       //If current voxel is dead and has 3 live neighbours, voxel is alive
                        {
                            if (livingNeighbours == 3)
                            {
                                currentVoxel.Alive = true;
                            }
                        }
                    }
                }
            }
        }
    }
    ////copy layer to above
    //private void CopyLayersToAbove()
    //{
    //    for (int x = 0; x < _gridDimensions.x; x++)
    //    {
    //        for (int y = 0; y < _gridDimensions.y; y++)
    //        {
    //            for (int z = 0; z < _gridDimensions.z; z++)
    //            {
    //                Vector3Int voxelIndex = new Vector3Int(x, y, z);            //create vector holding specific coordinates
    //                Voxel currentVoxel = _grid.GetVoxelByIndex(voxelIndex);     //find the voxel using those coordinates and store in currentVoxel
    //                foreach (currentVoxel in voxelIndex)                        //for each voxel
    //                {
    //                    if (currentVoxel.Alive)
    //                    {
    //                        int Y;
    //                        Y = y;
    //                        Y = Y++;                                                //add 1 to the y value
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
}
