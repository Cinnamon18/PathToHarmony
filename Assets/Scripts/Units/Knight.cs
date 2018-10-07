﻿using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using Gameplay;
using UnityEngine;

namespace Units {
	public class Knight : MeleeUnit {
		public Knight() : base(ArmorType.Medium, 100, MoveType.ArmoredInfantry, 4, DamageType.Slash, 50, Faction.Xingata) {

		}
	}
}