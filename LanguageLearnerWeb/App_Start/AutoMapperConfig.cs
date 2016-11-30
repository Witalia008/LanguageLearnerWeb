using AutoMapper;
using LanguageLearnerWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageLearnerWeb
{
    public class AutoMapperConfig
    {
        public static void Config()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<LanguageLearnerWeb.Models.Profile, ProfileDTO>()
                    .ForMember(dest => dest.Email, opts => opts.MapFrom(opt => opt.ApplicationUser.Email));
                cfg.CreateMap<ProfileDTO, LanguageLearnerWeb.Models.Profile>();
                cfg.CreateMap<Settings, SettingsDTO>();
                cfg.CreateMap<SettingsDTO, Settings>();
            });
        }
    }
}