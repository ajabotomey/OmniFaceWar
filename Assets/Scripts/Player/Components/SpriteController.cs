using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpriteDirection
{
    South, Southwest, West, Northwest, North, Northeast, East, Southeast
}

public class SpriteController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer currentSprite;

    [SerializeField] private Sprite[] spriteDirections; // 0 South, 1 Southwest, 2 West, 3 Northwest, 4 North, 5 Northeast, 6 East, 7 Southeast

    public void RotateSprite(float horizontal, float vertical)
    {
        // South
        if (horizontal == 0 && vertical < 0) {
            currentSprite.sprite = spriteDirections[(int)SpriteDirection.South];
        } else if (horizontal < 0 && vertical < 0) { // Southwest
            currentSprite.sprite = spriteDirections[(int)SpriteDirection.Southwest];
        } else if (horizontal < 0 && vertical == 0) { // West
            currentSprite.sprite = spriteDirections[(int)SpriteDirection.West];
        } else if (horizontal < 0 && vertical > 0) { // Northwest
            currentSprite.sprite = spriteDirections[(int)SpriteDirection.Northwest];
        } else if (horizontal == 0 && vertical > 0) { // North
            currentSprite.sprite = spriteDirections[(int)SpriteDirection.North];
        } else if (horizontal > 0 && vertical > 0) { // Northeast
            currentSprite.sprite = spriteDirections[(int)SpriteDirection.Northeast];
        } else if (horizontal > 0 && vertical == 0) { // East
            currentSprite.sprite = spriteDirections[(int)SpriteDirection.East];
        } else if (horizontal > 0 && vertical < 0) { // Southeast
            currentSprite.sprite = spriteDirections[(int)SpriteDirection.Southeast];
        }
    }
}
