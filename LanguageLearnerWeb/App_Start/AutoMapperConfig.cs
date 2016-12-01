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

                cfg.CreateMap<ProfileMaterial, ProfileMaterialDTO>()
                    .ForMember(dest => dest.LanguageId, opts => opts.MapFrom(opt => opt.Material.LanguageId))
                    .ForMember(dest => dest.Difficulty, opts => opts.MapFrom(opt => opt.Material.Difficulty))
                    .ForMember(dest => dest.Image, opts => opts.MapFrom(opt => opt.Material.Image))
                    .ForMember(dest => dest.Headline, opts => opts.MapFrom(opt => opt.Material.Headline))
                    .ForMember(dest => dest.Text, opts => opts.MapFrom(opt => opt.Material.Text))
                    .ForMember(dest => dest.ShortDescr, opts => opts.MapFrom(opt => opt.Material.ShortDescr))
                    .ForMember(dest => dest.Rating, opts => opts.MapFrom(
                        opt => opt.Rating == 0 ? opt.Material.Rating : opt.Rating));
                cfg.CreateMap<ProfileMaterialDTO, ProfileMaterial>();

                cfg.CreateMap<ProfileWord, ProfileWordDTO>()
                    .ForMember(dest => dest.LanguageFromId, opts => opts.MapFrom(opt => opt.Word.LanguageFromId))
                    .ForMember(dest => dest.LanguageToId, opts => opts.MapFrom(opt => opt.Word.LanguageToId))
                    .ForMember(dest => dest.Name, opts => opts.MapFrom(opt => opt.Word.Name))
                    .ForMember(dest => dest.Translation, opts => opts.MapFrom(opt => opt.Word.Translation))
                    .ForMember(dest => dest.Transcription, opts => opts.MapFrom(opt => opt.Word.Transcription))
                    .ForMember(dest => dest.Image, opts => opts.MapFrom(opt => opt.Word.Image));
                cfg.CreateMap<ProfileWordDTO, ProfileWord>();
            });
        }
    }
}