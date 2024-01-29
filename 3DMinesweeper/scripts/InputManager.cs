using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InputManager : MonoBehaviour
{
    public LayerMask whatIsAGridLayer;

    public GameObject uncovered;
    public GameObject mine;
    public GameObject cell0;
    public GameObject cell1;
    public GameObject cell2;
    public GameObject cell3;
    public GameObject cell4;
    public GameObject cell5;
    public GameObject cell6;
    public GameObject cell7;
    public GameObject cell8;
    public GameObject cell9;
    public GameObject cell10;
    public GameObject cell11;
    public GameObject cell12;
    public GameObject cell13;
    public GameObject cell14;
    public GameObject cell15;
    public GameObject cell16;
    public GameObject cell17;
    public GameObject cell18;
    public GameObject cell19;
    public GameObject cell20;
    public GameObject cell21;
    public GameObject cell22;
    public GameObject cell23;
    public GameObject cell24;
    public GameObject cell25;
    public GameObject cell26;

    public Material flaggedMaterial;
    public GameObject endGameWindow;
    static GameObject[] arr;

    public GameObject stopwatch;
    public Stopwatch watch;

    void Start(){
        arr = new GameObject[]{cell0, cell1, cell2, cell3, cell4, cell5, cell6, cell7, cell8, cell9, cell10, cell11, cell12, cell13, cell14, cell15, cell16, cell17, cell18, cell19, cell20, cell21, cell22, cell23, cell24, cell25, cell26};
        GameObject.DontDestroyOnLoad(this.gameObject);
        
        watch = Instantiate(stopwatch, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Stopwatch>();
    }

    void Update(){
        Cell curr = MouseIsOver();    //assigns current cell to whichever cell the mouse is coering

        if(curr != null){
            if(Input.GetMouseButtonDown(0) && !curr.revealed){    //left click to reveal the cell
                if(curr.type == Cell.Type.Mine){
                    EndGame(curr);
                }else{
                    RevealNumber(curr);
                }
            }else if(Input.GetMouseButtonDown(1) && !curr.revealed){   //flagging function
                FlagCell(curr);
            }
        }
    }

    private Cell MouseIsOver(){   //returns the clicked cell
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, whatIsAGridLayer)){
            return hitInfo.transform.GetComponent<Cell>();
        }else{
            return null;
        }
    }

    public void EndGame(Cell curr){ 
        Debug.LogError("KACHOWOWOWOWOOW LMFAO I WANNA KMS I HATE THIS");
        GameObject mineObject = Instantiate(mine, curr.transform.position, Quaternion.identity);

        watch.StopStopwatch();
        String time = watch.GetTime();
        endGameWindow.GetComponentInChildren<Text>().text = time;

        Instantiate(endGameWindow, new Vector3(0, 0, 0), Quaternion.identity);  //opens end game window
    }

    public void RevealNumber(Cell curr){
        int num = curr.number;
        Debug.Log("revealed number " + num);

        GameObject temp = Instantiate(arr[num], curr.pos, Quaternion.identity);
        Cell newCell = temp.GetComponent<Cell>();
        
        newCell.type = curr.type;
        newCell.revealed = true;

        Destroy(curr.gameObject);

        if(newCell.type == Cell.Type.Empty){
            Debug.Log("ClearAdjacent called");
            GetComponent<Grid>().ClearAdjacent(newCell);
        }else{
            newCell.transform.Rotate(0f, 0f, 180f);
            newCell.transform.position += new Vector3(2.76f, -2.76f, -0.24f);
        }
    }

    // public void RevealNumber(Cell curr){ THIS CODE WORKS DO NOT TOUCH IT
    //     int num = curr.number;
    //     Debug.Log("revealed number " + num);
        
    //     GameObject temp = Instantiate(arr[num], curr.pos, Quaternion.identity);
    //     Cell newCell = temp.GetComponent<Cell>();

    //     newCell.type = curr.type;
    //     newCell.revealed = true;

    //     if(num == 0){
    //         GetComponent<Grid>().ClearAdjacent(curr);
    //     }else{
    //         newCell.transform.Rotate(0f, 0f, 180f);
    //         newCell.transform.position += new Vector3(2.76f, -2.76f, -0.24f);
    //     }
    //     Destroy(curr.gameObject);
    // }

    public void FlagCell(Cell curr){
        Debug.Log("flagged");
        curr.GetComponent<MeshRenderer>().material = flaggedMaterial;
        curr.flagged = true;
    }
}
