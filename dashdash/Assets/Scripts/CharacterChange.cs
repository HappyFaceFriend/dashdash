using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChange : MonoBehaviour
{
    public SpriteRenderer head;
    public SpriteRenderer mouth;
    public SpriteRenderer body;
    //head mouth body
    public Sprite[] red;
    public Sprite[] green;
    public Sprite[] blue;
    public Sprite[] pink;
    public Sprite[] gold;


    public void ChangeImage(int character)
    {
        List<Sprite> sprites = new List<Sprite>();
        if(character == 1)
            sprites.AddRange(red);
        else if(character == 2)
            sprites.AddRange(green);
        else if(character == 3)
            sprites.AddRange(blue);
        else if(character == 4)
            sprites.AddRange(pink);
        else if(character == 5)
            sprites.AddRange(gold);

        head.sprite = sprites[0];
        mouth.sprite = sprites[1];
        body.sprite = sprites[2];
    }

}
