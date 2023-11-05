using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlScriptMerge.Lib;
internal static class MapperInstance
{
    public static Mapper Mapper {  get; } = MapperProfile.InitializeAutomapper();

}
