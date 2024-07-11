using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoTweenDemo : MonoBehaviour
{
    [SerializeField] bool MoveRightOnStart = false;
    [SerializeField] bool MoveLeftOnStart = false;
    [SerializeField] bool MoveUpOnStart = false;
    [SerializeField] bool MoveDownOnStart = false;
    [SerializeField] bool ShakeOnStart = false;
    [SerializeField] bool PanBetweenTwoPointsOnStart = false;
    [SerializeField] bool RandomlyJumpOnStart = false;
    private void Start()
    {

        if (MoveRightOnStart)
        {
            MoveRight(1);
        }

        if (MoveLeftOnStart)
        {
            MoveLeft(1);
        }

        if (MoveUpOnStart)
        {
            MoveUp(1);
        }

        if (MoveDownOnStart)
        {
            MoveDown(1);
        }

        if (ShakeOnStart)
        {
            Shake(1);
        }

        if (PanBetweenTwoPointsOnStart)
        {
            PanBetweenTwoPoints(new Vector3(-1, 0, 0), new Vector3(1, 0, 0), 1, 30);
        }
        if (RandomlyJumpOnStart)
        {
            RandomlyJump(5, 2, 5, 30);
        }
    }




    public void MoveRight(float time)
    {
        transform.DOMoveX(5, time);
    }

    public void MoveLeft(float time)
    {
        transform.DOMoveX(-5, time);
    }

    public void MoveUp(float time)
    {
        transform.DOMoveY(5, time);
    }

    public void MoveDown(float time)
    {
        transform.DOMoveY(-5, time);
    }

    public void Shake(float time)
    {
        transform.DOShakePosition(time);
    }

    public void PanBetweenTwoPoints(Vector3 startPos, Vector3 targetPos, float speed = 1, int quantity = 0)
    {
        if (quantity == 0)
        {

            return;
        }


        //ease is the type of movement curve
        Ease ease = Ease.OutCirc;

        transform.DOMove(targetPos, 1 / speed).SetEase(ease).OnComplete(() =>
        {
            transform.DOMove(startPos, 1 / speed).SetEase(ease).OnComplete(() =>
            {
                int newQuantity = quantity - 1;
                PanBetweenTwoPoints(startPos, targetPos, speed, newQuantity);
            });
        });
    }


    public void RandomlyJump(float speed, float minJumpHeight, float maxJumpHeight, int jumps = 0)
    {
        if (jumps == 0)
        {
            return;
        }

        Ease ease = Ease.InOutCubic;

        float radius = 5;
        Vector2 horizontalDirection = Random.insideUnitCircle * radius;
        Vector3 randomPos = new Vector3(horizontalDirection.x, transform.position.y, horizontalDirection.y);

        float randomHeight = Random.Range(minJumpHeight, maxJumpHeight);

        transform.DOJump(randomPos, randomHeight, 1, (1 / speed) * randomHeight).SetEase(ease).OnComplete(() =>
        {
            int newJumps = jumps - 1;
            RandomlyJump(speed, minJumpHeight, maxJumpHeight, newJumps);
        });

    }



}
