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
    public class BiographicalFactService : IBiographicalFactService
    {
        private readonly IRepository<Person, Guid> personRepository;
        private readonly IRepository<BiographicalFact, Guid> factRepository;
        public BiographicalFactService(IRepository<Person, Guid> personRepository, IRepository<BiographicalFact, Guid> factRepository)
        {
            this.personRepository = personRepository;
            this.factRepository = factRepository;
        }

        public void Create(FactToCreateDTO factToCreate)
        {
            var person = personRepository.GetById((Guid)factToCreate.PersonId);
            if (person == null)
            {
                throw new Exception($"Person with id {factToCreate.PersonId} doesn't exist");
            }

            var isSingleOnlyFactTypes = factToCreate.FactType == "Birth" || factToCreate.FactType == "Deth";
            if (isSingleOnlyFactTypes)
            {
                var isFactExist = factRepository.GetByField(f => f.PersonId == factToCreate.PersonId && f.FactType == factToCreate.FactType).ToList().Count() > 0;
                if (isFactExist)
                {
                    throw new Exception($"Person may have only one fact of birth and deth. Fact of {factToCreate.FactType} already exists");
                }
            }

            factRepository.Add(new BiographicalFact
            {
                Id = factToCreate.Id,
                PersonId = factToCreate.PersonId,
                FactType = factToCreate.FactType
            });
        }

        public void Delete(Guid id)
        {
            factRepository.Delete(id);
        }
    }
}
