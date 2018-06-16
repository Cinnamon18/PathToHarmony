using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Sfx {

    //Maps the easy name to the file name.
    private static Dictionary<string, string> sounds = new Dictionary<string, string>();

    public static void addSound(string friendlyName, string fileName) {
        sounds.Add(friendlyName, fileName);
    }

    public static void playSound(string sound) {
        //string soundFile = sounds.Get(sound);
        //TODO
    }
}
