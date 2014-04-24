using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TFax.Web.CORE.COMMON.ViewModels;
using TFax.Web.CORE.DAL;

namespace TFax.Web
{
    public class AutoMapperConfig
    {

        public static void Initialize()
        {

            //poco 
            Mapper.CreateMap<MemberMaster, MemberViewModel>()
                  .ReverseMap();
            Mapper.CreateMap<Review, ReviewViewModel>()
                .ReverseMap();

            //value
            Mapper.AllowNullDestinationValues = true;

            //conf
            Mapper.AssertConfigurationIsValid();
        }

    }
}