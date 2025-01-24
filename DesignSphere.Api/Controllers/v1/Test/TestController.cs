using Asp.Versioning;
using AutoWrapper.Wrappers;
using DesignSphere.Domain.Entites;
using DesignSphere.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static System.Net.WebRequestMethods;

namespace DesignSphere.Api.Controllers.v1.Test;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/test")]
public class TestController : BaseApiController
{
  
    private readonly SQLContext _context;

    public TestController(SQLContext context)
    {
        _context = context;
    }

    [HttpPost("SaveDoorData")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
    public async Task<ApiResponse> SaveDoorData([FromBody] RequestDto requestDto)
    {
        if (requestDto == null)
        {
            return new ApiResponse(400, "Invalid door data.");
        }

        foreach (var doorData in requestDto.DoorPlans)
        {
            var doorEntity = new TestDoorEntity
            {
                ProjectName = requestDto.ProjectName,
                FileName = requestDto.FileName,
                DoorId = doorData.DoorId,
                DoorType = doorData.DoorType,
                FloorName = doorData.FloorName,
                DoorWidth = doorData.DoorWidth,
                DoorHeight = doorData.DoorHeight,
                LocationX = doorData.Location.X,
                LocationY = doorData.Location.Y,
                LocationZ = doorData.Location.Z
            };

            _context.TestDoorPlans.Add(doorEntity);
        }

        await _context.SaveChangesAsync();

        return new ApiResponse(200, "Door data saved successfully");
    }

    public class RequestDto
    {
        public string ProjectName { get; set; }
        public string FileName { get; set; }

        public List<DoorDto> DoorPlans { get; set; }

    }
    public class DoorDto
    {
        public string DoorId { get; set; }
        public string DoorType { get; set; }
        public string FloorName { get; set; }
        public string DoorWidth { get; set; }
        public string DoorHeight { get; set; }
        public LocationDto Location { get; set; }
    }

    public class LocationDto
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }
}
