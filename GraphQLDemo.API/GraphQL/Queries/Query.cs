﻿using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Models;
using GraphQLDemo.API.Models.Common;
using GraphQLDemo.API.Models.Domain;
using GraphQLDemo.API.Models.Entities;
using GraphQLDemo.API.Repositories;
using GraphQLDemo.API.GraphQL.Filters;
using GraphQLDemo.API.GraphQL.Sorters;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.API.GraphQL.Queries
{
    public class Query
    {
        private readonly CourseRepository _courseRepostory;

        public Query(CourseRepository courseRepostory)
        {
            _courseRepostory = courseRepostory;
        }

        //[UsePaging] // for enabling cursor based edges -> node pagination. Its a recommended one.
        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 10)]
        public async Task<List<Course>> GetCourses()
        {
            return await _courseRepostory.GetAllCoursesAsync();
        }

        [UseOffsetPaging(IncludeTotalCount = true, DefaultPageSize = 10)] // for enabling skip/take offset based pagination.
        public async Task<List<Course>> GetOffsetCourses()
        {
            return await _courseRepostory.GetAllCoursesAsync();
        }

        // [Note]: Order does matter of attributes below, so be careful.    
        [UseDbContext(typeof(SchoolDbContext))]
        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 10)] // in above pagination functions limit is not applied to query because of IEnumerable, so quering with IQueryable here and limit will apply here as it'll be affecting database because IQueryable is a optimized query.
        [UseProjection] 
        [UseFiltering(typeof(CourseFilterType))] // filter n removing/ignoring students property here using custom filter type.
        [UseSorting(typeof(CourseSortType))] // ignoring id based fields for sorting using custom sorter.
        public IQueryable<CourseDTO> GetPaginatedCourses([ScopedService] SchoolDbContext context) // method dependency injection using scopedService attribute by HotChocolate.
        {
            return context.Courses.Select(c => new CourseDTO
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId
            });
        }

        public async Task<Course> GetCourseByIdAsync(Guid id)
        {
            return await _courseRepostory.FindCourseById(id);
        }

        [GraphQLDeprecated("This query is deprecated.")]
        public string Instructions => "Smash that like button and subscribe to SingletonSean";
    }
}