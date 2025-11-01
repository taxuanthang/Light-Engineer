using UnityEngine;

public class ColorGoal : Goal
{
    public Color matchColor = Color.red;


    public override void CheckWin(Bullet bullet)
    {
        switch (isAllowed)
        {
            case true:
                break;
            case false:
                return;
        }

        if (matchColor == bullet.color)
        {
            print("Thang");
        }
    }
}