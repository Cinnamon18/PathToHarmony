using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

namespace Units {
    public class Unit {

        ArmorType armor;
        WeaponType weapon;
        bool hasMovedThisTurn;

        public Unit() {
            //TODO Unit(ArmorType.Unarmored, WeaponType.None);
        }

        public Unit(ArmorType armorType, WeaponType weaponType) {
            armor = armorType;
            weapon = weaponType;
            hasMovedThisTurn = false;
        }

    }
}