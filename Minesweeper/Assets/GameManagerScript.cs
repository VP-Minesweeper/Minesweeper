using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
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
