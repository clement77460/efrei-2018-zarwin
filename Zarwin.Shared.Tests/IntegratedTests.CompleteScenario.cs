using System.Collections.Generic;
using Zarwin.Shared.Contracts.Input;
using Zarwin.Shared.Contracts.Output;

namespace Zarwin.Shared.Tests
{
    public partial class IntegratedTests
    {
        public static IEnumerable<Scenario> Scenarios
        {
            get
            {
                yield return new Scenario(
                    "No Soldier",
                    "v2",

                    new Parameters(
                        1,
                        new FirstSoldierDamageDispatcher(),
                        new HordeParameters(1),
                        new CityParameters(0)),

                    new Result(
                        new WaveResult(
                            new TurnResult(
                                soldiers: new SoldierState[0],
                                horde: new HordeState(1),
                                wallHealthPoints: 0))));



                yield return new Scenario(
                    "1 Soldier, 1 Zombie, No Wall",
                    "v2",

                    new Parameters(
                        1,
                        new FirstSoldierDamageDispatcher(),
                        new HordeParameters(1),
                        new CityParameters(0),
                        new SoldierParameters(1, 1)),

                    new Result(
                        new WaveResult(
                            new TurnResult(
                                soldiers: new[] { new SoldierState(1, 1, 4) },
                                horde: new HordeState(1),
                                wallHealthPoints: 0),
                            new TurnResult(
                                soldiers: new[] { new SoldierState(1, 1, 4) },
                                horde: new HordeState(1),
                                wallHealthPoints: 0),
                            new TurnResult(
                                soldiers: new[] { new SoldierState(1, 2, 5) },
                                horde: new HordeState(0),
                                wallHealthPoints: 0))));



                yield return new Scenario(
                    "1 Soldier, 2 Zombies, No Wall",
                    "v2",

                    new Parameters(
                        1,
                        new FirstSoldierDamageDispatcher(),
                        new HordeParameters(2),
                        new CityParameters(0),
                        new SoldierParameters(1, 1)),

                    new Result(
                        new WaveResult(
                            new TurnResult(
                                soldiers: new[] { new SoldierState(1, 1, 4) },
                                horde: new HordeState(2),
                                wallHealthPoints: 0),
                            new TurnResult(
                                soldiers: new[] { new SoldierState(1, 1, 4) },
                                horde: new HordeState(2),
                                wallHealthPoints: 0),
                            new TurnResult(
                                soldiers: new[] { new SoldierState(1, 2, 4) },
                                horde: new HordeState(1),
                                wallHealthPoints: 0),
                            new TurnResult(
                                soldiers: new[] { new SoldierState(1, 3, 5) },
                                horde: new HordeState(0),
                                wallHealthPoints: 0))));
            }
        }
    }
}
