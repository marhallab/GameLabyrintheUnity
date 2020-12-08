using Assets.MazeGenerator._scriptRA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistanceManagerScript : MonoBehaviour
{
    public static PersistanceManagerScript Instance { get; private set; }

    //mes variables partagées
    public MazeInfo mazeInfo;

    public PersistanceManagerScript()
    {
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            initVariablePartage();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void initVariablePartage()
    {
        // mazeInfo = new MazeInfo();
    }
}
