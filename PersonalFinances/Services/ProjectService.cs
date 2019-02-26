using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using PersonalFinances.Repositories;
using PersonalFinances.Models;
using PersonalFinances.Models.Enums;
using PersonalFinances.Services.Exceptions;

namespace PersonalFinances.Services
{
    public class ProjectService
    {
        private ProjectRepository _repository = new ProjectRepository();
        private MovementRepository _movementRepository = new MovementRepository();

        /// <summary>
        /// Get all projects
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Project>> GetAllProjects()
        {
            return await _repository.GetAllProjects();
        }

        /// <summary>
        /// Get all active projects
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Project>> GetAllActiveProjects()
        {
            return await _repository.GetAllActiveProjects();
        }

        /// <summary>
        /// Add a new project
        /// </summary>
        /// <param name="project"></param>
        public async Task Add (Project project)
        {
            project.Enabled = true;
            project.StartDate = DateTime.Today;
            project.ProjectStatus = ProjectStatus.InProgress;

            var nameExists = (await _repository.GetProjectByName(project.Name)) != null;

            if (!nameExists)
                await _repository.Insert(project);
            else
                throw new AlreadyExistsException($"Already exists a project with the name {project.Name}");
        }

        /// <summary>
        /// Update a project
        /// </summary>
        /// <param name="project"></param>
        public async Task Update (Project project)
        {
            project.Enabled = true;
            var quantity = (await _repository.GetAllProjects(project.Name)).Count(p => !p.Id.Equals(project.Id));

            if (quantity.Equals(0))
                await _repository.Update(project);
            else
                throw new AlreadyExistsException($"Already exists a project with the name {project.Name}");
        }

        /// <summary>
        /// Delete an existing project
        /// </summary>
        /// <param name="id"></param>
        public async Task Delete (int id)
        {
            var project = await GetProjectById(id);
            project.Enabled = false;

            await _repository.Update(project);
        }

        /// <summary>
        /// Get a project by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Project> GetProjectById (int id)
        {
            var project = await _repository.GetProjectById(id);

            if (project != null)
                return project;
            else
                throw new NotFoundException("This project not exists");
        }
    }
}