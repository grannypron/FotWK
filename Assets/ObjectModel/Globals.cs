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

    public static double ENCOUNTER_ENEMY_SIZE_MULTIPLIER = .15;
    public static int ENCOUNTER_ENEMY_SIZE_RANDOM_FACTOR = 3;

    public static int SURPRISE_CHANCE = 1;//20;
    public static float ATTACK_PRESS_CHANCE_MULTIPLIER = .012f;

    public static int MOVES_PER_TURN = 4;

    public static int MIN_RATION_SANCTUARY_HELP = 10;
    public static int MIN_WARRIOR_SANCTUARY_HELP = 10;
}
