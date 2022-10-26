using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private bool nw = true;
    [SerializeField] private bool sw = true;
    [SerializeField] private bool ww = true;
    [SerializeField] private bool ew = true;

    private int x = -1;
    private int y = -1;

    private int northIndex = -1;
    private int southIndex = -1;
    private int westIndex  = -1;
    private int eastIndex  = -1;


    [SerializeField] private GameObject northWall;
    [SerializeField] private GameObject southWall;
    [SerializeField] private GameObject westWall;
    [SerializeField] private GameObject eastWall;

    void Start()
    {
        DrawWalls();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveWall(int index, int dest) {
        var grid = MazeGenerator.grid;
        (x, y) = MazeGenerator.IndexToCoords(index);

        northIndex  = MazeGenerator.CoordsToIndex(x, y + 1);
        southIndex  = MazeGenerator.CoordsToIndex(x, y - 1);
        westIndex   = MazeGenerator.CoordsToIndex(x-1, y);
        eastIndex   = MazeGenerator.CoordsToIndex(x+1, y);

        checkWall(dest);
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public void DrawWalls() {
        northWall.SetActive(nw);
        southWall.SetActive(sw);
        westWall.SetActive(ww);
        eastWall.SetActive(ew);
    }

    private void checkWall(int index) {
        nw = nw && northIndex != index;
        sw = sw && southIndex != index;
        ww = ww && westIndex  != index;
        ew = ew && eastIndex  != index;
    }
}
