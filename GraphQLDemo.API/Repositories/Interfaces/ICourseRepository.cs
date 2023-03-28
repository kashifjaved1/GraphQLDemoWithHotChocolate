﻿using GraphQLDemo.API.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace GraphQLDemo.API.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        public Task<Course> CreateCourseAsync(Course course);
        public Task<Course> UpdateCourseAsync(Course course);
        public Task<bool> DeleteCourse(Guid id);
        public Task<Course> FindCourseById(Guid id);
        public Task<List<Course>> GetAllCoursesAsync();

    }
}
