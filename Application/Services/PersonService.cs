using Application.Extentions;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Application.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PersonService : IPersonService
    {

        private readonly IRepository<Person, Guid> personRepository;
        private readonly IRepository<BiographicalFact, Guid> factRepository;
        private readonly IRepository<Domain.Entities.Version, Guid> versionRepository;
        private readonly IRepository<Family, Guid> familyRepository;
        private readonly IRepository<PersonImage, Guid> imageRepository;
        private readonly IRepository<PersonName, Guid> nameRepository;
        private readonly IRepository<Relation, Guid> relationRepository;
        private readonly IRelationService relationService;
        private readonly INameService nameService;
        private readonly IBiographicalFactService factService;
        private readonly IRepository<FamilyMember, Guid> fmRepository;
        //private readonly PersonService versionService;
        //private readonly PersonService relativeService;

        public PersonService(IRepository<Person, Guid> personRepository, IRepository<BiographicalFact, Guid> factRepository, IRepository<Domain.Entities.Version, Guid> versionRepository,
            IRepository<Family, Guid> familyRepository, IRepository<PersonImage, Guid> imageRepository, IRepository<PersonName, Guid> nameRepository, IRepository<Relation, Guid> relationRepository,
            IRelationService relationService, IRepository<FamilyMember, Guid> fmRepository)
        {
            this.factRepository = factRepository;
            this.versionRepository = versionRepository;
            this.familyRepository = familyRepository;
            this.personRepository = personRepository;
            this.imageRepository = imageRepository;
            this.nameRepository = nameRepository;
            this.relationRepository = relationRepository;
            this.relationService = relationService;
            this.fmRepository = fmRepository;
            //versionService = this;
            //relativeService = this;
        }

        public Guid Create(PersonToCreateDTO personToCreate)
        {
            var family = familyRepository.GetById((Guid)personToCreate.FamilyId);
            if (family == null)
            {
                throw new Exception($"Family with id {personToCreate.FamilyId} doesn't exist");
            }

            Guid id = Guid.NewGuid();
            personRepository.Add(new Domain.Entities.Person
            {
                Id = id,
                FamilyId = personToCreate.FamilyId
            });

            if (personToCreate?.ToPersonId != null)
            {
                var relative = personRepository.GetById((Guid)personToCreate?.ToPersonId);
                if (relative != null)
                {
                    RelationToCreateDTO relation;
                    if (personToCreate.RelationType == "Child")
                    {
                        var person = personRepository.GetById((Guid)personToCreate.ToPersonId);
                        string relationType = "Other";
                        if (person.Gender == "Male")
                        {
                            relationType = "Father";
                        }
                        else if (person.Gender == "Female")
                        {
                            relationType = "Mother";
                        }
                        relation = new RelationToCreateDTO
                        {
                            PersonIsId = personToCreate.ToPersonId,
                            ToPersonId = id,
                            RelationType = relationType
                        };
                    }
                    else
                    {
                        relation = new RelationToCreateDTO
                        {
                            PersonIsId = id,
                            ToPersonId = personToCreate.ToPersonId,
                            RelationType = personToCreate.RelationType
                        };
                    }
                    relationService.Create(relation);
                }
            }
            return id;
        }

        public void Update(PersonProfileDTO personToUpdate)
        {
            var person = personRepository.GetById(personToUpdate.Id);
            if (person == null)
            {
                throw new Exception($"Person with id {personToUpdate.Id} doesn't exist");
            }

            person.Prefix = personToUpdate.Prefix;
            person.Suffix = personToUpdate.Suffix;
            person.Gender = personToUpdate.Gender;
            person.Nationality = personToUpdate.Nationality;
            person.Religion = personToUpdate.Religion;
            person.Biography = personToUpdate.Biography;

            List<PersonName> newNames = new List<PersonName>();
            foreach (var n in personToUpdate?.Names)
            {
                newNames.Add(new PersonName
                {
                    Id = n.Id,
                    Name = n.Name,
                    NameType = n.NameType,
                    IsMain = n.IsMain,
                    PersonId = person.Id
                });
            }
            person.PersonNames = newNames;

            List<PersonImage> newImages = new List<PersonImage>();
            foreach (var i in personToUpdate?.Images)
            {
                newImages.Add(new PersonImage
                {
                    Id = i.Id,
                    Image = i.Image,
                    PersonId = person.Id
                });
            }
            person.PersonImages = newImages;

            List<BiographicalFact> newFacts = new List<BiographicalFact>();
            foreach (var f in personToUpdate?.Facts)
            {
                List<Domain.Entities.Version> newVersions = new List<Domain.Entities.Version>();
                foreach (var v in f.Versions)
                {
                    newVersions.Add(new Domain.Entities.Version
                    {
                        Id = v.Id,
                        DateFrom = v.DateFrom,
                        DateTo = v.DateTo,
                        BiographicalFactId = f.Id,
                        Location = v.Location,
                        Note = v.Note,
                        Place = v.Place,
                        Role = v.Role,
                        Source = v.Source,
                        Veracity = v.Veracity
                    });
                }
                newFacts.Add(new BiographicalFact
                {
                    Id = f.Id,
                    FactType = f.FactType,
                    PersonId = person.Id,
                    Versions = newVersions
                });
            }
            person.BiographicalFacts = newFacts;
            personRepository.Update(person);
        }

        public void Delete(Guid id)
        {
            var person = personRepository.GetById(id);
            if (person != null)
            {
                var relations = relationRepository.GetByField(r => r.ToPersonId == id);
                foreach (var rel in relations)
                {
                    relationRepository.Delete(rel.Id);
                }
                
                personRepository.Delete(id);
            }
        }

        public PersonProfileDTO get(Guid id, string userLogin)
        {
            var person = personRepository.GetById(id);
            if (person == null)
            {
                throw new Exception($"Person with id {id} doesn't exist");
            }
            var family = familyRepository.GetById((Guid)person.FamilyId);

            // find names
            var names = nameRepository.GetByField(n => n.PersonId == id);
            var namesToReturn = new List<PersonNameDTO>();
            foreach (var name in names)
            {
                namesToReturn.Add(new PersonNameDTO
                {
                    Id = name.Id,
                    NameType = name.NameType,
                    Name = name.Name,
                    IsMain = name.IsMain
                });
            }

            // find images
            var images = imageRepository.GetByField(i => i.PersonId == id);
            var imagesToReturn = new List<PersonImageDTO>();
            foreach (var image in images)
            {
                imagesToReturn.Add(new PersonImageDTO
                {
                    Id = image.Id,
                    Image = image.Image
                });
            }

            // find relatives
            var relsToReturn = GetRelatives(id);

            // find facts
            var facts = factRepository.GetByField(f => f.PersonId == id);
            var factsToReturn = new List<PersonFactDTO>();

            var birthVersions = getVersions(id, "Birth");
            var dethVersions = getVersions(id, "Deth");

            foreach (var fact in facts)
            {
                // find fact versions
                var vers = versionRepository.GetByField(v => v.BiographicalFactId == fact.Id);
                var versToReturn = new List<PersonFactVersionDTO>();
                foreach (var ver in vers)
                {
                    var conflictVers = new List<ConflictVerDTO>();
                    // подія сталася раніше за народження
                    verifyVersionDate2(ver, birthVersions, (v, c) => v?.DateFrom < c?.DateFrom, conflictVers, $"Дата події раніша за дату народження особи по версії народження ");
                    // подія сталася після смерті
                    verifyVersionDate2(ver, dethVersions, (v, c) => v?.DateTo > c?.DateTo, conflictVers, $"Дата події пізніша за дату смерті особи по версії смерті ");
                    // конфлікти версій про народження особи з версіями фактів про близьких родичів
                    if (fact.FactType == "Birth")
                    {
                        foreach (var relative in relsToReturn)
                        {
                            var relativeBirthVersions = getVersions(relative.Id, "Birth");
                            var relativeDethVersions = getVersions(relative.Id, "Deth");
                            if (relative.RelationType == "Partner" || relative.RelationType == "Ex-partner")
                            {
                                // особа народилася після смерті партнера
                                verifyVersionDate2(ver, relativeDethVersions, (p, r) => p?.DateFrom >= r?.DateTo, conflictVers, $"Дата народження особи пізніша за дату смерті її партнера ", relative);
                            }
                            else if (relative.RelationType == "Child")
                            {
                                // особа народилася після народження її ж дитини
                                verifyVersionDate2(ver, relativeBirthVersions, (p, r) => p?.DateFrom >= r?.DateFrom, conflictVers, $"Дата народження особи пізніша за дату народження її дитини ", relative);
                                // особа народилася після смерті її ж дитини
                                verifyVersionDate2(ver, relativeDethVersions, (p, r) => p?.DateFrom >= r?.DateTo, conflictVers, $"Дата народження особи пізніша за дату смерті її дитини ", relative);
                            }
                            else if (relative.RelationType == "Father" || relative.RelationType == "Mother")
                            {
                                // особа народилася до народження її ж батьків
                                verifyVersionDate2(ver, relativeBirthVersions, (p, r) => p?.DateFrom <= r?.DateFrom, conflictVers, $"Дата народження особи раніша за дату народження її батька/матері ", relative);
                                // особа народилася після смерті її ж батьків
                                verifyVersionDate2(ver, relativeDethVersions, (p, r) => p?.DateFrom > r?.DateTo, conflictVers, $"Дата народження особи пізніша за дату смерті її батька/матері ", relative);
                            }
                        }
                    }
                    // конфлікти версій про смерь особи з версіями фактів про близьких родичів
                    else if (fact.FactType == "Deth")
                    {
                        foreach (var relative in relsToReturn)
                        {
                            var relativeBirthVersions = getVersions(relative.Id, "Birth");
                            var relativeDethVersions = getVersions(relative.Id, "Deth");
                            if (relative.RelationType == "Partner" || relative.RelationType == "Ex-partner")
                            {
                                // особа померла до народження партнера
                                verifyVersionDate2(ver, relativeBirthVersions, (p, r) => p?.DateTo <= r?.DateFrom, conflictVers, $"Дата смерті особи раніша за дату народження її партнера ", relative);
                            }
                            else if (relative.RelationType == "Child")
                            {
                                // особа померла до народження її ж дитини
                                verifyVersionDate2(ver, relativeBirthVersions, (p, r) => p?.DateTo < r?.DateFrom, conflictVers, $"Дата смерті особи раніша за дату народження її дитини ", relative);
                            }
                            else if (relative.RelationType == "Father" || relative.RelationType == "Mother")
                            {
                                // особа померла до народження її ж батьків
                                verifyVersionDate2(ver, relativeBirthVersions, (p, r) => p?.DateTo <= r?.DateFrom, conflictVers, $"Дата смерті особи раніша за дату народження її батька/матері ", relative);
                            }
                        }
                    }

                    versToReturn.Add(new PersonFactVersionDTO
                    {
                        Id = ver.Id,
                        Role = ver.Role,
                        Place = ver.Place,
                        Location = ver.Location,
                        DateFrom = ver.DateFrom,
                        DateTo = ver.DateTo,
                        Note = ver.Note,
                        Source = ver.Source,
                        Veracity = ver.Veracity,
                        ConflictVersions = conflictVers
                    });
                }

                factsToReturn.Add(new PersonFactDTO
                {
                    Id = fact.Id,
                    FactType = fact.FactType,
                    Versions = versToReturn
                });
            }

            return new PersonProfileDTO
            {
                Id = person.Id,
                MyRole = fmRepository.GetByField(m => m.UserLogin == userLogin && m.FamilyId == family.Id).FirstOrDefault()?.Role ?? "Guest",
                Suffix = person?.Suffix,
                Prefix = person?.Prefix,
                FullName = GetFullName(names, person),
                Names = namesToReturn,
                FamilyId = person?.FamilyId,
                FamilyName = family?.Name,
                Gender = person?.Gender,
                Biography = person?.Biography,
                Nationality = person?.Nationality,
                Religion = person?.Religion,
                Images = imagesToReturn,
                Facts = factsToReturn,
                Relatives = relsToReturn
            };
        }

        public IEnumerable<PersonMainInfoDTO> getAll(PersonOptionsDTO? personOptions)
        {
            // set serching creterias
            Expression<Func<Person, bool>> predicate = p => p != null;
            // serch by familyId
            if (personOptions?.FamilyId != null)
            {
                Expression<Func<Person, bool>> familyPredicate = p => p.FamilyId == personOptions.FamilyId;
                predicate = predicate.MergeAnd(familyPredicate);
            }
            // get not private families
            else
            {
                Expression<Func<Person, bool>> familyPredicate = p => p.Family.PrivacyLevel != 1;
                predicate = predicate.MergeAnd(familyPredicate);
            }
            // serch prefix
            if (!string.IsNullOrEmpty(personOptions?.Prefix))
            {
                Expression<Func<Person, bool>> prefixPredicate = p => (p.Prefix ?? "")
                    .Trim()
                    .IndexOf(personOptions.Prefix.Trim()) == 0;
                predicate = predicate.MergeAnd(prefixPredicate);
            }
            // serch firstname
            if (!string.IsNullOrEmpty(personOptions?.FirstName))
            {
                Expression<Func<Person, bool>> firstNamePredicate = p => p.PersonNames
                    .Any(n => n.NameType == "FirstName"
                        && (n.Name ?? "")
                        .Trim()
                        .IndexOf(personOptions.FirstName.Trim()) == 0);
                predicate = predicate.MergeAnd(firstNamePredicate);
            }
            // serch midlename
            if (!string.IsNullOrEmpty(personOptions?.MidleName))
            {
                Expression<Func<Person, bool>> midleNamePredicate = p => p.PersonNames
                    .Any(n => n.NameType == "MidleName"
                        && (n.Name ?? "")
                        .Trim()
                        .IndexOf(personOptions.MidleName.Trim()) == 0);
                predicate = predicate.MergeAnd(midleNamePredicate);
            }
            // serch lastname
            if (!string.IsNullOrEmpty(personOptions?.LastName))
            {
                Expression<Func<Person, bool>> lastNamePredicate = p => p.PersonNames
                    .Any(n => n.NameType == "LastName"
                        && (n.Name ?? "")
                        .Trim()
                        .IndexOf(personOptions.LastName.Trim()) == 0);
                predicate = predicate.MergeAnd(lastNamePredicate);
            }
            // serch suffix
            if (!string.IsNullOrEmpty(personOptions?.Suffix))
            {
                Expression<Func<Person, bool>> suffixPredicate = p => (p.Suffix ?? "")
                    .Trim()
                    .IndexOf(personOptions.Suffix.Trim()) == 0;
                predicate = predicate.MergeAnd(suffixPredicate);
            }
            // serch gender
            if (!string.IsNullOrEmpty(personOptions?.Gender))
            {
                Expression<Func<Person, bool>> genderPredicate = p => (p.Gender ?? "")
                    .Trim()
                    .IndexOf(personOptions.Gender.Trim()) == 0;
                predicate = predicate.MergeAnd(genderPredicate);
            }
            // serch facts
            if (personOptions?.Facts != null)
            {
                foreach (var fact in personOptions.Facts)
                {
                    // empty fact options not in serch
                    if (string.IsNullOrEmpty(fact?.Location) && fact?.DateFrom == null && fact?.DateTo == null || string.IsNullOrEmpty(fact?.FactType))
                    {
                        continue;
                    }
                    // serch by fact version
                    Expression<Func<Domain.Entities.Version, bool>> versionPredicate = v => true;
                    if (!string.IsNullOrEmpty(fact?.Location))
                    {
                        Expression<Func<Domain.Entities.Version, bool>> locationPredicate = v => (v.Location ?? "")
                            .Trim()
                            .IndexOf(fact.Location.Trim()) == 0;
                        versionPredicate = versionPredicate.MergeAnd(locationPredicate);
                    }
                    // serch date from
                    if (fact?.DateFrom != null)
                    {
                        Expression<Func<Domain.Entities.Version, bool>> dateFronPredicate = v => v.DateFrom >= fact.DateFrom || v.DateTo >= fact.DateFrom;
                        versionPredicate = versionPredicate.MergeAnd(dateFronPredicate);
                    }
                    // serch date to
                    if (fact?.DateTo != null)
                    {
                        Expression<Func<Domain.Entities.Version, bool>> dateToPredicate = v => v.DateTo <= fact.DateTo || v.DateFrom <= fact.DateTo;
                        versionPredicate = versionPredicate.MergeAnd(dateToPredicate);
                    }
                    predicate = predicate.MergeAnd(
                        p => p.BiographicalFacts.Any(
                            f => f.FactType == fact.FactType && f.Versions.AsQueryable().Any(versionPredicate)));
                }
            }

            // get persons by serching options
            // начало метода в записку
            List<PersonMainInfoDTO> personsToShow = new List<PersonMainInfoDTO>();
            var persons = personRepository.GetByField(predicate); // p => p.FamilyId == familyId
            foreach (var p in persons)
            {
                // отримання інформації про сім'ю
                var family = familyRepository.GetById((Guid)p.FamilyId);

                // отримання імені
                var names = nameRepository.GetByField(n => n.PersonId == p.Id);

                // отримання списку родичів, з якими встановлено прямий зв'язок
                var relsToReturn = GetRelatives(p.Id);

                // отримання інформації про народження
                var birthFacts = factRepository.GetByField(f => f.PersonId == p.Id && f.FactType == "Birth").FirstOrDefault();
                var birth = GetShortFact(birthFacts);
                // отримання інформації про смерть
                var dethFacts = factRepository.GetByField(f => f.PersonId == p.Id && f.FactType == "Deth").FirstOrDefault();
                var deth = GetShortFact(dethFacts);

                // отримання зображення профілю особи
                var image = imageRepository.GetByField(i => i.PersonId == p.Id)?.ToList()?.FirstOrDefault();

                personsToShow.Add(new PersonMainInfoDTO
                {
                    Id = p.Id,
                    FamilyId = family.Id,
                    FamilyName = family.Name,
                    FullName = GetFullName(names, p),
                    Relatives = relsToReturn,
                    Birth = birth,
                    Deth = deth,
                    Image = image?.Image
                });
            }
            return personsToShow;
        }


        // для показа
        //public IEnumerable<ConflictVersionDTO> verifyFactDates(Guid personId)
        //{
        //    var conflictVersions = new List<ConflictVersionDTO>();
        //    // валідація id особи
        //    var person = getPersonIfExist(personId);
        //    string RelationPartner = "Partner";
        //    string RelationExPartner = "Ex-partner";
        //    string RelationChild = "Child";

        //    // отримання версій фактів особи для перевірки
        //    var versions = versionService.getVersions(personId);
        //    var birthVersions = versionService.getBirthVersions(personId);
        //    var deathVersions = versionService.getDeathVersions(personId);

        //    // для кожної версії перевірка на відповідність еталонам
        //    foreach (var version in versions)
        //    {
        //        // чи подія сталася раніше за народження цієї ж особи
        //        verifyVersionDate(version, birthVersions, 
        //            (v, c) => v?.DateFrom < c?.DateFrom, conflictVersions);
        //        // чи подія сталася після смерті цієї ж особи
        //        verifyVersionDate(version, deathVersions, 
        //            (v, c) => v?.DateTo > c?.DateTo, conflictVersions);
        //    }
        //    // перевірка чи є конфлікти між версіями фактів про себе та про близьких родичів
        //    var relatives = relativeService.GetRelatives(personId);
        //    foreach (var relative in relatives)
        //    {
        //        var relativeBirthVersions = versionService.getBirthVersions(relative.Id);
        //        var relativeDeathVersions = versionService.getDeathVersions(relative.Id);
        //        // версій про народження особи з версіями фактів про близьких родичів
        //        foreach (var selfBirthVersion in birthVersions)
        //        {
        //            if (relative.RelationType == RelationPartner 
        //                || relative.RelationType == RelationExPartner)
        //            {
        //                // перевірка чи особа народилася після смерті партнера
        //                verifyVersionDate(selfBirthVersion, relativeDeathVersions, 
        //                    (p, r) => p?.DateFrom >= r?.DateTo, conflictVersions, relative);
        //            }
        //            else if (relative.RelationType == RelationChild)
        //            {
        //                // особа народилася після народження її ж дитини
        //                verifyVersionDate(selfBirthVersion, relativeBirthVersions, 
        //                    (p, r) => p?.DateFrom >= r?.DateFrom, conflictVersions, relative);
        //                // особа народилася після смерті її ж дитини
        //                verifyVersionDate(selfBirthVersion, relativeDeathVersions, 
        //                    (p, r) => p?.DateFrom >= r?.DateTo, conflictVersions, relative);
        //            }
        //            else if (relative.RelationType == "Father" || relative.RelationType == "Mother")
        //            {
        //                // особа народилася до народження її ж батьків
        //                verifyVersionDate(selfBirthVersion, relativeBirthVersions, 
        //                    (p, r) => p?.DateFrom <= r?.DateFrom, conflictVersions, relative);
        //                // особа народилася після смерті її ж батьків
        //                verifyVersionDate(selfBirthVersion, relativeDeathVersions, 
        //                    (p, r) => p?.DateFrom > r?.DateTo, conflictVersions, relative);
        //            }
        //        }
        //        // конфлікти версій про смерь особи з версіями фактів про близьких родичів
        //        foreach (var selfDethVersion in deathVersions)
        //        {
        //            if (relative.RelationType == "Partner" || relative.RelationType == "Ex-partner")
        //            {
        //                // особа померла до народження партнера
        //                verifyVersionDate(selfDethVersion, relativeBirthVersions, 
        //                    (p, r) => p?.DateTo <= r?.DateFrom, conflictVersions, relative);
        //            }
        //            else if (relative.RelationType == "Child")
        //            {
        //                // особа померла до народження її ж дитини
        //                verifyVersionDate(selfDethVersion, relativeBirthVersions, 
        //                    (p, r) => p?.DateTo < r?.DateFrom, conflictVersions, relative);
        //            }
        //            else if (relative.RelationType == "Father" || relative.RelationType == "Mother")
        //            {
        //                // особа померла до народження її ж батьків
        //                verifyVersionDate(selfDethVersion, relativeBirthVersions, 
        //                    (p, r) => p?.DateTo <= r?.DateFrom, conflictVersions, relative);
        //            }
        //        }
        //    }
        //    return conflictVersions;
        //}

        public HierarchicTreeNodeDTO getHierarchicTreeNode(Guid id, sbyte direction)
        {
            var person = getPersonIfExist(id);
            var relatives = GetRelatives(id);

            var relativesToReturn = new List<HierarchicTreeNodeDTO>();
            if (direction > 0)
            {
                var father = relatives.Where(r => r.RelationType == "Father").FirstOrDefault();
                if (father != null)
                {
                    relativesToReturn.Add(getHierarchicTreeNode(father.Id, direction));
                }
                var mother = relatives.Where(r => r.RelationType == "Mother").FirstOrDefault();
                if (mother != null)
                {
                    relativesToReturn.Add(getHierarchicTreeNode(mother.Id, direction));
                }
            }
            else if (direction < 0)
            {
                var children = relatives.Where(r => r.RelationType == "Child");
                foreach (var child in children)
                {
                    relativesToReturn.Add(getHierarchicTreeNode(child.Id, direction));
                }
            }

            return new HierarchicTreeNodeDTO
            {
                Id = id,
                Name = GetPartName(nameRepository.GetByField(n => n.PersonId == id), person),
                X = 0,
                y = 0,
                SubNodes = relativesToReturn
            };

        }

        // перевірка чи входить версія для перевірки в обмеження версій-еталонів
        //private void verifyVersionDate(Domain.Entities.Version versionToVerify, 
        //    IEnumerable<Domain.Entities.Version> constraints, 
        //    Func<Domain.Entities.Version, Domain.Entities.Version, bool> condition, 
        //    List<ConflictVersionDTO> conflictVersions,
        //    PersonRelativeDTO conflictRelative = null)
        //{
        //    // чи входить версія для перевірки в часові рамки версій-обмежень
        //    foreach (var constraint in constraints)
        //    {
        //        // не порівнюємо дати двох версій, за якими встановлено обмеження
        //        if (constraints.Any(v => v.Id == versionToVerify.Id))
        //            continue;
        //        /* якщо перевірюєма версія не входить у часові рамки,
        //        що встановлює версія-обмеження за певною умовою */
        //        if (condition(versionToVerify, constraint))
        //        {
        //            conflictVersions.Add(new ConflictVersionDTO
        //            {
        //                VersionId = versionToVerify.Id,
        //                ConflictVersionId = constraint.Id,
        //                ConflictRelative = conflictRelative
        //            });
        //        }
        //    }
        //}




        private void verifyVersionDate2(Domain.Entities.Version versionToVerify, IEnumerable<Domain.Entities.Version> constraints,
            Func<Domain.Entities.Version, Domain.Entities.Version, bool> condition, List<ConflictVerDTO> conflictVersions, string ex,
            PersonRelativeDTO conflictRelative = null)
        {
            // перевіряємо чи входить версія для перевірки в часові рамки версій-обмежень
            foreach (var constraint in constraints)
            {
                // не порівнюємо дати двох версій, за якими встановлено обмеження
                if (constraints.Any(v => v.Id == versionToVerify.Id))
                    continue;
                // якщо перевірюєма версія не входить у часові рамки, що встановлює версія-обмеження за певною умовою
                if (condition(versionToVerify, constraint))
                {
                    conflictVersions.Add(new ConflictVerDTO
                    {
                        ConflictVersionId = constraint.Id,
                        Message = ex,
                        ConflictRelative = conflictRelative
                    });
                }
            }
        }

        public IEnumerable<Domain.Entities.Version> getVersions(Guid personId, string relationType)
        {
            return versionRepository.GetByField(
                v => v.BiographicalFact.PersonId == personId
                && v.BiographicalFact.FactType == relationType);
        }
        public IEnumerable<Domain.Entities.Version> getBirthVersions(Guid personId, string relationType = "Birth")
        {
            return versionRepository.GetByField(
                v => v.BiographicalFact.PersonId == personId
                && v.BiographicalFact.FactType == relationType);
        }
        public IEnumerable<Domain.Entities.Version> getDeathVersions(Guid personId, string relationType = "Deth")
        {
            return versionRepository.GetByField(
                v => v.BiographicalFact.PersonId == personId
                && v.BiographicalFact.FactType == relationType);
        }

        // перевірка наявності особи з personId
        private Person getPersonIfExist(Guid id)
        {
            var person = personRepository.GetById(id);
            if (person == null)
            {
                throw new Exception($"Person with id {id} doesn't exist");
            }
            else
            {
                return person;
            }
        }


        // other
        private FactShortInfoDTO GetShortFact(BiographicalFact? fact)
        {
            //find fact versions
            if (fact == null)
            {
                return new FactShortInfoDTO
                {
                    DateFrom = null,
                    DateTo = null,
                    Place = null
                };
            }
            var vers = versionRepository.GetByField(v => v.BiographicalFactId == fact.Id);
            DateOnly? DateFrom = vers.Min(v => v.DateFrom);
            DateOnly? DateTo = vers.Max(v => v.DateTo);
            string? place = vers.ToList()?.FirstOrDefault()?.Location;

            return new FactShortInfoDTO
            {
                DateFrom = DateFrom,
                DateTo = DateTo,
                Place = place
            };
        }
        private List<PersonRelativeDTO> GetRelatives(Guid id)
        {
            var relsIs = relationRepository.GetByField(r => r.PersonIsId == id);
            var relsTo = relationRepository.GetByField(r => r.ToPersonId == id);
            var relsToReturn = new List<PersonRelativeDTO>();
            // relatives first
            foreach (var rel in relsIs)
            {
                // find and build relative full name
                var relp = personRepository.GetById((Guid)rel.ToPersonId);
                var relpNames = nameRepository.GetByField(n => n.PersonId == relp.Id);
                // define relation type
                string? relType = rel.RelationType;
                if (relType != null && (relType == "Father" || relType == "Mother"))
                {
                    relType = "Child";
                }
                // add data
                relsToReturn.Add(new PersonRelativeDTO
                {
                    Id = (Guid)rel.ToPersonId,
                    FullName = GetFullName(relpNames, relp),
                    RelationType = relType
                });
            }
            // relatives secound
            foreach (var rel in relsTo)
            {
                // find and build relative full name
                var relp = personRepository.GetById((Guid)rel.PersonIsId);
                var relpNames = nameRepository.GetByField(n => n.PersonId == relp.Id);
                // add data
                relsToReturn.Add(new PersonRelativeDTO
                {
                    Id = (Guid)rel.PersonIsId,
                    FullName = GetFullName(relpNames, relp),
                    RelationType = rel.RelationType
                });
            }
            return relsToReturn;
        }

        private string GetFullName(IEnumerable<PersonName> relpNames, Person relp)
        {
            StringBuilder fullName = new StringBuilder();
            var relpMainFirstNames = relpNames.Where(n => n.NameType == "FirstName" && n.IsMain == true);
            var relpOtherFirstNames = relpNames.Where(n => n.NameType == "FirstName" && n.IsMain == false);
            var relpMainMidleNames = relpNames.Where(n => n.NameType == "MidleName" && n.IsMain == true);
            var relpOtherMidleNames = relpNames.Where(n => n.NameType == "MidleName" && n.IsMain == false);
            var relpMainLastNames = relpNames.Where(n => n.NameType == "LastName" && n.IsMain == true);
            var relpOtherLastNames = relpNames.Where(n => n.NameType == "LastName" && n.IsMain == false);
            //prefix
            fullName.Append(relp?.Prefix + " ");
            //firstname
            foreach (var n in relpMainFirstNames)
            {
                fullName.Append(n.Name + " ");
            }
            if (relpOtherFirstNames.Count() > 0)
            {
                fullName.Append("(");
                foreach (var n in relpOtherFirstNames)
                {
                    fullName.Append(n.Name + ", ");
                }
                fullName.Remove(fullName.Length - 2, 2);
                fullName.Append(") ");
            }
            //midlename
            foreach (var n in relpMainMidleNames)
            {
                fullName.Append(n.Name + " ");
            }
            if (relpOtherMidleNames.Count() > 0)
            {
                fullName.Append("(");
                foreach (var n in relpOtherMidleNames)
                {
                    fullName.Append(n.Name + ", ");
                }
                fullName.Remove(fullName.Length - 2, 2);
                fullName.Append(") ");
            }
            //lastname
            foreach (var n in relpMainLastNames)
            {
                fullName.Append(n.Name + " ");
            }
            if (relpOtherLastNames.Count() > 0)
            {
                fullName.Append("(");
                foreach (var n in relpOtherLastNames)
                {
                    fullName.Append(n.Name + ", ");
                }
                fullName.Remove(fullName.Length - 2, 2);
                fullName.Append(") ");
            }
            //suffix
            fullName.Append(relp?.Suffix);

            return fullName.ToString();
        }


        private string GetPartName(IEnumerable<PersonName> relpNames, Person relp)
        {
            var firstName = relpNames.Where(n => n.NameType == "FirstName" && n.IsMain == true).FirstOrDefault();
            var lastName = relpNames.Where(n => n.NameType == "LastName" && n.IsMain == true).FirstOrDefault();

            return $"{firstName?.Name ?? "Невідомо"}\n{lastName?.Name ?? "Невідомо"}";
        }
    }
}
