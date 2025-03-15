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
    public class PersonNameService : INameService
    {
        private readonly IRepository<Person, Guid> personRepository;
        private readonly IRepository<PersonName, Guid> nameRepository;

        public PersonNameService(IRepository<Person, Guid> personRepository, IRepository<PersonName, Guid> nameRepository)
        {
            this.personRepository = personRepository;
            this.nameRepository = nameRepository;
        }

        public void Create(NameToCreateDTO nameToCreate)
        {
            var person = personRepository.GetById((Guid)nameToCreate.PersonId);
            if (person == null)
            {
                throw new Exception($"Person with id {nameToCreate.PersonId} doesn't exist");
            }

            nameRepository.Add(new PersonName
            {
                Id = nameToCreate.Id,
                PersonId = nameToCreate.PersonId,
                NameType = nameToCreate.NameType,
                Name = nameToCreate.Name,
                IsMain = nameToCreate.IsMain
            });
        }

        public void Delete(Guid id)
        {
            nameRepository.Delete(id);
        }

        public void Update(NameToUpdateDTO nameToUpdate)
        {
            var name = nameRepository.GetById(nameToUpdate.Id);
            if (name == null)
            {
                throw new Exception($"Person's name with id {nameToUpdate.Id} doesn't exist");
            }

            name.Name = nameToUpdate.Name;
            name.IsMain = nameToUpdate.IsMain;

            nameRepository.Update(name);
        }
    }
}
