using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGroundDefault : MonoBehaviour
{
    public byte crackCount;

    public Sprite[] _sprites;
    private Sprite _downSpike;
    private Sprite _topSpike;
    private Sprite _doubleSpike;
    private Sprite _background_1;

    private byte blockGroundLayer = 10;
    private byte blockStoneLayer = 9;
    private byte emptyLayer = 8;
    private float hitRange = 1f;

    private byte _crackCount;
    public byte CrackCount
    {
        get { return _crackCount; }
        set { if (_crackCount < 4) { _crackCount = value ; } }
    }

    public void Hit()
    {
        InitSprites();

        if (crackCount < 4)
        {
            crackCount++;
        }

        if (crackCount == 4)
        {

            SetDeadState();
            return;
        }

        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = _sprites[crackCount+3];
    }

    private void SetDeadState()
    {
        InitSprites();
        gameObject.layer = emptyLayer;
        gameObject.GetComponent<SpriteRenderer>().sprite = _background_1;

        SetSpikesToBlock();
    }

    private void SetSpikesToBlock()
    {
        InitSprites();

        var hitDown = Physics2D.Raycast(transform.position, Vector2.down, hitRange,  1 << blockGroundLayer | 1 << emptyLayer);
        var hitTop = Physics2D.Raycast(transform.position, Vector2.up, hitRange, 1 << blockGroundLayer | 1 << emptyLayer| 1<< blockStoneLayer);

        int? hitDownLayer = hitDown.collider?.gameObject.layer;
        int? hitTopLayer = hitTop.collider?.gameObject.layer;

        //top - stone
        if(hitTopLayer == blockStoneLayer)
        {
            hitTop.collider.GetComponent<BlockStone>().MoveObjectDown(0);
        }

        //down - ground    top - ground
        if (hitDownLayer == blockGroundLayer && hitTopLayer == blockGroundLayer)
        {
            this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = _doubleSpike;
        }

        ////down - ground    top - EMPTY
        //else if (hitDownLayer == blockGroundLayer && hitTopLayer == emptyLayer)
        //{
        //    SetNextSpikes(hitTop.collider?.gameObject, Vector2.up, _topSpike);
        //    this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = _downSpike;
        //}


        //down - ground    top - empty
        else if (hitDownLayer == blockGroundLayer && (hitTopLayer == emptyLayer|| hitTopLayer == default) )
        {
            SetNextSpikes(hitTop.collider?.gameObject, Vector2.up, _topSpike);
            this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = _downSpike;
        }

        //down - empty  top - ground
        else if (hitDownLayer == emptyLayer && hitTopLayer == blockGroundLayer)
        {
            SetNextSpikes(hitTop.collider?.gameObject, Vector2.down, _downSpike);
            this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = _topSpike;
        }

        //down - not ground   top - empty
        else if (hitDownLayer != blockGroundLayer && hitTopLayer == emptyLayer)
        {
            SetNextSpikes(hitDown.collider?.gameObject, Vector2.down, _downSpike);
            SetNextSpikes(hitTop.collider?.gameObject, Vector2.up, _topSpike);
            this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
        }
    }

    private void SetNextSpikes(GameObject gameObject, Vector2 direction, Sprite sprite)
    {
        if (gameObject != null)
        {
             var hitTop = Physics2D.Raycast(gameObject.transform.position, direction, hitRange, 1 << blockGroundLayer | 1 << emptyLayer);

            if (hitTop.collider?.gameObject.layer != blockGroundLayer)
            {
                gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;

                if (hitTop.collider != null)
                {
                    SetNextSpikes(hitTop.collider.gameObject, direction, sprite);
                }
            }
            else if (hitTop.collider?.gameObject.layer == blockGroundLayer)
            {
                gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
            }
        }
    }

    private void InitSprites()
    {
        if (_sprites == null || _sprites.Length == 0)
        {
            _sprites = Resources.LoadAll<Sprite>("prefabs/sprites/tm_tile");
            _downSpike = _sprites[10];
            _topSpike = _sprites[11];
            _doubleSpike = _sprites[12];
            _background_1 = _sprites[38];
        }
    }
}