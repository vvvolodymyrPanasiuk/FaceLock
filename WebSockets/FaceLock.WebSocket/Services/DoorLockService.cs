﻿using FaceLock.WebSocket.LockCommunicationService;
using FaceLock.WebSocket.LockCommunicationService.WebSocketCommunicationImpl;
using FaceLock.WebSocket.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FaceLock.WebSocket.Services
{
    public class DoorLockService : DoorLock.DoorLockBase
    {
        private readonly ILockCommunicationStrategy _lockCommunication;
        private readonly ILogger<DoorLockService> _logger;
        public DoorLockService(ILockCommunicationStrategy lockCommunication,
            ILogger<DoorLockService> logger) 
        {
            _lockCommunication = lockCommunication;
            _logger = logger;
        }


        public override async Task<DoorLockServiceResponse> OpenDoorLock(DoorLockServiceRequest request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                {
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Request is null"), "Bad request");
                }
                var serialNumber = request.SerialNumber;

                // Logic to open the door lock
                var res = await _lockCommunication.SendToLockAsync(serialNumber); 
                if(res == true)
                {
                    _logger.LogInformation($"200: Door lock: OPEN");
                    return await Task.FromResult(new DoorLockServiceResponse
                    {
                        Status = 200,
                        Message = "Door lock: OPEN"
                    });
                }
                else
                {
                    _logger.LogInformation($"500: Door lock: ERROR");
                    return await Task.FromResult(new DoorLockServiceResponse
                    {
                        Status = 500,
                        Message = "Door lock: ERROR"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return await Task.FromResult(new DoorLockServiceResponse
                {
                    Status = 500,
                    Message = "Error: " + ex.Message
                });
            }        
        }

        public override async Task<DoorLockServiceResponse> AddLockToWhiteList(DoorLockServiceRequest request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                {
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Request is null"), "Bad request");
                }
                var serialNumber = request.SerialNumber;

                await _lockCommunication.AddToWhiteListAsync(serialNumber);

                _logger.LogInformation($"WhiteList: Added new serial number");
                return await Task.FromResult(new DoorLockServiceResponse
                {
                    Status = 200,
                    Message = "WhiteList: Added new serial number"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return await Task.FromResult(new DoorLockServiceResponse
                {
                    Status = 500,
                    Message = "Error: " + ex.Message
                });
            }
        }
    }
}
