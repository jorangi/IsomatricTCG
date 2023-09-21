using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Character : MonoBehaviour
{
    private WaitForFixedUpdate fixedFrame;
    private Vector3Int pos = Vector3Int.zero;
    public Vector3Int Pos
    {
        get => pos;
        set
        {
            pos = value;
        }
    }
    private SpriteRenderer sprite;
    Coroutine moving = null;
    private int ap;
    public int AP
    {
        get => ap;
        set
        {
            ap = value;
        }
    }

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        Init();
    }
    protected virtual void Init()
    {
        AP = 20;
    }
    protected virtual void Update()
    {
    }
    public void SetPosition(Vector3Int pos)
    {
        sprite.sortingLayerName = $"floor{pos.z}";
        sprite.sortingOrder = GameManager.Inst.tileManager.width * GameManager.Inst.tileManager.height + pos.y;

        transform.position = new((GameManager.Inst.tileManager.width - 1) * -0.3f + pos.x * 0.3f + pos.y * 0.3f + (GameManager.Inst.tileManager.width - GameManager.Inst.tileManager.height) * 0.15f,
                                  pos.x * 0.15f + pos.y * -0.15f + (GameManager.Inst.tileManager.height - GameManager.Inst.tileManager.width) * 0.1f + 0.3f);

        MoveableRange(true);
    }
    public void MovePosition(Vector3Int pos)
    {
        if(moving == null)
            moving = StartCoroutine(Move(PathFind(pos)));
    }
    public IEnumerator Move(Vector3Int[] _pos)
    {
        if (_pos[0] == Pos)
        {
            Debug.Log("아무 일도 일어나지 않았다.");
            moving = null;
            yield break;
        }

        GameManager.Inst.battleManager.focus = true;
        GameManager.Inst.battleManager.curCha = this;

        for (int i = 0; i < _pos.Length; i++)
        {
            Vector3 des = new((GameManager.Inst.tileManager.width - 1) * -0.3f + _pos[i].x * 0.3f + _pos[i].y * 0.3f + (GameManager.Inst.tileManager.width - GameManager.Inst.tileManager.height) * 0.152f,
                                      _pos[i].x * 0.15f + _pos[i].y * -0.15f + (GameManager.Inst.tileManager.height - GameManager.Inst.tileManager.width) * 0.1f + 0.3f);
            Vector3Int tempDes = _pos[i];

            sprite.sortingLayerName = $"floor{tempDes.z}";
            sprite.sortingOrder = GameManager.Inst.tileManager.width * GameManager.Inst.tileManager.height + tempDes.y;

            while ((transform.position - des).sqrMagnitude > 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position, des, Time.deltaTime * GameManager.Inst.gameSpeed * 10f);
                yield return fixedFrame;
            }
            transform.position = des;
            Pos = _pos[i];
            TileData tile = GameManager.Inst.tileManager.tileDatas[tempDes.z, tempDes.y, tempDes.x];
            tile.sprite.color = Color.white;
            tile.tileCover._MoveArrow = MoveArrow.none;
            AP--;
        }
        moving = null;
        MoveableRange(true);
        GameManager.Inst.battleManager.focus = false;
    }
    public void MoveableRange(bool show)
    {
        for (int i = Mathf.Max(0, Pos.y - AP); i <= Mathf.Min(GameManager.Inst.tileManager.height - 1, Pos.y + AP); i++)
        {
            for (int j = Mathf.Max(0, Pos.x - AP); j <= Mathf.Min(GameManager.Inst.tileManager.width - 1, Pos.x + AP); j++)
            {
                TileData tile = GameManager.Inst.tileManager.tileDatas[0, i, j];
                if (Mathf.Abs(Pos.y - i) + Mathf.Abs(Pos.x - j) > AP || !tile.node.moveAble)
                    continue;
                if (show)
                    tile.sprite.color = Color.green;
                else
                    tile.sprite.color = Color.white;

            }
        }
    }
    public Vector3Int[] PathFind(Vector3Int des)
    {
        GameManager.Inst.tileManager.TileDataReset();
        if (Mathf.Abs(des.x - Pos.x) + Mathf.Abs(des.y - Pos.y) + Mathf.Abs(des.z - Pos.z) > AP)
        {
            Debug.Log("이동력이 모자르다");
            Vector3Int[] vector3Ints = new Vector3Int[1];
            vector3Ints[0] = new Vector3Int(Pos.x, Pos.y, Pos.z);
            return vector3Ints;
        }
        if (!GameManager.Inst.tileManager.tileDatas[des.z, des.y, des.x].node.moveAble)
        {
            Debug.Log("그 곳으로는 이동할 수 없다");
            Vector3Int[] vector3Ints = new Vector3Int[1];
            vector3Ints[0] = new Vector3Int(Pos.x, Pos.y, Pos.z);
            return vector3Ints;
        }
        MoveableRange(false);
        List<Node> openList = new();
        List<Node> closeList = new();
        TileData[,,] tiles = GameManager.Inst.tileManager.tileDatas;
        Node node = null;
        openList.Add(tiles[Pos.z, Pos.y, Pos.x].node);
        for (int i = Mathf.Max(0, Pos.z-1); i <= Mathf.Min(GameManager.Inst.tileManager.batch-1, Pos.z + 1); i++)
        {
            for(int j = Mathf.Max(0, Pos.y-1); j <= Mathf.Min(GameManager.Inst.tileManager.height-1, Pos.y+1); j++)
            {
                for (int k = Mathf.Max(0, Pos.x - 1); k <= Mathf.Min(GameManager.Inst.tileManager.width-1, Pos.x + 1); k++)
                {
                    if (Mathf.Abs(Pos.x - k) + Mathf.Abs(Pos.y - j) + Mathf.Abs(Pos.z - i) != 1)
                        continue;
                    openList.Add(tiles[i, j, k].node);
                    tiles[i, j, k].node.parentNode = tiles[Pos.z, Pos.y, Pos.x].node;
                    tiles[i, j, k].node.G = tiles[i, j, k].node.parentNode.G + 1;
                    tiles[i, j, k].node.H = Mathf.Abs(des.x - k) + Mathf.Abs(des.y - j) + Mathf.Abs(des.z - i);
                }
            }
        }
        openList.Remove(tiles[Pos.z, Pos.y, Pos.x].node);
        closeList.Add(tiles[Pos.z, Pos.y, Pos.x].node);
        node = tiles[Pos.z, Pos.y, Pos.x].node;
        while (true)
        {
            int max = openList[0].F;

            Node tempNode = openList[0];
            for (int l = 0; l <openList.Count; l++)
            {
                if (openList[l].F < max &&
                    !closeList.Contains(openList[l]) &&
                    openList[l].moveAble)
                {
                    tempNode = openList[l];
                    max = openList[l].F;
                }
            }
            node = tempNode;
            if(node.pos != des)
            {
                closeList.Add(node);
                openList.Remove(node);
            }
            for (int i = Mathf.Max(0, node.pos.z - 1); i <= Mathf.Min(GameManager.Inst.tileManager.batch - 1, node.pos.z + 1); i++)
            {
                for (int j = Mathf.Max(0, node.pos.y - 1); j <= Mathf.Min(GameManager.Inst.tileManager.height - 1, node.pos.y + 1); j++)
                {
                    for (int k = Mathf.Max(0, node.pos.x - 1); k <= Mathf.Min(GameManager.Inst.tileManager.width - 1, node.pos.x + 1); k++)
                    {
                        if (Mathf.Abs(node.pos.x - k) + Mathf.Abs(node.pos.y - j) + Mathf.Abs(node.pos.z - i) != 1)
                            continue;
                        tiles[i, j, k].node.H = Mathf.Abs(des.x - k) + Mathf.Abs(des.y - j) + Mathf.Abs(des.z - i);

                        if (!closeList.Contains(tiles[i, j, k].node) && node.moveAble)
                        {
                            tiles[i, j, k].node.parentNode = node;
                            if(tiles[i, j, k].node.G < tiles[i, j, k].node.parentNode.G + 1)
                            {
                                tiles[i, j, k].node.G = tiles[i, j, k].node.parentNode.G + 1;
                                openList.Add(tiles[i, j, k].node);
                                tiles[i, j, k].node.parentNode = node;
                            }
                            if (k == des.x && j == des.y && i == des.z)
                            {
                                node = tiles[i, j, k].node;
                                goto arrived;
                            }
                        }
                    }
                }
            }
            closeList.Add(node);
            openList.Remove(node);

        arrived:
            if (node == tiles[des.z, des.y, des.x].node)
            {
                break;
            }
            if (openList.Count == 0)
            {
                Vector3Int[] vector3Ints = new Vector3Int[1];
                vector3Ints[0] = new Vector3Int(Pos.x, Pos.y, Pos.z);
                return vector3Ints;
            }
        }
        List<Vector3Int> result = new();
        result.Clear();
        Node n = node;
        while(n != tiles[Pos.z, Pos.y, Pos.x].node)
        {
            result.Add(n.pos);
            tiles[n.pos.z, n.pos.y, n.pos.x].sprite.color = Color.red;
            n = n.parentNode;
        }
        result.Reverse();
        return result.ToArray();
    }
}
