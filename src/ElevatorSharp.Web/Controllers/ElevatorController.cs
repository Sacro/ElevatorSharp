﻿using System.Web.Mvc;
using ElevatorSharp.Domain.DataTransferObjects;
using ElevatorSharp.Game;
using ElevatorSharp.Web.ViewModels;
using Newtonsoft.Json;

namespace ElevatorSharp.Web.Controllers
{
    public class ElevatorController : BaseController
    {
        #region Elevator Events
        /// <summary>
        /// Triggered when the elevator has completed all its tasks and is not doing anything.
        /// </summary>
        /// <param name="skyscraperDto"></param>
        /// <returns></returns>
        public ContentResult Idle(SkyscraperDto skyscraperDto)
        {
            var skyscraper = SyncSkyscraper(skyscraperDto);

            // This invokes the delegate from IPlayer
            ElevatorCommands elevatorCommands;
            try
            {
                skyscraper.Elevators[skyscraperDto.EventRaisedElevatorIndex].OnIdle();
                elevatorCommands = CreateElevatorCommands(skyscraperDto, skyscraper);
            }
            catch (System.Exception e)
            {
                elevatorCommands = new ElevatorCommands {Message = e.Message};
            }

            
            var json = JsonConvert.SerializeObject(elevatorCommands);
            return Content(json, "application/json");
        }

        /// <summary>
        /// Triggered when a passenger has pressed a button inside the elevator. 
        /// This tells us which floor the passenger wants to go to.
        /// Maybe tell the elevator to go to that floor?
        /// </summary>
        /// <param name="skyscraperDto"></param>
        /// <returns></returns>
        public ContentResult FloorButtonPressed(SkyscraperDto skyscraperDto)
        {
            var skyscraper = SyncSkyscraper(skyscraperDto);

            // This invokes the delegate from IPlayer
            ElevatorCommands elevatorCommands;
            try
            {
                var eventRaisedElevatorIndex = skyscraperDto.EventRaisedElevatorIndex;
                var floorNumberPressed = skyscraperDto.Elevators[eventRaisedElevatorIndex].FloorNumberPressed;
                skyscraper.Elevators[eventRaisedElevatorIndex].OnFloorButtonPressed(floorNumberPressed);
                elevatorCommands = CreateElevatorCommands(skyscraperDto, skyscraper);
            }
            catch (System.Exception e)
            {
                elevatorCommands = new ElevatorCommands { Message = e.Message };
            }
            var json = JsonConvert.SerializeObject(elevatorCommands);
            return Content(json, "application/json");
        }

        /// <summary>
        /// Triggered slightly before the elevator will pass a floor. 
        /// A good time to decide whether to stop at that floor. 
        /// Note that this event is not triggered for the destination floor. 
        /// Direction is either "up" or "down".
        /// </summary>
        /// <param name="skyscraperDto"></param>
        /// <returns></returns>
        public ContentResult PassingFloor(SkyscraperDto skyscraperDto)
        {
            var skyscraper = SyncSkyscraper(skyscraperDto);

            // This invokes the delegate from IPlayer
            ElevatorCommands elevatorCommands;
            try
            {
                var eventRaisedElevatorIndex = skyscraperDto.EventRaisedElevatorIndex;
                var passingFloorNumber = skyscraperDto.Elevators[eventRaisedElevatorIndex].PassingFloorNumber;
                var direction = skyscraperDto.Elevators[eventRaisedElevatorIndex].Direction;
                var eventArgs = new PassingFloorEventArgs
                {
                    PassingFloorNumber = passingFloorNumber,
                    Direction = direction
                };
                skyscraper.Elevators[eventRaisedElevatorIndex].OnPassingFloor(eventArgs);
                elevatorCommands = CreateElevatorCommands(skyscraperDto, skyscraper);
            }
            catch (System.Exception e)
            {
                elevatorCommands = new ElevatorCommands { Message = e.Message };
            }

            var json = JsonConvert.SerializeObject(elevatorCommands);
            return Content(json, "application/json");
        }

        /// <summary>
        /// Triggered when the elevator has arrived at a floor.
        /// Maybe decide where to go next?
        /// </summary>
        /// <param name="skyscraperDto"></param>
        /// <returns></returns>
        public ContentResult StoppedAtFloor(SkyscraperDto skyscraperDto)
        {
            var skyscraper = SyncSkyscraper(skyscraperDto);

            // This invokes the delegate from IPlayer
            ElevatorCommands elevatorCommands;
            try
            {
                var eventRaisedElevatorIndex = skyscraperDto.EventRaisedElevatorIndex;
                skyscraper.Elevators[eventRaisedElevatorIndex].OnStoppedAtFloor(skyscraperDto.Elevators[eventRaisedElevatorIndex].StoppedAtFloorNumber);
                elevatorCommands = CreateElevatorCommands(skyscraperDto, skyscraper);
            }
            catch (System.Exception e)
            {
                elevatorCommands = new ElevatorCommands { Message = e.Message };
            }

            var json = JsonConvert.SerializeObject(elevatorCommands);
            return Content(json, "application/json");
        }
        #endregion
    }
}