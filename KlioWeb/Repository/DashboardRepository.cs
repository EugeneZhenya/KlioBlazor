﻿using KlioBlazor.Shared.DTOs;
using KlioWeb.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace KlioWeb.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext context;

        public DashboardRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<DashboardDTO> GetDashboardDTO()
        {
            var response = new DashboardDTO();

            response.CategoriesCount = await context.Categories.CountAsync();
            response.PartitionsCount = await context.Partitions.CountAsync();
            response.KeywordsCount = await context.Keywords.CountAsync();
            response.GenresCount = await context.Genres.CountAsync();
            response.CountriesCount = await context.Countries.CountAsync();
            response.CreatorsCount = await context.Creators.CountAsync();
            response.PeopleCount = await context.People.CountAsync();
            response.MoviesCount = await context.Movies.CountAsync();

            return response;
        }
    }
}
