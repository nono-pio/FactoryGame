using UnityEngine;

public class RendererPlayer : MonoBehaviour
{
    private Animator animator;

    private string[] animationName = {"N", "NW", "W", "SW", "S", "SE", "E", "NE"};
    private int indexCurAnimation;
    private MoveState curMoveState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        indexCurAnimation = 4;
        animator.speed = 0;
    }

    public void SetSprite(Vector2 dir, bool isRunning)
    {
        if (dir.magnitude > 0.01f)
        {
            int indexAnim = getIndex(dir);
            if(dir.magnitude < 0.2f)
                SetAnimation(indexAnim, MoveState.idle);
            else if (isRunning)
                SetAnimation(indexAnim, MoveState.run);
            else
                SetAnimation(indexAnim, MoveState.walk);
        }
    }

    private void SetAnimation(int indexAnim, MoveState moveState)
    {
        if (indexAnim == indexCurAnimation && moveState == curMoveState)
            return;
        
        curMoveState = moveState;
        indexCurAnimation = indexAnim;
        animator.Play(animationName[indexAnim]);

        switch (moveState)
        {
            case MoveState.idle :
                animator.speed = 0;
                break;
            case MoveState.walk :
                animator.speed = 1;
                break;
            case MoveState.run :
                animator.speed = 3;
                break;
            default :
                animator.speed = 0;
                break;
        }
    }

    private int getIndex(Vector2 dir){
        Vector2 normDir = dir.normalized;

        float step = 360f/8;
        float halfStep = step/2;
        float angle = Vector2.SignedAngle(Vector2.up, normDir);

        angle += halfStep;
        if (angle < 0) angle += 360;

        float index = angle/step;
        return Mathf.FloorToInt(index);
    }
}

enum MoveState
{
    idle,
    walk,
    run
}
