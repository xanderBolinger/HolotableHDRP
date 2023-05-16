using System;

public class DiceRoller
{
    static Random rnd = new Random();

    public static int Roll(int min, int max) {
        return rnd.Next(min, max);
    }

}
