using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleBlockScript : MonoBehaviour
{
    // SingleBlockScript is the backbone of this project and is used together with BoardScript as the Core mechanics of the game
    public GameObject cube;
    
    public int i; // used to save position of this block form the BoardScript.myBoard Matrix
    public int j;

    public int numberOfBombsAround = 0;
    public int numberOfFlagsAround = 0; // Its used for the mechanic of clearing blocks around an already oppened block(explanation later)
    public bool hasBomb = false;
    public bool hasBeenOpened = false;
    public bool isFirstCube = false; // If its first cube it will always be clear of bombs
    public Text bombsText;

    private BoardScript myBoardScript;
    private bool isFlagged = false; // holds info if block is "flagged" by player (green colored block). Block is "flagged" with right Click.
    
    void Start()
    {
        myBoardScript = FindObjectOfType<BoardScript>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setFlag() { // marking block as "flagged"
        isFlagged = !isFlagged;
        if(isFlagged) {
            GetComponentInChildren<Renderer>().material.color = new Color32(25, 255, 25, 255); // Green
        } else {
            GetComponentInChildren<Renderer>().material.color = Color.gray;

        }
    }
    public void openCube() { // when player clicks the block all the logic is handled here

        if(hasBeenOpened) { // checks if clicked block has already been opened ie: handles clicking on a number on the board (CORE MECHANIC)
            calculateNumberOfBombs(); // gets number of bombs around already opened block
            if(numberOfBombsAround == numberOfFlagsAround) { // if number of flags around clicked block is equal to number of bombs around that block we "Reveal" 9 blocks around it
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


                            if (tempBlock.hasBomb && !tempBlock.isFlagged) // there is a bomb that wasnt flagged by player(he made a mistake) therefore Game over
                            {

                                Renderer temp = tempBlock.GetComponentInChildren<Renderer>();
                                temp.material.color = Color.red;
                                // end game panel

                                myBoardScript.isGameOver = true;
                                myBoardScript.panel.SetActive(true);
                                myBoardScript.endText.text = "Game Over!";
                                myBoardScript.stopWatch.stopStopWatch(); //stoping stopwatch, this time wont be counted of course


                            }
                            else if (!tempBlock.isFlagged) // if block isnt flag open that block
                            {
                                if (tempBlock.numberOfBombsAround == 0)
                                    tempBlock.floodFill();
                                if (!tempBlock.hasBeenOpened)
                                    myBoardScript.numberOfOppenedBlocks++;
                                Destroy(myBoardScript.myBoard[x, y].cube); // this opens the block and reveals the number under it
                            }

                            if (myBoardScript.numberOfOppenedBlocks + myBoardScript.bombs == myBoardScript.width * myBoardScript.height)
                            { // if number of opened blocks + bombs on the board is equal to all the blocks in the board Game Has been WON!
                                // Game won panel

                                myBoardScript.isGameOver = true;
                                myBoardScript.panel.SetActive(true); // endScreen panel enabled
                                myBoardScript.stopWatch.stopStopWatch(); // stoping stopWatch

                            }
                            tempBlock.hasBeenOpened = true;

                        }
                    }
                }
            }
        } else {
            myBoardScript.numberOfOppenedBlocks++;

        }

        hasBeenOpened = true;
        if (hasBomb) { // player clicked direcly to a bomb therefore Game Over
            Renderer temp = GetComponentInChildren<Renderer>();
            temp.material.color = Color.red;
            // end game panel

            myBoardScript.isGameOver = true;
            myBoardScript.panel.SetActive(true);
            myBoardScript.endText.text = "Game Over!";
            myBoardScript.stopWatch.stopStopWatch();
        }
        else if (!hasBomb) {
            if (this.numberOfBombsAround == 0) // if clicked block has No bombs around we must execute "FloodFill" Algorithm
                floodFill(); 
            Destroy(cube); // revelaing number in block
        }

        if(myBoardScript.numberOfOppenedBlocks+myBoardScript.bombs == myBoardScript.width*myBoardScript.height) {
            // Game Has been Won!
            myBoardScript.isGameOver = true;
            myBoardScript.panel.SetActive(true);
            myBoardScript.endText.text = "You WIN!";
            myBoardScript.stopWatch.stopStopWatch();

        }
    }
    public void calculateNumberOfBombs() { // calculates number of bombs around this SingleBlockScript (Used by BoardScipt when setting up the board)
        int bombcounter = 0;
        numberOfFlagsAround = 0;
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
        // "FloodFill" Algorithm ( Found form Wikipedia). 
        // Works recursevly together with openBlock() function to open all neighboring blocks that dont have bombs around
        // Removes Tedius task of unnecessarily clearing obvious blocks and Enhances gameplay.
        for (int xoff=-1; xoff<=1; xoff++) {
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
