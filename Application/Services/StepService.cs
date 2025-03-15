using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Application.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class StepService : IStepService
    {
        private readonly IRepository<Step, Guid> stepRepository;
        private readonly IRepository<Domain.Entities.Task, Guid> taskRepository;

        public StepService(IRepository<Step, Guid> stepRepository, IRepository<Domain.Entities.Task, Guid> taskRepository)
        {
            this.stepRepository = stepRepository;
            this.taskRepository = taskRepository;
        }

        public void Create(StepToAddDTO stepToCreate)
        {
            var task = taskRepository.GetById((Guid)stepToCreate.TaskId);
            if (task == null)
            {
                throw new Exception($"Task with id {stepToCreate.TaskId} doesn't exist");
            }

            var lastStep = stepRepository
                .GetByField(s => s.TaskId == stepToCreate.TaskId)
                .OrderByDescending(s => s.StepNum)
                .FirstOrDefault()?
                .StepNum ?? 0;

            Guid id = Guid.NewGuid();
            stepRepository.Add(new Step
            {
                Id = id,
                Purpose = stepToCreate.Purpose,
                Source = stepToCreate.Source,
                SourceLocation = stepToCreate.SourceLocation,
                Result = stepToCreate.Result,
                IsDone = false,
                StepNum = ++lastStep,
                TaskId = stepToCreate?.TaskId,
                Term = stepToCreate?.Term
            });
        }

        public void Delete(Guid id)
        {
            var step = stepRepository.GetById(id);
            if (step != null)
            {
                var stepsToDecreseNum = stepRepository.GetByField(s => s.TaskId == step.TaskId && s.StepNum > step.StepNum);
                foreach (var s in stepsToDecreseNum)
                {
                    s.StepNum--;
                    stepRepository.Update(s);
                }
                stepRepository.Delete(id);
            }
        }

        public void Replace(StepToReplaceDTO stepToReplace)
        {
            var step1 = stepRepository.GetById(stepToReplace.StepId1);
            if (step1 == null)
            {
                throw new Exception($"Step with id {stepToReplace.StepId1} doesn't exist");
            }
            var step2 = stepRepository.GetById(stepToReplace.StepId2);
            if (step2 == null)
            {
                throw new Exception($"Step with id {stepToReplace.StepId2} doesn't exist");
            }
            if (step1.TaskId != step2.TaskId)
            {
                throw new Exception($"Steps may belong to one task");
            }
            var temp = step1.StepNum;
            step1.StepNum = step2.StepNum;
            step2.StepNum = temp;
            stepRepository.Update(step1);
            stepRepository.Update(step2);
        }

        public void Update(StepToUpdateDTO stepToUpdate)
        {
            var step = stepRepository.GetById(stepToUpdate.Id);
            if (step == null)
            {
                throw new Exception($"Step with id {stepToUpdate.Id} doesn't exist");
            }

            step.Purpose = stepToUpdate.Purpose;
            step.Source = stepToUpdate.Source;
            step.SourceLocation = stepToUpdate.SourceLocation;
            step.Term = stepToUpdate.Term;
            step.Result = stepToUpdate.Result;
            step.IsDone = stepToUpdate.IsDone;
            stepRepository.Update(step);
        }
    }
}
