using UnityEngine;
using UnityEngine.UI;

public class BoardScript : MonoBehaviour {
    public SingleBlockScript singleBlock; // building block of the game used to instantiate Blocks across the board
    public int height; // height of the board
    public int width; // width of the board
    public int bombs; // number of bombs(Mines) in the board
    public SingleBlockScript[,] myBoard; // the matrix that holds all the blocks of the board
    public bool isFirstClick = true; // flag to account for the first click on the board
    public int numberOfOppenedBlocks = 0; // number of oppened blocks without mines in them

    public GameObject panel; // gameOver pannel used by SingleBlockScript to show endscreen after finishing
    public Text endText; // Could be Game over or You WIN depending on the outcome
    public bool isGameOver = false; // Flag to make sure player cant click on board after Game over

    public StopwatchScript stopWatch; // stopWatch of the game
    void Start() { // setting up the board according to what player pressed in Main Menu Scene
        if(GameManagerScript.instance.gameMode.Equals("easy")) {
            height = 10;
            width = 10;
            bombs = 10;
        } else if(GameManagerScript.instance.gameMode.Equals("medium")) {
            height = 10;
            width = 10;
            bombs = 15;
        } else if(GameManagerScript.instance.gameMode.Equals("hard")) {
            height = 10;
            width = 20;
            bombs = 32;
            this.gameObject.transform.position = new Vector3(this.transform.position.x-5, this.transform.position.y, this.transform.position.z);

        }

        myBoard = new SingleBlockScript[width, height];
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                myBoard[i, j] = Instantiate(singleBlock, new Vector3(this.transform.position.x + i, this.transform.position.y, this.transform.position.z - j), Quaternion.identity);
                myBoard[i, j].i = i;
                myBoard[i, j].j = j;
            }
        }
    }
    void Update() {
        if (Input.GetMouseButtonUp(0) && !isGameOver) {
            SingleBlockScript rig = getRayCastHit();
            if (rig != null) {
                if (isFirstClick) { // On first click the board is generated randomly and 9 blocks around the clicked block are always clear of bombs
                    stopWatch.startStopWatch(); //starting stopwatch
                    for (int xoff = -1; xoff <= 1; xoff++) { // making sure 9 blocks around clicked block are always clear ( Makes for more enjoyable game and you cant lose on first click)
                        for (int yoff = -1; yoff <= 1; yoff++) { 
                            int x = rig.i + xoff;
                            int y = rig.j + yoff;
                            if (x > -1 && x < width && y > -1 && y < height) {
                                SingleBlockScript neighbour = myBoard[x, y];
                                neighbour.isFirstCube = true;
                            }
                        }
                    }
                    //rig.isFirstCube = true;
                    firstClickRandomize(); // randomly selecting which blocks will have bombs in them
                    isFirstClick = false;
                }
                rig.openCube();
            }

        }
        if (Input.GetMouseButtonUp(1) && !isGameOver) { // with right click we "Flag" a block ( CORE MECHANIC)
            SingleBlockScript rig = getRayCastHit();
            if (rig != null && !rig.hasBeenOpened) {
                rig.setFlag();
            }
        }
    }

    public SingleBlockScript getRayCastHit() { // returns the clicked block
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            SingleBlockScript rig = hit.collider.GetComponent<SingleBlockScript>();

            return rig;
        }
        return null;

    }

    public void firstClickRandomize() { // randomly puts bombs on blocks that are not "IsFirstCube" by the ammount of bombs according to difficulty
        for (int tempCounter = 0; tempCounter < bombs; tempCounter++) { // Bombs set up
            int i = Random.Range(0, width - 1);
            int j = Random.Range(0, height - 1);

            if (myBoard[i, j].hasBomb || myBoard[i, j].isFirstCube) {
                tempCounter--;
            }
            else {
                myBoard[i, j].hasBomb = true;
            }

        }

        for (int i = 0; i < width; i++) { // Calculates the numbers used to find the bombs during gameplay ( CORE MECHANIC )

            for (int j = 0; j < height; j++) {
                myBoard[i, j].calculateNumberOfBombs();
            }
        }
    }
}
