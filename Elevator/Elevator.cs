using System;
using System.Collections.Generic;
using System.Linq;

namespace Elevator
{
    public interface IElevator
    {
        public int GoTo(int floor);
        public Direction GetDirection();
        public int Move();
        public void EmergencyStop();
    }
    
    public class Elevator : IElevator
    {
        private readonly HashSet<int> _floors;
        private readonly int _secondsPerFloor;
        private int CurrentFloor { get; set; }
        private List<int> MovementPlan { get; set; }

        public Elevator(int numberOfFloors, int secondsPerFloor)
        {
            _floors = Enumerable.Range(0, numberOfFloors).ToHashSet();
            _secondsPerFloor = secondsPerFloor;
            MovementPlan = new List<int>();
        }

        // Returns estimated time to the floor, in seconds.
        public int GoTo(int floor)
        {
            if (!_floors.Contains(floor)) throw new ArgumentException("Floor does not exist in floors");
            if (floor == CurrentFloor) return 0;
            if (MovementPlan.Contains(floor)) return SecondsToFloor(floor);
            
            var previousFloor = CurrentFloor;
            var insertIndex = MovementPlan.FindIndex(nextFloor =>
            {
                if (floor.IsBetween(previousFloor, nextFloor))
                {
                    return true;
                }

                previousFloor = nextFloor;
                return false;
            });
                    
            if (insertIndex == -1)
            {
                MovementPlan.Add(floor);
            }
            else
            {
                MovementPlan.Insert(insertIndex, floor);
            }

            return SecondsToFloor(floor);
        }

        public Direction GetDirection()
        {
            if (MovementPlan.Count > 0) return CurrentFloor < MovementPlan.Last() ? Direction.Up : Direction.Down;
            return Direction.Up;
        }

        public int Move()
        {
            if (MovementPlan.Count == 0) return CurrentFloor;
            CurrentFloor = MovementPlan.First();
            MovementPlan.RemoveAt(0);
            return CurrentFloor;
        }

        public void EmergencyStop()
        {
            MovementPlan = new List<int>();
        }

        private int SecondsToFloor(int floor)
        {
            var movementPlanToFloor = MovementPlan.GetRange(0, MovementPlan.IndexOf(floor) + 1);
            var totalFloorsMoved = movementPlanToFloor.Aggregate(CurrentFloor, (i, i1) => i + Math.Abs(i - i1)) - CurrentFloor;
            return totalFloorsMoved * _secondsPerFloor;
        }
    }
    public enum Direction
    {
        Up,
        Down
    }
}


public static class NumberExtensions
{
    public static bool IsBetween(this int integer, int first, int second)
    {
        if (first < second)
        {
            return integer > first && integer < second;
        }
        if (first > second)
        {
            return integer < first && integer > second;
        }

        return false;
    }
}