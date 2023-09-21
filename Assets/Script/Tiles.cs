using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    public Sprite[] tileSet;
    public GameObject tilePrefab;
    public TileData[,,] tileDatas; 
    public int width, height, batch;

    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        width = 20;
        height = 10;
        batch = 1;

        tileDatas = new TileData[batch, height, width];
        for(int i = 0; i < batch; i++)
        {
            for(int j = 0; j < height; j++)
            {
                for(int k = 0; k < width; k++)
                {
                    GameObject obj = Instantiate(tilePrefab, transform);
                    obj.name = $"{k}|{j}|{i}";
                    obj.transform.position = new((width - 1) * -0.3f + k * 0.3f + j * 0.3f + (width - height)*0.15f, 
                                                 k * 0.15f + j * -0.15f + (height-width)*0.1f);
                    SpriteRenderer _sprite = obj.GetComponent<SpriteRenderer>();
                    _sprite.sortingLayerName = $"floor{i}";
                    _sprite.sortingOrder = height * j + width - k;
                    int qwe = Random.Range(0, 11);
                    bool asd = true;
                    if(qwe == 0 && k != 0 && j != 0)
                    {
                        _sprite.color = Color.black;
                        asd = false;
                    }
                    tileDatas[i, j, k] = new TileData()
                    {
                        tileObject = obj,
                        sprite = _sprite,
                        tileCover = obj.GetComponentInChildren<TileCover>(),
                        node = new Node()
                        {
                            moveAble = asd,
                            pos = new Vector3Int(k, j, i)
                        }
                    };
                    tileDatas[i, j, k].tileCover.sprite.sortingLayerName = _sprite.sortingLayerName;
                    tileDatas[i, j, k].tileCover.sprite.sortingOrder = _sprite.sortingOrder + 1;
                }
            }
        }
    }
    public void TileDataReset()
    {
        foreach(TileData tileData in tileDatas)
        {
            tileData.node.Reset();
        }
    }
}
