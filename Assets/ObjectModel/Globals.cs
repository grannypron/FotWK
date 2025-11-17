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

    public static int LOOT_TELEPORT_SPELL_PERCENTAGE = 25;  //3530  IF  RND (1) < .25 THEN I%(P,1) = I%(P,1) + 1: PRINT I$(1)
    public static int LOOT_DRAGON_SLAYER_PERCENTAGE = 20;  //3540  IF  RND (1) < .2 AND I%(P,6) < 1 THEN I%(P,6) = 1: PRINT I$(6)
    public static int LOOT_SPELL_OF_SEEING_PERCENTAGE = 15; //3550  IF  RND (1) < .15 AND I%(P,8) < 1 THEN I%(P,8) = 1: PRINT I$(8)
    public static int LOOT_SPECIAL_ITEM_PERCENTAGE = 30; //3700  IF L< 10 OR RND(1) > .3 THEN RETURN

    public static int WITCH_KING_LOOT_VALUE = 2000;

    public static int PLAYER_GOLD_CAP = 30000;  // 3590  IF I%(P,4) + Z > 30000 THEN I%(P,4) = 30000:Z = 0

    public static int MAGIC_MAP_CHANCE = 100 - 70; // 3600... IF RND(1) < .7 THEN 3640

}
