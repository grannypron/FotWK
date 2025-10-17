using FotWK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public static class Globals
{
    public static int MAX_MAP_X = 40;
    public static int MAX_MAP_Y = 40;
    public static int VISIT_SCREEN_NO_EVENT_PAUSE_TIME = 2;

    public static double ENCOUNTER_ENEMY_SIZE_MULTIPLIER = .15;  // 2190 L = O(S):X =  INT (1 / L * (I%(P,5) + I%(P,22))):X =  INT (X + (X * .15 * ( RND (1) * 3 - 1))): IF X < 1 THEN X = 1
    public static int ENCOUNTER_ENEMY_SIZE_RANDOM_FACTOR = 3;  // 2190 L = O(S):X =  INT (1 / L * (I%(P,5) + I%(P,22))):X =  INT (X + (X * .15 * ( RND (1) * 3 - 1))): IF X < 1 THEN X = 1

    public static int SURPRISE_CHANCE = 1;//20;   //2220  PRINT :D = 0:D1 = 0:X3 = 1: IF  RND (1) < .2 THEN  PRINT : PRINT "YOU HAVE BEEN SURPRISED!": GOSUB 1390: GOTO 2290
    public static float ATTACK_PRESS_CHANCE_MULTIPLIER = .012f;   // 2300  PRINT "THE BATTLE RAGES ON": IF  RND (1) < Y1 * .012 THEN  GOSUB 1400: PRINT N$(P)"'S SIDE PRESSES THE ATTACK ":D = D +  RND (1) * Y1 + 1: GOSUB 3840
    public static int RUN_AWAY_CHANCE = 181;   // 2270  PRINT "WILL YOU RUN AWAY";: GOSUB 40: IF Y AND  RND (1) < .8 THEN  GOSUB 1390: PRINT "     COWARD":CO = 1: GOTO 1390

    public static int MOVES_PER_TURN = 4;

    public static int ENCOUNTER_CHANCE_MULTIPLIER = 10;//0; // For quick turning on/off encounters or to scale difficulty

    public static int MIN_RATION_SANCTUARY_HELP = 10;
    public static int MIN_WARRIOR_SANCTUARY_HELP = 10;
}
