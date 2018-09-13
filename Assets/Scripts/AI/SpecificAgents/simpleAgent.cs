using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameplay;
using Units;
using UnityEngine;

namespace AI
{
    public class simpleAgent : Agent
    {
        public simpleAgent(Battlefield battlefield, Level level, Action<UnityEngine.Object> Destroy) : base(battlefield, level, Destroy) { }

        public override async Task<Move> getMove()
        {
            List<Unit> agentsUnits = battlefield.charactersUnits[character];
            agentsUnits unit = agentsUnits[0];
            return;
        }
    }
}
