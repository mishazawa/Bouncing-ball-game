using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Grid
{
    public int seed;
    public int width;
    public int height;
    public Element[] items;
}

[System.Serializable]
public class Element {
    public int parent;
    public int[] children;
}


public class MazeGenerator : MonoBehaviour
{



    public static string url = "http://localhost:8080/maze?width=";
    public static Grid grid;

    static private Vector3 tileScale;

    [SerializeField] [Range(4, 99)] private int mazeSize = 15;
    [SerializeField] private float tileWidth = 5;
    [SerializeField] private TextAsset json;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject finishPrefab;

    void Start()
    {
        InitData();
    }

    private void InitData() {
        StartCoroutine(getMaze());
        Debug.Log("loading");
    }

    private IEnumerator getMaze () {
        using (UnityWebRequest req = UnityWebRequest.Get(url + mazeSize)) {

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success) {
                Debug.Log(req.error);
                grid = JsonUtility.FromJson<Grid>(json.text);
            } else {
                grid = JsonUtility.FromJson<Grid>(req.downloadHandler.text);
            }
            SetupDimensions();
            Generate();
            Debug.Log($"{grid.seed}");
        }
    }

    private void SetupDimensions() {
        tileScale = Vector3.one * tileWidth;
    }

    private void Generate() {
        var cells = new Cell[grid.items.Length];

        int x, y;
        for (int i = 0; i < grid.items.Length; i++) {
                (x, y) = IndexToCoords(i);

                var cell = Instantiate(cellPrefab);
                cell.name = $"{x}x{y}";
                cell.transform.position = new Vector3(x, y) * tileScale.x;
                cell.transform.parent   = gameObject.transform;
                cell.transform.localScale = tileScale;

                cells[i] = cell.GetComponent<Cell>();
        }

        for(int i = 0; i < cells.Length; i++) {
            var item = grid.items[i];
            var cell = cells[i];

            if (item.parent != i) {
                cells[i].RemoveWall(i, item.parent);
                cells[item.parent].RemoveWall(item.parent, i);
            }

            foreach (int child in item.children) {
                cells[i].RemoveWall(i, child);
                cells[child].RemoveWall(child, i);
            }
        }

        foreach(var cell in cells) {
            cell.DrawWalls();
        }
        // tmp
        x = Mathf.FloorToInt(Random.Range(0, grid.width));
        y = Mathf.FloorToInt(Random.Range(0, grid.height));


        var dest = InitFinish(cells[CoordsToIndex(x, y)].transform);
        var ball = InitPlayerBall();

        SetTermometer(ball, dest);
    }

    private GameObject InitPlayerBall() {
        ball.transform.position = GetRandomGridPosition();
        return ball;
    }

    private GameObject InitFinish(Transform parent) {
        var finish = Instantiate(finishPrefab);

        finish.transform.position = parent.position;
        finish.transform.parent = parent;
        return finish;
    }

    private void SetTermometer(GameObject ball, GameObject destination) {
        var term = ball.GetComponent<DistanceMeter>();
        term.SetDestination(destination.transform);
    }

    private Vector3 GetRandomGridPosition() {
        var x = Mathf.FloorToInt(Random.Range(0, grid.width));
        var y = Mathf.FloorToInt(Random.Range(0, grid.height));
        return new Vector3(x, y, 0) * tileScale.x;
    }

    public static (int, int) IndexToCoords (int index) {
        return (index % grid.width, index / grid.width);
    }

    public static int CoordsToIndex (int x, int y) {
        return x + grid.width * y;
    }
}
