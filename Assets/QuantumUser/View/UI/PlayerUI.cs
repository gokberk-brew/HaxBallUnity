using System.Collections;
using System.Collections.Generic;
using Quantum;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerUI : MonoBehaviour
{
    public PlayerRef PlayerRef;
    [SerializeField] private TMP_Text _nickName;

    public void Init(string nickName, PlayerRef playerRef)
    {
        _nickName.text = nickName;
        PlayerRef = playerRef;
    }
}
