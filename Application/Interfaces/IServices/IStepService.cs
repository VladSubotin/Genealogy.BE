using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices
{
    public interface IStepService
    {
        void Create(StepToAddDTO taskToCreate);
        void Update(StepToUpdateDTO taskToUpdate);
        void Replace(StepToReplaceDTO taskToUpdate);
        void Delete(Guid id);
    }
}
