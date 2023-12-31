﻿using KlioBlazor.Shared.Entities;

namespace KlioBlazor.Repository
{
    public interface IPersonRepository
    {
        Task CreatePerson(Person person);
        Task<List<Person>> GetPeople();
        Task<List<Person>> GetPeopleByName(string name);
    }
}
