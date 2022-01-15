using System;
using System.Linq;
using Xunit;
using Elevator;

namespace ElevatorTester
{
    public class ElevatorTests
    {
        private const int NumberOfFloors = 10;
        private const int SecondsPerFloor = 30;
        private readonly Elevator.Elevator _elevator;
        
        public ElevatorTests()
        {
            _elevator = new Elevator.Elevator(NumberOfFloors, SecondsPerFloor);
        }
        
        [Fact]
        public void Should_return_seconds_until_arrival()
        {
            Assert.Equal(SecondsPerFloor * 5, _elevator.GoTo(5));
        }
        
        [Fact]
        public void Should_go_up_then_down()
        {
            _elevator.GoTo(2);
            _elevator.Move();
            _elevator.GoTo(5);
            Assert.Equal(SecondsPerFloor * 7, _elevator.GoTo(1));
        }
        
        [Fact]
        public void Should_return_correct_direction()
        {
            Assert.Equal(Direction.Up, _elevator.GetDirection());
            _elevator.GoTo(3);
            Assert.Equal(Direction.Up, _elevator.GetDirection());
            _elevator.Move();
            _elevator.GoTo(1);
            Assert.Equal(Direction.Down, _elevator.GetDirection());
        }
        
        [Fact]
        public void Should_return_0_when_going_to_current_floor()
        {
            _elevator.GoTo(3);
            _elevator.Move();
            Assert.Equal(0, _elevator.GoTo(3));
        }
        
        [Fact]
        public void Should_return_destination_floor_when_calling_Move()
        {
            _elevator.GoTo(3);
            Assert.Equal(3, _elevator.Move());
        }
        
        [Fact]
        public void Should_return_not_change_plan_when_floor_already_in_plan()
        {
            _elevator.GoTo(3);
            _elevator.GoTo(5);
            _elevator.GoTo(3);
            _elevator.Move();
            _elevator.Move();
            Assert.Equal(5, _elevator.Move());
        }
    }
}