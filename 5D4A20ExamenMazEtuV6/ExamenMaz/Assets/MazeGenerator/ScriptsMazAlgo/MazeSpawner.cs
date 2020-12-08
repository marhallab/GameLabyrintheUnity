using UnityEngine;
using System.Collections;
using System;
using Assets.MazeGenerator._scriptRA;
using System.Collections.Generic;

//<summary>
//Game object, that creates maze and instantiates it in scene
//</summary>
public class MazeSpawner : MonoBehaviour
{
    public enum MazeGenerationAlgorithm
    {
        PureRecursive,
        RecursiveTree,
        RandomTree,
        OldestTree,
        RecursiveDivision,
    }

    public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.RecursiveTree;
    public bool FullRandom = false;
    public int RandomSeed = 12345;

    public GameObject joueur = null;
    public GameObject Destroyer = null;
    public GameObject Floor = null;
    public GameObject Wall = null;
    public GameObject Pillar = null;
    public GameObject Pastille = null;
    public GameObject PieceDOr = null;

    public int Rows = 5;
    public int Columns = 5;

    private MazeInfo mazeInfo;

    private BasicMazeGenerator mMazeGenerator = null;


    void Start()
    {
        Debug.Log("MazeSpawner Start()");

        setMazeInfo();
        selectMazeGenerationAlgorithm();
        mMazeGenerator.GenerateMaze();
        faireLesMurs();
        faireCoinsDeMurs();
        
    }

    private void selectMazeGenerationAlgorithm()
    {
        if (!FullRandom)
        {
            UnityEngine.Random.InitState(RandomSeed);
        }
        switch (Algorithm)
        {
            case MazeGenerationAlgorithm.PureRecursive:
                mMazeGenerator = new RecursiveMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RecursiveTree:
                mMazeGenerator = new RecursiveTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RandomTree:
                mMazeGenerator = new RandomTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.OldestTree:
                mMazeGenerator = new OldestTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RecursiveDivision:
                mMazeGenerator = new DivisionMazeGenerator(Rows, Columns);
                break;
        }
    }

    private void setMazeInfo()
    {
        mazeInfo = new MazeInfo();

        mazeInfo.Rows = Rows;
        mazeInfo.Columns = Columns;
        mazeInfo.FloorX = Floor.transform.localScale.x;
        mazeInfo.FloorZ = Floor.transform.localScale.z;

        PersistanceManagerScript.Instance.mazeInfo = mazeInfo;
    }


    private GameObject addGameObject(GameObject go, Vector3 v3, Quaternion quaternion)
    {
        GameObject gObj;

        gObj = Instantiate(go, v3, quaternion) as GameObject;
        gObj.transform.parent = transform;
        return gObj;
    }


    private void faireLesMurs()
    {
        List<Vector3> listePastille = new List<Vector3>();
        List<Vector3> listeGoal = new List<Vector3>();

        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                float x = column * mazeInfo.FloorX;
                float z = row * mazeInfo.FloorZ;
                MazeCell cell = mMazeGenerator.GetMazeCell(row, column);

                addGameObject(Floor, new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0));

                if (cell.WallRight)
                {
                    addGameObject(Wall, new Vector3(x + mazeInfo.FloorX / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 90, 0));
                }

                if (cell.WallFront)
                {
                    addGameObject(Wall, new Vector3(x, 0, z + mazeInfo.FloorZ / 2) + Wall.transform.position, Quaternion.Euler(0, 0, 0));
                }

                if (cell.WallLeft)
                {
                    addGameObject(Wall, new Vector3(x - mazeInfo.FloorX / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 270, 0));
                }

                if (cell.WallBack)
                {
                    addGameObject(Wall, new Vector3(x, 0, z - mazeInfo.FloorZ / 2) + Wall.transform.position, Quaternion.Euler(0, 180, 0));
                }


                if (cell.isGoal())
                {
                    listeGoal.Add(new Vector3(x, 1, z));
                }
                else
                {
                    listePastille.Add(new Vector3(x, 1, z));
                }
            }
        }
        placerLesPatilles(listePastille);
        placerJoueur(listeGoal);
        placerDestroyer(listeGoal);
        placerLesGoals(listeGoal);
    }


    private void placerLesGoals(List<Vector3> listeGoal)
    {
        foreach (Vector3 v3 in listeGoal)
        {
            addGameObject(PieceDOr, v3, Quaternion.Euler(0, 0, 0));
        }
    }

    private void placerJoueur(List<Vector3> listPosition)
    {
        int i = UnityEngine.Random.Range(0, listPosition.Count);
        joueur.transform.position = new Vector3(listPosition[i].x, 1, listPosition[i].z);
        listPosition.RemoveAt(i);
    }

    private void placerLesPatilles(List<Vector3> listePastille)
    {
        foreach (Vector3 v3 in listePastille)
        {
            addGameObject(Pastille, v3, Quaternion.Euler(45, 45, 45));
        }
    }


    private void placerDestroyer(List<Vector3> listPosition)
    {
        int i = UnityEngine.Random.Range(0, listPosition.Count);
        Destroyer.transform.position = new Vector3(listPosition[i].x + 11, 0, listPosition[i].z+11);
        listPosition.RemoveAt(i);
    }

        private void faireCoinsDeMurs()
    {
        if (Pillar != null)
        {
            for (int row = 0; row < Rows + 1; row++)
            {
                for (int column = 0; column < Columns + 1; column++)
                {
                    float x = column * mazeInfo.FloorX;
                    float z = row * mazeInfo.FloorZ;
                    GameObject tmp = Instantiate(Pillar, new Vector3(x - mazeInfo.FloorX / 2, 0, z - mazeInfo.FloorZ / 2), Quaternion.identity) as GameObject;
                    tmp.transform.parent = transform;
                }
            }
        }

    }
}
