using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices
{
    public interface ITaskService
    {
        void Create(TaskToCreateDTO taskToCreate);
        void Update(TaskToUpdateDTO taskToUpdate);
        void Delete(Guid id);
        TaskDTO get(Guid id);
        IEnumerable<TaskDTO> getAll(TaskOptionsDTO? taskOptions);
    }
}
