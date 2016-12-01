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

                cfg.CreateMap<ProfileLanguage, ProfileLanguageDTO>()
                    .ForMember(dest => dest.Source, opts => opts.MapFrom(opt => opt.Language.Source))
                    .ForMember(dest => dest.LanguageId, opts => opts.MapFrom(opt => opt.Language.Id))
                    .ForMember(dest => dest.Name, opts => opts.MapFrom(opt => opt.Language.Name))
                    .ForMember(dest => dest.NameOriginal, opts => opts.MapFrom(opt => opt.Language.NameOriginal))
                    .ForMember(dest => dest.ShortName, opts => opts.MapFrom(opt => opt.Language.ShortName))
                    .ForMember(dest => dest.ShortNameCC, opts => opts.MapFrom(opt => opt.Language.ShortNameCC));

                cfg.CreateMap<ProfileLanguageDTO, ProfileLanguage>();

                cfg.CreateMap<ProfilePreposition, ProfilePrepositionDTO>()
                    .ForMember(dest => dest.LanguageId, opts => opts.MapFrom(opt => opt.Preposition.LanguageId))
                    .ForMember(dest => dest.Prefix, opts => opts.MapFrom(opt => opt.Preposition.Prefix))
                    .ForMember(dest => dest.Suffix, opts => opts.MapFrom(opt => opt.Preposition.Suffix))
                    .ForMember(dest => dest.Answer, opts => opts.MapFrom(opt => opt.Preposition.Answer))
                    .ForMember(dest => dest.Options, opts => opts.MapFrom(opt => opt.Preposition.Options))
                    .ForMember(dest => dest.Rule, opts => opts.MapFrom(opt => opt.Preposition.Rule));

                cfg.CreateMap<ProfilePrepositionDTO, ProfilePreposition>();
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