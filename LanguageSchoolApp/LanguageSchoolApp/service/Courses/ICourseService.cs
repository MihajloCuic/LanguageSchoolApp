﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.model;
using LanguageSchoolApp.model.Courses;

namespace LanguageSchoolApp.service.Courses
{
    public interface ICourseService
    {
        Dictionary<int, Course> GetAllCourses();
        List<Course> GetAllCoursesById(List<int> courseIds);
        Course GetCourse(int courseId);
        bool CourseExists(int courseId);
        List<Course> GetAllAvailableCourses();
        List<Course> GetAllFilteredCourses(List<Course> courses, string languageName, string languageLevelStr, string courseTypeStr);
        List<Course> SortCourses(List<Course> courses, string beginningDateSortingStr, string durationSortingStr);
        void CreateCourse(string languageName, string languageLevelStr, int maxParticipants, int duration, List<ClassPeriod> classPeriods, DateTime beginningDate, string courseTypeStr, string teacherId);
        void UpdateCourse(int courseId, string languageName, string languageLevelStr, int maxParticipants, int duration, List<ClassPeriod> classPeriods, DateTime beginningDate, string courseTypeStr);
        void DeleteCourse(int courseId);
        int GenerateId(LanguageProficiency languageProficiency, DateTime beginningDate, CourseType courseType, string teacherId);
        List<FinishedCourseDTO> GetFinishedCoursesDTO(List<FinishedCourse> finishedCourses);
        List<Course> GetTeachersPendingCourses(List<int> allTeacherCoursesIds);
        List<Course> GetTeacherActiveCourses(List<int> allTeacherCoursesIds);
    }
}
