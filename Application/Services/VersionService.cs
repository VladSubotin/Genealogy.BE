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
    public class VersionService : IVersionService
    {
        private readonly IRepository<Domain.Entities.Version, Guid> versionRepository;
        private readonly IRepository<BiographicalFact, Guid> factRepository;

        public VersionService(IRepository<Domain.Entities.Version, Guid> versionRepository, IRepository<BiographicalFact, Guid> factRepository)
        {
            this.versionRepository = versionRepository;
            this.factRepository = factRepository;
        }

        public void Create(VersionToCreateDTO versionToCreate)
        {
            var fact = factRepository.GetById((Guid)versionToCreate.BiographicalFactId);
            if (fact == null)
            {
                throw new Exception($"Fact with id {versionToCreate.BiographicalFactId} doesn't exist");
            }

            versionRepository.Add(new Domain.Entities.Version
            {
                Id = versionToCreate.Id,
                BiographicalFactId = versionToCreate.BiographicalFactId,
                DateFrom = versionToCreate.DateFrom,
                DateTo = versionToCreate.DateTo,
                Location = versionToCreate.Location,
                Place = versionToCreate.Place,
                Role = versionToCreate.Role,
                Note = versionToCreate.Note,
                Source = versionToCreate.Source,
                Veracity = versionToCreate.Veracity
            });
        }

        public void Delete(Guid id)
        {
            versionRepository.Delete(id);
        }

        public void Update(VersionToUpdateDTO versionToUpdate)
        {
            var version = versionRepository.GetById(versionToUpdate.Id);
            if (version == null)
            {
                throw new Exception($"Version with id {versionToUpdate.Id} doesn't exist");
            }

            version.Location = versionToUpdate.Location;
            version.Place = versionToUpdate.Place;
            version.Role = versionToUpdate.Role;
            version.Note = versionToUpdate.Note;
            version.Source = versionToUpdate.Source;
            version.Veracity = versionToUpdate.Veracity;
            version.DateFrom = versionToUpdate.DateFrom;
            version.DateTo = versionToUpdate.DateTo;

            versionRepository.Update(version);
        }
    }
}
