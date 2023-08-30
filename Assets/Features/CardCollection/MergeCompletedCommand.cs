using System.Collections;
using System.Collections.Generic;
using Features.BattleSim;
using Features.BattleSim.Units;
using Features.GameDesign;
using Features.Shared;
using UnityEngine;

namespace Features.CardCollection
{
    public class MergeCompletedCommand : ICommand<UnitSpawnInfo[]>
    {
        private readonly GridManager _gridManager;
        private readonly BattleSimulator _battleSimulator;
        private readonly Bounds _unitsSpawnArea;
        private readonly IUnitsFactory _unitsFactory;
        private readonly IRoutineRunner _routineRunner;

        public MergeCompletedCommand(GridManager gridManager,
            BattleSimulator battleSimulator,
            Bounds unitsSpawnArea,
            IUnitsFactory unitsFactory,
            IRoutineRunner routineRunner)
        {
            _gridManager = gridManager;
            _battleSimulator = battleSimulator;
            _unitsSpawnArea = unitsSpawnArea;
            _unitsFactory = unitsFactory;
            _routineRunner = routineRunner;
        }

        public void Execute(UnitSpawnInfo[] payload)
        {
            _gridManager.ShowAll();
            _routineRunner.StartCoroutine(BeginBattleDelayed(2f, payload));
        }

        private IEnumerator BeginBattleDelayed(float delay, IReadOnlyList<UnitSpawnInfo> payload)
        {
            yield return new WaitForSeconds(delay);
            _gridManager.gameObject.SetActive(false);
            StartSimulation(payload);
        }

        private void StartSimulation(IReadOnlyList<UnitSpawnInfo> payload)
        {
            var units = new List<UnitBehaviour>(6);

            for (var i = 0; i < payload.Count; i++)
            {
                var placement = payload[i];
                if (placement.IsEmpty)
                {
                    continue;
                }

                var instance = _unitsFactory.CreateUnit(_unitsSpawnArea, placement);

                instance.name = $"[User] {placement.UnitKey} #{i:00}";
                units.Add(instance);
            }

            _battleSimulator.Simulate(units);
        }
    }
}