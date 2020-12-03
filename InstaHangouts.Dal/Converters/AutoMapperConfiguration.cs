using AutoMapper;
using InstaHangouts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaHangouts.Dal.Converters
{
    public static class AutoMapperConfiguration
    {
        /// <summary>
        /// Configures this instance.
        /// </summary>
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new InstaHangoutsProfile());
            });
        }

        /// <summary>
        /// Maps the object.
        /// </summary>
        /// <typeparam name="TSource">The type of the t source.</typeparam>
        /// <typeparam name="TDestination">The type of the t destination.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <returns>T Destination.</returns>
        private static TDestination MapObject<TSource, TDestination>(TSource source, TDestination destination)
        {
            var mapconfiguration = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());
            var mapper = mapconfiguration.CreateMapper();
            var result = mapper.Map(source, destination);
            return result;
        }

        /// <summary>
        /// Converts the user.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>User Model.</returns>
        public static UserModel ConvertUser(this UserInfo entity)
        {
            return MapObject(entity, new UserModel());
        }

        public static PlanEventModel Convert(this PlanEvent entity)
        {
            return MapObject(entity, new PlanEventModel());
        }

        public static GroupDetailListModel Convert(this GroupDetail entity)
        {
            return MapObject(entity, new GroupDetailListModel());
        }

        public static ICollection<PlanEventModel> Convert(this ICollection<PlanEvent> entities)
        {
            return MapListObjects(entities, new List<PlanEventModel>());
        }

        public static ICollection<UserInfo> Convert(this ICollection<UserModel> entities)
        {
            return MapListObjects(entities, new List<UserInfo>());
        }

        public static ICollection<GroupDetailListModel> Convert(this ICollection<GroupDetail> entities)
        {
            return MapListObjects(entities, new List<GroupDetailListModel>());
        }

        /// <summary>
        /// Maps the list objects.
        /// </summary>
        /// <typeparam name="TSource">The type of the t source.</typeparam>
        /// <typeparam name="TDestination">The type of the t destination.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <returns>ICollection&lt;TDestination&gt;.</returns>
        private static ICollection<TDestination> MapListObjects<TSource, TDestination>(IEnumerable<TSource> source,
                                                           ICollection<TDestination> destination)
        {
            var mapconfiguration = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());
            var mapper = mapconfiguration.CreateMapper();
            var result = mapper.Map(source, destination);
            return result;
        }


        /// <summary>
        /// Class InstaHangoutsProfile.
        /// </summary>
        /// <seealso cref="AutoMapper.Profile" />
        public class InstaHangoutsProfile : Profile
        {
            public InstaHangoutsProfile()
            {
                CreateMap<UserInfo, UserModel>();
                CreateMap<PlanEvent, PlanEventModel>();
                CreateMap<GroupDetail, GroupDetailListModel>();

            }
        }
    }
}
