using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [SerializeField]
    protected TilemapVisualizer tilemapVisualizer = null;
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;
    public Tilemap floorTilemap;
    public Tilemap wallTilemap;
    public GameObject[] objectPrefab;
    private int floor = 1;

    private void Start()
    {
        floorTilemap = tilemapVisualizer.FloorTilemap;
        wallTilemap = tilemapVisualizer.WallTilemap;
        floor = GameObject.Find("GameManager").GetComponent<GameManager>().playerData.currentFloor;
        GenerateDungeon();
        GenerateObject();
    }



    public void GenerateDungeon()
    {
        tilemapVisualizer.Clear();
        RunProceduralGeneration();
    }
    protected abstract void RunProceduralGeneration();

    private void GenerateObject()
    {
        // Obtener las coordenadas del tilemap floor y wall
        BoundsInt floorBounds = floorTilemap.cellBounds;
        BoundsInt wallBounds = wallTilemap.cellBounds;

        // Obtener todos los tiles del tilemap floor y wall
        TileBase[] floorTiles = floorTilemap.GetTilesBlock(floorBounds);
        TileBase[] wallTiles = wallTilemap.GetTilesBlock(wallBounds);

        // Lista de posiciones disponibles para colocar el objeto
        List<Vector3Int> availablePositions = new();

        // Recorrer todos los tiles del tilemap floor y agregar las posiciones disponibles a la lista
        for (int x = floorBounds.xMin; x < floorBounds.xMax; x++)
        {
            for (int y = floorBounds.yMin; y < floorBounds.yMax; y++)
            {
                Vector3Int tilePosition = new(x, y, 0);
                bool isWallTile = wallTilemap.HasTile(tilePosition);
                bool isFloorTile = floorTilemap.HasTile(tilePosition);

                if (!isWallTile && isFloorTile)
                {
                    availablePositions.Add(tilePosition);
                }
            }
        }
        // Obtener una posición aleatoria de la lista de posiciones disponibles
        Vector3Int randomPosition;
        int randomIndex;
        do
        {
            randomIndex = UnityEngine.Random.Range(0, availablePositions.Count);
            randomPosition = availablePositions[randomIndex];
        } while (!CheckAdjacentTilesEmpty(randomPosition));

        GameObject.Find("Stairs").transform.position = randomPosition;
        do
        {

            randomIndex = UnityEngine.Random.Range(0, availablePositions.Count);
            randomPosition = availablePositions[randomIndex];
        } while (!CheckAdjacentTilesEmpty(randomPosition));
        GameObject.Find("Player").transform.position = randomPosition;
        int[] a = new int[] { 10, 20, 30 };
        availablePositions.Remove(randomPosition);

        if (a.Contains(floor))
        {
            do
            {

                randomIndex = UnityEngine.Random.Range(0, availablePositions.Count);
                randomPosition = availablePositions[randomIndex];
            } while (!CheckAdjacentTilesEmpty(randomPosition));
            GameObject.Find("DownStairs").transform.position = randomPosition;

            var boss = (floor / 10) - 1;
            do
            {

                randomIndex = UnityEngine.Random.Range(0, availablePositions.Count);
                randomPosition = availablePositions[randomIndex];
            } while (!CheckAdjacentTilesEmpty(randomPosition));
            Instantiate(objectPrefab[boss], randomPosition, Quaternion.identity);


        }
        else
        {
            do
            {

                randomIndex = UnityEngine.Random.Range(0, availablePositions.Count);
                randomPosition = availablePositions[randomIndex];
            } while (!CheckAdjacentTilesEmpty(randomPosition));

            GameObject.Find("Revenant").transform.position = randomPosition;


            int prefab = (int)Math.Floor(floor / 10f);

            for (int i = 0; i < CalculateNumMonsters(); i++)
            {
                do
                {
                    randomIndex = UnityEngine.Random.Range(0, availablePositions.Count);
                    randomPosition = availablePositions[randomIndex];
                } while (!CheckAdjacentTilesEmpty(randomPosition));
                
                Instantiate(objectPrefab[prefab], randomPosition, Quaternion.identity);
            }
        }

    }

    private bool CheckAdjacentTilesEmpty(Vector3Int position)
    {
        // Obtener las posiciones adyacentes
        Vector3Int[] adjacentPositions = new Vector3Int[]
        {
            position + Vector3Int.up,
            position + Vector3Int.down,
            position + Vector3Int.left,
            position + Vector3Int.right
        };

        // Verificar si las posiciones adyacentes no tienen tiles en el Tilemap
        foreach (Vector3Int adjacentPos in adjacentPositions)
        {
            if (wallTilemap.HasTile(adjacentPos))
            {
                return false;
            }
        }

        return true;
    }
    private int CalculateNumMonsters()
    {
        // Aplica una regla personalizada para calcular la cantidad de monstruos según el nivel
        float t = Mathf.Clamp01((float)(floor - 1) / 29); // Normaliza el nivel en un rango de 0 a 1 para un piso de 1 a 30

        int numMonsters = Mathf.RoundToInt(Mathf.Lerp(4, 28, t));

        return numMonsters;
    }
}