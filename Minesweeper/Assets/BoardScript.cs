using UnityEngine;
using UnityEngine.UI;

public class BoardScript : MonoBehaviour {
    // Start is called before the first frame update
    public SingleBlockScript singleBlock;
    public int height;
    public int width;
    public int bombs;
    public SingleBlockScript[,] myBoard;
    public bool isFirstClick = true;
    public int numberOfOppenedBlocks = 0;

    public GameObject panel;
    public Text endText;
    public bool isGameOver = false;
    void Start() {
        myBoard = new SingleBlockScript[width, height];
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                myBoard[i, j] = Instantiate(singleBlock, new Vector3(this.transform.position.x + i, this.transform.position.y, this.transform.position.z - j), Quaternion.identity);
                myBoard[i, j].i = i;
                myBoard[i, j].j = j;
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonUp(0) && !isGameOver) {
            SingleBlockScript rig = getRayCastHit();
            if(rig!=null) {
                if (isFirstClick) {
                    rig.isFirstCube = true;
                    firstClickRandomize();
                    isFirstClick = false;
                }
                rig.openCube();
            }

        }
        if(Input.GetMouseButtonUp(1) && !isGameOver) {
            SingleBlockScript rig = getRayCastHit();
            if(rig!=null && !rig.hasBeenOpened) {
                rig.setFlag();
            }
        }
    }

    public SingleBlockScript getRayCastHit() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            SingleBlockScript rig = hit.collider.GetComponent<SingleBlockScript>();

            return rig;
        }
        return null;

    }

    public void firstClickRandomize() {
        for (int tempCounter = 0; tempCounter < bombs; tempCounter++) {
            int i = Random.Range(0, width - 1);
            int j = Random.Range(0, height - 1);

            if (myBoard[i, j].hasBomb || myBoard[i, j].isFirstCube) {
                tempCounter--;
            }
            else {
                myBoard[i, j].hasBomb = true;
            }

        }

        for (int i = 0; i < width; i++) {

            for (int j = 0; j < height; j++) {
                myBoard[i, j].calculateNumberOfBombs();
            }
        }
    }
}
