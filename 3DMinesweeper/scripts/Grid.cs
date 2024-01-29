using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int height;
    public int width;
    public int length;
    public GameObject uncovered;
    public GameObject mine;
    public double mineCount;

    public GameObject[,,] gameGrid;
    public GameObject cell0;
    public Material emptyMaterial;

    void Start()
    {
        CreateGrid();
        GenerateNumbers();
    }

    public void CreateGrid()
    {
        if(uncovered == null || mine == null){
            Debug.LogError("uncovered or mine is unassigned stupid");
            return;
        }

        //creating empty 3d grid "gameGrid" using dimensions of height, width, length
        gameGrid = new GameObject[height, width, length];

        //for-loops to iterate through the entire 3d grid
        for(int y = 0; y < height; y++){
            for(int x = 0; x < width; x++){
                for(int z = 0; z < length; z++){                    
                    gameGrid[x, y, z] = Instantiate(uncovered, new Vector3(x, y, z), Quaternion.identity);  //instantiates a basic uncovered cell

                    Cell curr = gameGrid[x, y, z].GetComponent<Cell>();
                    curr.pos = curr.transform.position;

                    curr.x = x;
                    curr.y = y;
                    curr.z = z;
                    
                    int r = UnityEngine.Random.Range(0, 100);   //randomizer to determine if the cell will be a mine or regular cell

                    if(r < mineCount){  //if the cell will be a mine, instantiate an uncovered cell and assign it to type mine
                        curr.type = Cell.Type.Mine;
                    }else{  //if the cell is not a mine, instantiate an uncovered cell and assign it to type empty (will go back later and re-assign numbers)
                        curr.type = Cell.Type.Empty;
                    }
                    
                    gameGrid[x,y,z].transform.parent = transform;
                }
            }
        }
    }

    public void GenerateNumbers(){
        // for neighbors, check minimum left/top/closest bound vs maximum
        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                for(int z = 0; z < length; z++){
                    Cell mine = gameGrid[x, y, z].GetComponent<Cell>();
                    if(mine.type == Cell.Type.Mine){
                        int[] arr = GenerateBounds(x, y, z);
                        //iterate through neighbors, skipping the mine itself
                        for (int i = arr[0]; i < arr[1]; i++){
                            for (int j = arr[2]; j < arr[3]; j++){
                                for (int k = arr[4]; k < arr[5]; k++){
                                    Cell curr = gameGrid[i, j, k].GetComponent<Cell>();
                                    if (curr.type != Cell.Type.Mine){
                                        curr.type = Cell.Type.Number;
                                        curr.number++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public int[] GenerateBounds(int x, int y, int z){
        int left = Math.Max(0, x - 1);
        int right = Math.Min(width, x + 2);
        int top = Math.Max(0, y - 1);
        int bottom = Math.Min(height, y + 2);
        int forward = Math.Max(0, z - 1);
        int back = Math.Min(length, z + 2);

        int[] arr = new int[6] {left, right, top, bottom, forward, back};
        return arr;
    }

    // public void ClearAdjacent(Cell emptyCell){
    //     Debug.Log("ClearAdjacent called");

    //     if(emptyCell.type != Cell.Type.Empty || emptyCell.revealed){
    //         return;
    //     }

    //     Cell curr = Instantiate(cell0, emptyCell.pos, Quaternion.identity).GetComponent<Cell>();
    //     curr.revealed = true;
    //     curr.type = Cell.Type.Empty;

    //     int[] adjBounds = GenerateBounds(curr.x, curr.y, curr.z);
    //     for(int i = adjBounds[0]; i < adjBounds[1]; i++){
    //         for(int j = adjBounds[2]; j < adjBounds[3]; j++){
    //             for(int k = adjBounds[4]; k < adjBounds[5]; k++){
    //                 ClearAdjacent(gameGrid[i, j, k].GetComponent<Cell>());
    //                 Destroy(gameGrid[i, j, k]);
    //             }
    //         }
    //     }
    // }
    public void ClearAdjacent(Cell emptyCell)
    {
        if (emptyCell.revealed){
            return;
        }

        emptyCell.GetComponent<MeshRenderer>().material = emptyMaterial;
        emptyCell.revealed = true;
        emptyCell.type = Cell.Type.Empty;

        int[] adjBounds = GenerateBounds(emptyCell.x, emptyCell.y, emptyCell.z);

        for (int i = adjBounds[0]; i < adjBounds[1]; i++){
            for (int j = adjBounds[2]; j < adjBounds[3]; j++){
                for (int k = adjBounds[4]; k < adjBounds[5]; k++)
                {
                    Cell adjacentCell = gameGrid[i, j, k].GetComponent<Cell>();

                    if(adjacentCell.type != Cell.Type.Empty || emptyCell.revealed){
                        ClearAdjacent(adjacentCell);
                    }
                }
            }
        }
    }

}
