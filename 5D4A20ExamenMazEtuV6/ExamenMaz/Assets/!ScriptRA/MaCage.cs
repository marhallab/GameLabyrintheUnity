using Assets.MazeGenerator._scriptRA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaCage : MonoBehaviour
{
    public GameObject Planche = null;
    public GameObject CameraPlongeanteGlobale = null;
    public MazeInfo mazeInfo;
    public int Bordure;

    private float grandeurX;
    private float grandeurZ;


    void Start()
    {
        Debug.Log("MaCage Start()");

        this.mazeInfo = PersistanceManagerScript.Instance.mazeInfo;

        initDimensions();
        faireLaCage();
        positionnerCamera();
    }

    void initDimensions()
    {
        grandeurX = mazeInfo.GrandeurX + Bordure * mazeInfo.FloorX * 2;
        grandeurZ = mazeInfo.GrandeurZ + Bordure * mazeInfo.FloorZ * 2;
    }


    private GameObject addGameObjectNew(GameObject go, Vector3 v3, Quaternion quaternion, Vector3 scale, bool visible)
    {
        GameObject gObj;

        gObj = Instantiate(go, v3, quaternion) as GameObject;
        gObj.transform.parent = transform;
        gObj.transform.localScale = scale;
        gObj.GetComponent<Renderer>().enabled = visible;

        return gObj;
    }

    private void positionnerCamera()
    {
        //Ref : https://www.calculat.org/fr/aire-perimetre/triangle-rectangle.html

        float angleVisionDegres = 30;
        float angleOpp = 90 - angleVisionDegres;

        float baseVue = (grandeurX < grandeurZ ? grandeurZ : grandeurX) / 2;
        float positionX = +grandeurX / 2;
        float positionZ = +grandeurZ / 2;

        float angleOppRad = angleOpp * (Mathf.PI / 180);
        float hauteur = Mathf.Tan(angleOppRad) * baseVue;
        positionX -= mazeInfo.FloorX / 2;
        positionZ -= mazeInfo.FloorZ / 2;

        CameraPlongeanteGlobale.transform.position = new Vector3(mazeInfo.V3Center.x, hauteur, mazeInfo.V3Center.z);
    }


    public void faireLaCage()
    {
        Vector3 position, scale;
        Quaternion rotation;

        float scaleXZ = mazeInfo.FloorX;
        float positionX = +grandeurX / 2 - Bordure * scaleXZ;
        float positionZ = +grandeurZ / 2 - Bordure * scaleXZ;
        float epaisseur = .02f;
        float offsetY = -epaisseur;
        positionX -= mazeInfo.FloorX / 2;
        positionZ -= mazeInfo.FloorZ / 2;

        //plancher
        position = new Vector3(positionX, offsetY, positionZ);
        rotation = Quaternion.Euler(0, 0, 0);
        scale = new Vector3(grandeurX, epaisseur, grandeurZ);
        addGameObjectNew(Planche, position, rotation, scale, true);

        //plafond
        position = new Vector3(positionX, grandeurZ / 2 + offsetY, positionZ);
        addGameObjectNew(Planche, position, rotation, scale, false);

        //mur Z +
        position = new Vector3(positionX, grandeurZ / 4 + offsetY, positionZ + grandeurZ / 2);
        rotation = Quaternion.Euler(90, 90, 90);
        scale = new Vector3(grandeurX, epaisseur, grandeurZ / 2);
        addGameObjectNew(Planche, position, rotation, scale, false);

        //mur Z -
        position = new Vector3(positionX, grandeurZ / 4 + offsetY, positionZ - grandeurZ / 2);
        addGameObjectNew(Planche, position, rotation, scale, false);

        //mur X +
        position = new Vector3(positionX + grandeurX / 2 + offsetY, grandeurZ / 4, positionZ);
        rotation = Quaternion.Euler(90, 0, -90);
        scale = new Vector3(grandeurZ, epaisseur, grandeurZ / 2);
        addGameObjectNew(Planche, position, rotation, scale, false);

        //mur X -
        position = new Vector3(positionX - grandeurX / 2 + offsetY, grandeurZ / 4, positionZ);
        addGameObjectNew(Planche, position, rotation, scale, false);
    }
}


