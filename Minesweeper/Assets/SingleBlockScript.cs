using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleBlockScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cube;
    
    public int i;
    public int j;

    public int numberOfBombsAround = 0;
    public int numberOfFlagsAround = 0;
    public bool hasBomb = false;
    public bool hasBeenOpened = false;
    public bool isFirstCube = false;
    public Text bombsText;

    private BoardScript myBoardScript;
    private bool isFlagged = false;


    

    
    void Start()
    {
           
        myBoardScript = FindObjectOfType<BoardScript>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setFlag() {
        isFlagged = !isFlagged;
        if(isFlagged) {
            GetComponentInChildren<Renderer>().material.color = Color.red;
        } else {
            GetComponentInChildren<Renderer>().material.color = Color.gray;

        }
    }
    public void openCube() {
        if(hasBeenOpened) {
            
            calculateNumberOfBombs();
            if(numberOfBombsAround == numberOfFlagsAround) {
                Debug.Log("should clear here");
                for (int xoff = -1; xoff <= 1; xoff++)
                {
                    for (int yoff = -1; yoff <= 1; yoff++)
                    {
                        int x = this.i + xoff;
                        int y = this.j + yoff;
                        if (xoff == 0 && yoff == 0)
                            continue;
                        if (x > -1 && x < myBoardScript.width && y > -1 && y < myBoardScript.height)
                        {
                            SingleBlockScript tempBlock = myBoardScript.myBoard[x, y];


                            if (tempBlock.hasBomb && !tempBlock.isFlagged)
                            {

                                Renderer temp = tempBlock.GetComponentInChildren<Renderer>();
                                temp.material.color = Color.black;
                                // end game panel

                                myBoardScript.isGameOver = true;
                                myBoardScript.panel.SetActive(true);
                                myBoardScript.endText.text = "Game Over!";


                            }
                            else if (!tempBlock.isFlagged)
                            {
                                if (tempBlock.numberOfBombsAround == 0)
                                    tempBlock.floodFill();
                                Debug.Log(x + ":" + y);
                                Debug.Log("hasip");
                                if (!tempBlock.hasBeenOpened)
                                    myBoardScript.numberOfOppenedBlocks++;
                                Destroy(myBoardScript.myBoard[x, y].cube);
                            }

                            if (myBoardScript.numberOfOppenedBlocks + myBoardScript.bombs == myBoardScript.width * myBoardScript.height)
                            {
                                // Game won panel

                                myBoardScript.isGameOver = true;
                                myBoardScript.panel.SetActive(true);
                                myBoardScript.endText.text = "You have Won!";
                            }
                            tempBlock.hasBeenOpened = true;

                        }
                    }
                }
                Debug.Log("should clear here");
                // clear when clicking already oppened block FEATURE
            }
        } else {
            myBoardScript.numberOfOppenedBlocks++;

        }
        hasBeenOpened = true;
        if (hasBomb) {
            Renderer temp = GetComponentInChildren<Renderer>();
            temp.material.color = Color.black;
            // end game panel

            myBoardScript.isGameOver = true;
            myBoardScript.panel.SetActive(true);
            myBoardScript.endText.text = "Game Over!";


        }
        else if (!hasBomb) {
            if (this.numberOfBombsAround == 0)
                floodFill();
            Destroy(cube);
        }

        if(myBoardScript.numberOfOppenedBlocks+myBoardScript.bombs == myBoardScript.width*myBoardScript.height) {
            // Game won panel

            myBoardScript.isGameOver = true;
            myBoardScript.panel.SetActive(true);
            myBoardScript.endText.text = "You have Won!";
        }
        
    }
    public void calculateNumberOfBombs() {
        int bombcounter = 0;
        for (int xoff = -1; xoff <= 1; xoff++) {
            for (int yoff = -1; yoff <= 1; yoff++) {
                int x = this.i + xoff;
                int y = this.j + yoff;
                if (x > -1 && x < myBoardScript.width && y > -1 && y < myBoardScript.height) {
                    if (myBoardScript.myBoard[x, y].hasBomb)
                        bombcounter++;
                    if (myBoardScript.myBoard[x, y].isFlagged) {
                        numberOfFlagsAround++;
                    }
                }
            }
        }
        this.numberOfBombsAround = bombcounter;
        updateText();
    }
    public void updateText() {
        if(numberOfBombsAround !=0)
            bombsText.text = numberOfBombsAround.ToString();
    }
    private void floodFill() {
        for(int xoff=-1; xoff<=1; xoff++) {
            for(int yoff=-1; yoff<=1; yoff++) {
                int x = this.i + xoff;
                int y = this.j + yoff;
                if(x>-1 && x<myBoardScript.width && y>-1 && y<myBoardScript.height) {
                    SingleBlockScript neighbour = myBoardScript.myBoard[x,y];
                    if(!neighbour.hasBomb) {
                        if(!neighbour.hasBeenOpened)
                            neighbour.openCube();
                    }
                }
            }
        }
    }

    
}
