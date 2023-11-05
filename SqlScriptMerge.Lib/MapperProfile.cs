using AutoMapper;
using SqlScriptMerge.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlScriptMerge.Lib;
internal class MapperProfile
{
        public static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TaggedQueryBasic, TaggedQuery>();
            });

            var mapper = new Mapper(config);
            return mapper;
        }    
}
