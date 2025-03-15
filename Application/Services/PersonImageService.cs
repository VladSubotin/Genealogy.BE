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
    public class PersonImageService : IImageService
    {
        private readonly IRepository<Person, Guid> personRepository;
        private readonly IRepository<PersonImage, Guid> imageRepository;

        public PersonImageService(IRepository<Person, Guid> personRepository, IRepository<PersonImage, Guid> imageRepository)
        {
            this.personRepository = personRepository;
            this.imageRepository = imageRepository;
        }

        public void Create(ImageToCreateDTO imageToCreate)
        {
            var person = personRepository.GetById((Guid)imageToCreate.PersonId);
            if (person == null)
            {
                throw new Exception($"Person with id {imageToCreate.PersonId} doesn't exist");
            }

            imageRepository.Add(new PersonImage
            {
                Id = imageToCreate.Id,
                PersonId = imageToCreate.PersonId,
                Image = imageToCreate.Image
            });
        }

        public void Delete(Guid id)
        {
            imageRepository.Delete(id);
        }
    }
}
