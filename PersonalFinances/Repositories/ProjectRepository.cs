using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

using PersonalFinances.Models;
using PersonalFinances.Models.Enums;

namespace PersonalFinances.Repositories
{
    public class ProjectRepository
    {
        /// <summary>
        /// Get all projects
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Project>> GetAllProjects ()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.Projects
                    .Include(p => p.Movements)
                    .Include("Movements.Category")
                .Where(p => p.Enabled).ToListAsync();
            }
        }

        /// <summary>
        /// Get all projects by name
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Project>> GetAllProjects (string name)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.Projects
                    .Include(p => p.Movements)
                    .Include("Movements.Category")
                .Where(p => p.Name.Equals(name) &&  p.Enabled).ToListAsync();
                
            }
        }

        /// <summary>
        /// Get all active projects (in progress projects)
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Project>> GetAllActiveProjects ()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.Projects
                    .Include(p => p.Movements)
                    .Include("Movements.Category")
                .Where(p => p.ProjectStatus == ProjectStatus.InProgress && p.Enabled).ToListAsync();
            }
        }

        /// <summary>
        /// Get a a project by Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Project> GetProjectByName (string name)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.Projects
                    .Include(p => p.Movements)
                .SingleOrDefaultAsync(p => p.Name.Equals(name) && p.Enabled);
            }
        }

        /// <summary>
        /// Get a project by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Project> GetProjectById (int id)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.Projects
                    .Include(p => p.Movements)
                    .Include("Movements.Category")
                .SingleOrDefaultAsync(p => p.Id.Equals(id) && p.Enabled);
            }
        }

        /// <summary>
        /// Insert a new Project 
        /// </summary>
        /// <param name="project"></param>
        public async Task Insert (Project project)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Projects.Add(project);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Update an existing Project
        /// </summary>
        /// <param name="project"></param>
        public async Task Update (Project project)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Entry(project).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
    }
}