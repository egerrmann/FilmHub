using System.Collections.Generic;
using FilmHub.Models;

namespace FilmHub.Services.Registration
{
    public interface IRegistrationService
    {
        static bool isLogged;
        static int currentUserId;
    }
}