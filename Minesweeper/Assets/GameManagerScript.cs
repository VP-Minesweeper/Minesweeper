using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    // GameManagerScript is what is called a "Singleton"
    // Its makes sure on every scene there is only One GameManagerScript
    // used to keep track of gameMode beetween Scenes.
    public static GameManagerScript instance = null;
    public string gameMode;
    // Start is called before the first frame update

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else if(instance!=this) {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
