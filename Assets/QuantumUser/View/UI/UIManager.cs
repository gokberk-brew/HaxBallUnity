using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GoalUIHandler goalUIHandler;
    [SerializeField] private LobbyUIHandler lobbyUIHandler;
    
    // Start is called before the first frame update
    void Start()
    {
        // lobbyUIHandler.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
