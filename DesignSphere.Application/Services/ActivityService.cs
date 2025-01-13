//using DesignSphere.Application.Contracts.Services;
//using DesignSphere.Domain.Entites;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DesignSphere.Application.Services;

//public class ActivityService : IActivityService
//{
//    private readonly IActivityRepository _repository;

//    public ActivityService(IActivityRepository repository)
//    {
//        _repository = repository;
//    }

//    public async Task LogActivityAsync(int projectId, string activityType, string description, int userId)
//    {
//        var activity = new Conversation
//        {
//            ProjectId = projectId,
//            ActivityType = activityType,
//            Description = description,
//            UserId = userId,
//            Timestamp = DateTime.UtcNow
//        };

//        await _repository.AddAsync(activity);
//    }

//    public async Task<List<ActivityDto>> GetActivitiesAsync(int projectId)
//    {
//        var activities = await _repository.GetByProjectIdAsync(projectId);
//        return activities.Select(a => new ActivityDto
//        {
//            Id = a.Id,
//            ActivityType = a.ActivityType,
//            Description = a.Description,
//            Timestamp = a.Timestamp
//        }).ToList();
//    }
//}
