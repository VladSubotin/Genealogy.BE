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
    public class RelationService : IRelationService
    {
        private readonly IRepository<Person, Guid> personRepository;
        private readonly IRepository<Relation, Guid> relationRepository;

        public RelationService(IRepository<Person, Guid> personRepository, IRepository<Relation, Guid> relationRepository)
        {
            this.personRepository = personRepository;
            this.relationRepository = relationRepository;
        }

        public void Create(RelationToCreateDTO relationToCreate)
        {
            var personIs = personRepository.GetById((Guid)relationToCreate.PersonIsId);
            if (personIs == null)
            {
                throw new Exception($"Person with id {relationToCreate.PersonIsId} doesn't exist");
            }
            var toPerson = personRepository.GetById((Guid)relationToCreate.ToPersonId);
            if (toPerson == null)
            {
                throw new Exception($"Person with id {relationToCreate.ToPersonId} doesn't exist");
            }
            var relationExist = relationRepository.GetByField(
                r => (r.PersonIsId == relationToCreate.PersonIsId 
                && r.ToPersonId == relationToCreate.ToPersonId
                || r.PersonIsId == relationToCreate.ToPersonId
                && r.ToPersonId == relationToCreate.PersonIsId)
                && r.RelationType == relationToCreate.RelationType)
                .ToList().Count > 0;
            if (relationExist)
            {
                throw new Exception($"Relation of type {relationToCreate.RelationType} between persons with id {relationToCreate.PersonIsId} and {relationToCreate.ToPersonId} already exist");
            }
            if (relationToCreate.PersonIsId == relationToCreate.ToPersonId)
            {
                throw new Exception($"Relation have to be between different persons");
            }

            Guid id = Guid.NewGuid();
            relationRepository.Add(new Relation
            {
                Id = id,
                PersonIsId = relationToCreate.PersonIsId,
                ToPersonId = relationToCreate.ToPersonId,
                RelationType = relationToCreate.RelationType
            });
        }

        public void Delete(Guid id)
        {
            var relation = relationRepository.GetById(id);

            var person1 = personRepository.GetById((Guid)relation.PersonIsId);
            var person2 = personRepository.GetById((Guid)relation.ToPersonId);

            var isSingleRelation1 = relationRepository.GetByField(
                r => r.PersonIsId == person1.Id
                || r.ToPersonId == person1.Id)
                .ToList().Count == 1;
            var isSingleRelation2 = relationRepository.GetByField(
                r => r.PersonIsId == person2.Id
                || r.ToPersonId == person2.Id)
                .ToList().Count == 1;
            if (isSingleRelation1 || isSingleRelation2)
            {
                throw new Exception($"Person may have at least one relation with other person. Single relation can't be removed");
            }
            relationRepository.Delete(id);
        }
    }
}
