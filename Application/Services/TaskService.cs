using Application.Extentions;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Application.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly IRepository<User, string> userRepository;
        private readonly IRepository<Domain.Entities.Task, Guid> taskRepository;
        private readonly IRepository<Family, Guid> familyRepository;
        private readonly IRepository<FamilyMember, Guid> familyMemberRepository;
        private readonly IRepository<Step, Guid> stepRepository;

        public TaskService(IRepository<User, string> userRepository, IRepository<Domain.Entities.Task, Guid> taskRepository, IRepository<Family, Guid> familyRepository, IRepository<FamilyMember, Guid> familyMemberRepository, IRepository<Step, Guid> stepRepository)
        {
            this.userRepository = userRepository;
            this.taskRepository = taskRepository;
            this.familyRepository = familyRepository;
            this.familyMemberRepository = familyMemberRepository;
            this.stepRepository = stepRepository;
        }

        public void Create(TaskToCreateDTO taskToCreate)
        {
            var user = userRepository.GetById(taskToCreate.UserLogin);
            if (user == null)
            {
                throw new Exception($"User {taskToCreate.UserLogin} doesn't exist");
            }
            var family = familyRepository.GetById((Guid)taskToCreate.FamilyId);
            if (family == null)
            {
                throw new Exception($"Family with id {taskToCreate.FamilyId} doesn't exist");
            }
            var familyMember = familyMemberRepository.GetByField(m => m.FamilyId == taskToCreate.FamilyId
                && m.UserLogin == taskToCreate.UserLogin);
            if (familyMember == null)
            {
                throw new Exception($"User {taskToCreate.UserLogin} isn't a member in the family with id {taskToCreate.FamilyId} doesn't exist");
            }

            Guid id = Guid.NewGuid();
            taskRepository.Add(new Domain.Entities.Task
            {
                Id = id,
                Name = taskToCreate.Name,
                Description = taskToCreate.Description,
                IsDone = false,
                CreationDate = DateOnly.FromDateTime(DateTime.Now),
                FamilyId = taskToCreate.FamilyId,
                UserLogin = taskToCreate.UserLogin
            });
        }

        public void Delete(Guid id)
        {
            taskRepository.Delete(id);
        }

        public TaskDTO get(Guid id)
        {
            var task = taskRepository.GetById(id);
            if (task == null)
            {
                throw new Exception($"Task with id {id} doesn't exist");
            }
            var user = userRepository.GetById(task.UserLogin);
            var family = familyRepository.GetById((Guid)task.FamilyId);
            var steps = stepRepository.GetByField(s => s.TaskId == id);
            List<StepDTO> stepsToReturn = new List<StepDTO>();
            foreach (var step in steps)
            {
                stepsToReturn.Add(new StepDTO
                {
                    Id = step.Id,
                    StepNum = step.StepNum,
                    IsDone = step.IsDone,
                    Purpose = step.Purpose,
                    Result = step.Result,
                    Source = step.Source,
                    SourceLocation = step.SourceLocation,
                    Term = step.Term
                });
            }

            return new TaskDTO
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                CreationDate = task.CreationDate,
                IsDone = task.IsDone,
                FamilyId = task.FamilyId,
                FamilyName = family.Name,
                UserLogin = task.UserLogin,
                UserName = user.Name,
                Steps = stepsToReturn
            };
        }

        public IEnumerable<TaskDTO> getAll(TaskOptionsDTO? taskOptions)
        {
            // set serching creterias
            Expression<Func<Domain.Entities.Task, bool>> predicate = t => t != null;
            if (!string.IsNullOrEmpty(taskOptions?.UserLogin))
            {
                Expression<Func<Domain.Entities.Task, bool>> loginPredicate = t => t.UserLogin == taskOptions.UserLogin;
                predicate = predicate.MergeAnd(loginPredicate);
            }
            if (taskOptions?.FamilyId != null)
            {
                Expression<Func<Domain.Entities.Task, bool>> familyPredicate = t => t.FamilyId == taskOptions.FamilyId;
                predicate = predicate.MergeAnd(familyPredicate);
            }
            if (taskOptions?.IsDone != null)
            {
                Expression<Func<Domain.Entities.Task, bool>> isDonePredicate = t => t.IsDone == taskOptions.IsDone;
                predicate = predicate.MergeAnd(isDonePredicate);
            }
            if (!string.IsNullOrEmpty(taskOptions?.TaskName))
            {
                Expression<Func<Domain.Entities.Task, bool>> taskNamePredicate = t => (t.Name ?? "")
                    .Trim()
                    .IndexOf(taskOptions.TaskName.Trim()) == 0;
                predicate = predicate.MergeAnd(taskNamePredicate);
            }

            // get tasks
            List<TaskDTO> tasksToShow = new List<TaskDTO>();
            var tasks = taskRepository.GetByField(predicate);
            foreach (var t in tasks)
            {
                var family = familyRepository.GetById((Guid)t.FamilyId);
                var user = userRepository.GetById(t.UserLogin);
                
                List<StepDTO> stepsToShow = new List<StepDTO>();
                var steps = stepRepository.GetByField(s => s.TaskId == t.Id);
                foreach (var s in steps)
                {
                    stepsToShow.Add(new StepDTO
                    {
                        Id = s.Id,
                        StepNum = s.StepNum,
                        Purpose = s.Purpose,
                        Source = s.Source,
                        SourceLocation = s.SourceLocation,
                        Term = s.Term,
                        Result = s.Result,
                        IsDone = s.IsDone
                    });
                }
                tasksToShow.Add(new TaskDTO
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    CreationDate = t.CreationDate,
                    IsDone = t.IsDone,
                    FamilyId = t.FamilyId,
                    FamilyName = family?.Name,
                    UserLogin = t.UserLogin,
                    UserName = user?.Name,
                    Steps = stepsToShow
                });
            }
            return tasksToShow;
        }

        public void Update(TaskToUpdateDTO taskToUpdate)
        {
            var task = taskRepository.GetById(taskToUpdate.Id);
            if (task == null)
            {
                throw new Exception($"Task with id {taskToUpdate.Id} doesn't exist");
            }
            var user = userRepository.GetById(taskToUpdate.UserLogin);
            if (user == null)
            {
                throw new Exception($"User {taskToUpdate.UserLogin} doesn't exist");
            }
            var familyMember = familyMemberRepository.GetByField(m => m.FamilyId == task.FamilyId
                && m.UserLogin == taskToUpdate.UserLogin);
            if (familyMember == null)
            {
                throw new Exception($"User {taskToUpdate.UserLogin} isn't a member in the family with id {task.FamilyId} doesn't exist");
            }

            task.Name = taskToUpdate.Name;
            task.Description = taskToUpdate.Description;
            task.IsDone = taskToUpdate.IsDone;
            task.UserLogin = taskToUpdate.UserLogin;

            taskRepository.Update(task);
        }
    }
}
