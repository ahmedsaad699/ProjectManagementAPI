using AutoMapper;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Enums;
using ProjectManagement.Shared.DTOs;
using ProjectManagement.Application.Commands.Auth;
using ProjectManagement.Application.Commands.Projects;
using ProjectManagement.Application.Commands.Tasks;
using TaskStatus = ProjectManagement.Domain.Enums.TaskStatus;

namespace ProjectManagement.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<RegisterCommand, User>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => (UserRole)src.Role));

        // Project mappings
        CreateMap<Project, ProjectDto>()
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => $"{src.Manager.FirstName} {src.Manager.LastName}"))
            .ForMember(dest => dest.TasksCount, opt => opt.MapFrom(src => src.Tasks.Count));

        CreateMap<CreateProjectCommand, Project>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ProjectStatus.Planning));

        CreateMap<UpdateProjectCommand, Project>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (ProjectStatus)src.Status));

        // Task mappings
        CreateMap<ProjectTask, TaskDto>()
            .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name))
            .ForMember(dest => dest.AssignedToName, opt => opt.MapFrom(src => src.AssignedTo != null ? $"{src.AssignedTo.FirstName} {src.AssignedTo.LastName}" : null))
            .ForMember(dest => dest.PriorityName, opt => opt.MapFrom(src => src.Priority.ToString()))
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<CreateTaskCommand, ProjectTask>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => TaskStatus.ToDo))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => (Priority)src.Priority));

        CreateMap<UpdateTaskCommand, ProjectTask>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (TaskStatus)src.Status))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => (Priority)src.Priority));
    }
}
