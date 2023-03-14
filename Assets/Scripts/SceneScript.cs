using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour
{

    int goToNextScene;
    public LayerMask playerLayer;
    float playerCheckRadius = 2f;
    public Transform[] playerCheck;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        goToNextScene = CheckIfGoToNextScene();
        if(goToNextScene != -1)
        {
            switch(goToNextScene){
                case 0:
                    SceneManager.LoadScene(0);
                    break;
                case 1:
                    SceneManager.LoadScene(2);
                    break;
            }
        }
    }

    int CheckIfGoToNextScene()
    {
        for(int i = 0; i < playerCheck.Length; i++)
        {
            if(Physics2D.OverlapCircle(playerCheck[i].position, playerCheckRadius, playerLayer))
            {
                return i;
            }
        }

        return -1;
    }
}
