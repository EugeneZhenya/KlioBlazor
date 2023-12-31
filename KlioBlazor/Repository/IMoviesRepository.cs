﻿using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;

namespace KlioBlazor.Repository
{
    public interface IMoviesRepository
    {
        Task<int> CreateMovie(Movie movie);
        Task<DetailsMovieDTO> GetDetailsMovieDTO(int id);
        Task<HomePageDTO> GetHomePageDTO();
    }
}
