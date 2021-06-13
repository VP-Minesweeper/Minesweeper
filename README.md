# Minesweeper
# Unity Project by: Јакуп Емини(191036), Хасиб Ибрахими(191065)

## Опис на проблемот:
   Играта што ја развиваме е класичната игра Minesweeper. Линк на Minesweeper од Google: https://www.google.com/search?q=minesweeper&oq=mine&aqs=chrome.0.69i59j69i57j69i59j35i39j46i67j69i60l3.1095j0j7&sourceid=chrome&ie=UTF-8

Играчот победува кога сите бомби(Mines) се откриени, и сите други ќелии се отворени. Играта станува конкурентна бидејќи има временски резултат и резултатот се зачувува за секоја категорија на тешкотии. Има 3 нивоа на тешкотии: Easy(10x10 и 10 бомби), Medium(10x10 и 15 бомби), Hard(10x20 и 30 бомби).
   
Голема цел што имавме ние за оваа игра е да обезбедиме минималистички дизајн, по интуитивен и полесен за играње, и тоа беше една ор причините зошто одбиравме за употребиме Unity за овој пројект.

## Како се игра:

Играта се игра користејки 3 основни механизми: Кликнање со лев клик директно на не отворена ќелиа, кликнање со лев клик на отворена ќелиа, и кликнање со десен клик врз не отворена келиа со што келиата е обележана(flagged). Сите три механизми применуват различни однесувања во кодот на играта. Играта има стратегиа и е предвидливa во најголем дел. Со поголеми тешкотии играта е се по не предвидлива и потешка.

## Решение на проблемот

Во почеток имавме овие проблеми, Како да се пополни матрицата со бомби со случајни позиции, како да се калкулират броевите што помагат до наоѓање на бомбите, како да се отворат многу ќелии наеднаш ако се празнии, и однесувањето кога кликнаме на келиа што е отоврена и се најдени соседните бомби и треба да се расчистат други келии.

'Рбетот на играта се состои од две класи: SingleBlockScript и BoardScript. BoardScript e составена од матрица со SingleBlockScript и некои други параметри:

```c#
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
 }
```
```c#
public class SingleBlockScript : MonoBehaviour {
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
 }
```
Една од најинтересните решениа што правивме е функсионалноста за отворање на помногу ќелии наеднаш. Тоа е правено со користење на Алгоритмот наречан "Flood Fill". Floodfill употребува рекурзиа и заедно со openCube() ги отварува сите соседни не отворени блокови кои немат бомби околу нив.
```c#
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
                            neighbour.openCube(); //in open cube flood fill can be called again therefore the whole system being a recursion until all neighbours are cleared
                    }
                }
            }
        }
    }
```
## Упатсво за играње
Уште еднаш целта на играта е да се отварат сите ќелии кои немат блокови внатре.Има 3 тешкотии со тоа што играта станува се по не предвидлива за откривање на бомбите. За наоѓање на овие бомби ни помагат броевите во табелата. Секој број во табелата ни кажува колку бомби има на соседните блокови (8 соседни блокови).
Матрицата со блокови секогаш се креира после првиот клик, со цел да не може да губиме на првиот клик, и секогаш во блокот кој е кликиран и соседите(8) на тој блок се гарантирани да немат бомби.
![Untitled](https://user-images.githubusercontent.com/72346887/121811079-7879aa00-cc63-11eb-9474-f665eb44e0a5.png)
Како што гледаме на сликата, бројот еден ни кажува дека има една бомба во соседидте, а има само еден сосед, се знае дека тој сосед има бомба внатре.

Со десен клик може да го правиме блокот "flagged" за да знаеме на иднина дека тој блок има бомба. Но ова механика може да се употреби за да се реши играта побрзо со тоа што ако ги флагираме толку соседи колку што бројот ни кажува, и кликнаме на тој број, автоматски се отварат останатите не флагирани соседи. Ако соседите не се флагирани правилно и се отварат на тој начин играта заврши.
![Untitled1](https://user-images.githubusercontent.com/72346887/121811387-9c89bb00-cc64-11eb-9baf-4685e7fe6b4f.png)
Значи кога ке кликнаме до лев клик врз бројката 1 во сликата, тие 3 блокови ке се отварат и ако флаг е ставен правилно блокови се отварат со само 1 клик, значи заштедеме време.

Пример на целосно решена игра(easy):![Untitled2](https://user-images.githubusercontent.com/72346887/121811706-7add0380-cc65-11eb-98b8-fedfb49c74e6.png)

За секоја игра се чува времето за решавање, и за 3-те тежини(easy,medium,hard), времето се чуваа особено. Ако сегашното време е помало од претходното време во таа тежина, и играта е добиена, се чува новото време. Има можност да се избришаат Highscore со притискање на Reset Highscores.
![Untitled3](https://user-images.githubusercontent.com/72346887/121814550-f47aee80-cc71-11eb-97e5-70993ece2359.png)







