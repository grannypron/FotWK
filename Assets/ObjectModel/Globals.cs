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

    public static float ENCOUNTER_CHANCE_MULTIPLIER = 1; // For quick turning on/off encounters or to scale difficulty

    public static int MIN_RATION_SANCTUARY_HELP = 10;
    public static int MIN_WARRIOR_SANCTUARY_HELP = 10;

    public static int LOOT_TELEPORT_SPELL_PERCENTAGE = 25;  //3530  IF  RND (1) < .25 THEN I%(P,1) = I%(P,1) + 1: PRINT I$(1)
    public static int LOOT_DRAGON_SLAYER_PERCENTAGE = 20;  //3540  IF  RND (1) < .2 AND I%(P,6) < 1 THEN I%(P,6) = 1: PRINT I$(6)
    public static int LOOT_SPELL_OF_SEEING_PERCENTAGE = 15; //3550  IF  RND (1) < .15 AND I%(P,8) < 1 THEN I%(P,8) = 1: PRINT I$(8)
    public static int LOOT_SPECIAL_ITEM_PERCENTAGE = 30; //3700  IF L< 10 OR RND(1) > .3 THEN RETURN

    public static int WITCH_KING_LOOT_VALUE = 2000;

    public static int PLAYER_GOLD_CAP = 30000;  // 3590  IF I%(P,4) + Z > 30000 THEN I%(P,4) = 30000:Z = 0

    public static int MAGIC_MAP_CHANCE = 100 - 70; // 3600... IF RND(1) < .7 THEN 3640

    public static int WARRIORS_CRUSHING_CHANCE = 25;  // 3840  PRINT: IF I% (P, 5) < 1 OR RND(1) < .75 THEN GOSUB 3890: GOTO 3910
    public static int DWARVES_CRUSHING_CHANCE = 50;   // 3890  IF I% (P, 22) < 1 OR RND(1) < .5 THEN RETURN
    public static int WIZARDS_CAST_CHANCE = 50;   // 3910  PRINT: IF I% (P, 11) < 1 OR RND(1) < .5 THEN 3980
    public static int ELVES_CAST_CHANCE = 35;   // 3980  PRINT: IF I% (P, 21) < 1 OR RND(1) < .65 THEN 3970
    public static int MONSTERS_CHARGE_CHANCE = 20;   // 4010  IF RND(1) < .80 THEN 4050
    public static int MONSTERS_CAST_CHANCE = 40;   // 4090  IF RND(1) < .6 THEN 3970
    public static int MONSTERS_ELVES_CAST_CHANCE = 30; // 4140  IF  RND (1) < .7 THEN 3970
    public static float WARRIORS_CRUSHING_DAMAGE_MULTIPLER = .8f;  // 3850  GOSUB 3890:M = 5:N = .8: GOSUB 3860: GOTO 3910 
    public static float DWARVES_CRUSHING_DAMAGE_MULTIPLER = 2;   // 3900 M = 22:N = 2: GOTO 3860
    public static float WIZARD_SPELL_GENERAL_DAMAGE_MULTIPLIER = 1.3f; // 3920  PRINT "YOUR "I$(11)" HAS CAST A ";:DF = INT(RND(1) * 4 + 1): ON DF GOSUB 3930,3940,3950,3960: GOSUB 1400:D = D + DF * RND(1) * 1.3 + 1: GOTO 3980
    public static int SPELL_DAMAGE_FACTOR_LIGHTNING_BOLT = 10;  // 3930  PRINT "LIGHTNING BOLT":DF = 10: GOTO 1480
    public static int SPELL_DAMAGE_FACTOR_FIRE_BALL = 15;       // 3940  PRINT "FIRE BALL":DF = 15:PP = 5: GOTO 1460
    public static int SPELL_DAMAGE_FACTOR_TOWER_OF_FLAME = 10;  // 3950  PRINT "TOWER OF FLAME":DF = 10:PP = 5: GOTO 1460
    public static int SPELL_DAMAGE_FACTOR_DEATH_RAY = 7;        // 3960  PRINT "DEATH RAY":DF = 7:PP = 2: GOTO 1460
    public static double MONSTERS_ADVANCE_COMPARE = .01;        // 2310  IF RND(1) < Y * .01 THEN PRINT "THE "O$(S);: IF X > 1 THEN PRINT "S ADVANCE";: GOTO 2340
}
