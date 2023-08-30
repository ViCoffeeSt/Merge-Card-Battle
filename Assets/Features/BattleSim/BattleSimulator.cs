using System;
using System.Collections.Generic;
using Features.BattleSim.Units;
using Features.Shared;
using UnityEngine;

namespace Features.BattleSim
{
    public class BattleSimulator : MonoBehaviour
    {
        [SerializeField] private UnitsBehaviour[] _simulationSteps;

        private IReadOnlyCollection<UnitBehaviour> _units = Array.Empty<UnitBehaviour>();
        private IReadOnlyCollection<UnitBehaviour> _enemies = Array.Empty<UnitBehaviour>();

        private float _totalHealth;

        private ILevelWavesService _levelWavesService;
        private ICommand<BattleResult> _waveCompletionCommand;

        public void Construct(ILevelWavesService levelWavesService,
            ICommand<BattleResult> waveCompletionCommand)
        {
            _levelWavesService = levelWavesService;
            _waveCompletionCommand = waveCompletionCommand;

            _enemies = _levelWavesService.SpawnEnemiesWave(transform);
            foreach (var enemy in _enemies)
            {
                _totalHealth += enemy.Parameters.Health;
            }
        }

        public void Simulate(IReadOnlyCollection<UnitBehaviour> units)
        {
            _units = units;
            foreach (var unit in units)
            {
                unit.transform.SetParent(transform);
            }

            enabled = _units?.Count > 0 && _enemies?.Count > 0;
        }

        private void Update()
        {
            foreach (var step in _simulationSteps)
            {
                step.UpdateUnits(_units, _enemies);
            }

            var unitsCap = _units.Count;
            foreach (var unit in _units)
            {
                unitsCap -= unit ? 0 : 1;
            }

            var enemiesCap = _enemies.Count;
            var enemiesCurrentHealth = 0f;
            foreach (var enemy in _enemies)
            {
                enemiesCap -= enemy ? 0 : 1;
                enemiesCurrentHealth += enemy ? 0 : enemy.Parameters.Health;
            }

            var battleEnded = enemiesCap == 0 || unitsCap == 0;
            if (!battleEnded)
            {
                return;
            }

            enabled = false;

            foreach (var unit in _units)
            {
                if (!unit)
                {
                    continue;
                }

                unit.Animations.Stop();
            }

            foreach (var enemy in _enemies)
            {
                if (!enemy)
                {
                    continue;
                }

                enemy.Animations.Stop();
            }

            var percentDone = 1 - enemiesCurrentHealth / _totalHealth;
            _waveCompletionCommand.Execute(new BattleResult(enemiesCap == 0, percentDone));
        }
    }
}