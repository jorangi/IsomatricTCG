using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private void Start()
    {
        Pos = new(GameManager.Inst.tileManager.width - 1, GameManager.Inst.tileManager.height - 1, GameManager.Inst.tileManager.batch - 1);
        SetPosition(Pos);
    }
}
