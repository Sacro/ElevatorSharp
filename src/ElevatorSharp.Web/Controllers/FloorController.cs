﻿using System.Web.Mvc;
using ElevatorSharp.Web.DataTransferObjects;
using ElevatorSharp.Web.ViewModels;
using Newtonsoft.Json;

namespace ElevatorSharp.Web.Controllers
{
    public class FloorController : BaseController
    {
        #region Floor Events
        /// <summary>
        /// Triggered when someone has pressed the up button at a floor. 
        /// Note that passengers will press the button again if they fail to enter an elevator.
        /// Maybe tell an elevator to go to this floor?
        /// </summary>
        /// <param name="floorDto"></param>
        /// <returns></returns>
        public ContentResult UpButtonPressed(FloorDto floorDto)
        {
            var skyscraper = SyncSkyscraper(floorDto);
            var floor = skyscraper.Floors[floorDto.FloorNumber];
            var elevators = skyscraper.Elevators;
            foreach (var elevator in elevators)
            {
                // We use these two queues to keep track of new destinations so that we can create elevator commands
                elevator.NewDestinations.Clear();
                elevator.JumpQueueDestinations.Clear();
            }

            // This invokes the delegate from IPlayer
            floor.OnUpButtonPressed(elevators);

            var elevatorCommands = CreateElevatorCommands(floorDto, skyscraper);
            var json = JsonConvert.SerializeObject(elevatorCommands);
            return Content(json, "application/json");
        }

        /// <summary>
        /// Triggered when someone has pressed the down button at a floor. 
        /// Note that passengers will press the button again if they fail to enter an elevator.
        /// Maybe tell an elevator to go to this floor?
        /// </summary>
        /// <param name="floorDto"></param>
        /// <returns></returns>
        public ContentResult DownButtonPressed(FloorDto floorDto)
        {
            var skyscraper = SyncSkyscraper(floorDto);
            var floor = skyscraper.Floors[floorDto.FloorNumber];
            var elevators = skyscraper.Elevators;
            foreach (var elevator in elevators)
            {
                // We use these two queues to keep track of new destinations so that we can create elevator commands
                elevator.NewDestinations.Clear();
                elevator.JumpQueueDestinations.Clear();
            }

            // This invokes the delegate from IPlayer
            floor.OnDownButtonPressed(elevators);

            var elevatorCommands = CreateElevatorCommands(floorDto, skyscraper);
            var json = JsonConvert.SerializeObject(elevatorCommands);
            return Content(json, "application/json");
        }
        #endregion
    }
}