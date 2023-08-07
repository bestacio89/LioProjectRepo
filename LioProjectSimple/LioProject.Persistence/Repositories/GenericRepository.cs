using LioProject.Domain.Entities.Helpers;
using LioProject.Domain.Users;
using LioProject.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LioProject.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, ISoftDeletable
    {
        #region Fields and Constructor

        protected LioProjectDbContext _context;

        /// <summary>
        /// Initialise une nouvelle instance de la classe GenericRepository.
        /// </summary>
        /// <param name="context">Le contexte de base de données utilisé par le référentiel.</param>
        public GenericRepository(LioProjectDbContext context) => _context = context;

        #endregion Fields and Constructor

        #region Methods

        /// <summary>
        /// Récupère l'entité avec l'ID spécifié.
        /// </summary>
        /// <param name="id">L'ID de l'entité à récupérer.</param>
        /// <returns>L'entité récupérée.</returns>
        public async Task<T> Get(int id) => await _context.Set<T>().FindAsync(id);

        /// <summary>
        /// Récupère l'entité avec l'ID spécifié.
        /// </summary>
        /// <param name="id">L'ID de l'entité à récupérer.</param>
        /// <returns>L'entité récupérée.</returns>
        /// Recupere l'entité avec details
        public async Task<T> GetWithDetails(int id) => await _context.Set<T>().Include(a => a.CreeParId).Include(a => a.ModifieEnDernierParId).FirstOrDefaultAsync(a => a.Id == id);

        /// <summary>
        /// Récupère toutes les entités de ce type.
        /// </summary>
        /// <returns>Une liste en lecture seule des entités récupérées.</returns>
        public async Task<IQueryable<T>> GetAll()
        {
            var entities = await _context.Set<T>().ToListAsync();
            return entities.AsQueryable();
        }

        /// <summary>
        /// Récupère toutes les entités actives de ce type.
        /// </summary>
        /// <returns>Une liste en lecture seule des entités récupérées.</returns>
        public async Task<IQueryable<T>> GetAllActif()
        {
            var entities = await _context.Set<T>().Where(a => a.EstActif == true).ToListAsync();
            return entities.AsQueryable();
        }

        public async Task<IQueryable<T>> FindByCondition(Expression<Func<T, bool>> expression)
        {
            var entities = await _context.Set<T>().Where(expression).ToListAsync();
            return entities.AsQueryable();
        }

        /// <summary>
        /// Supprime logiquement l'entité spécifiée du contexte de base de données.
        /// </summary>
        /// <param name="entity">L'entité à supprimer.</param>
        public async Task<T> SoftDelete(T entity, ApplicationUser user)
        {
            if ( entity is ApplicationUser)
            {
                entity.EstActif = false;
                entity.ModifieEnDernierParId = user.Id;
                entity.ModifieEnDernierLe = DateTime.Now;
                _context.Entry(entity).State = EntityState.Modified;
                _context.Set<T>().Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            else
            {
                await Delete(entity, user);
                return null;
            }
        }

        /// <summary>
        /// Supprime l'entité spécifiée du contexte de base de données.
        /// </summary>
        /// <param name="entity">L'entité à supprimer.</param>
        public async Task Delete(T entity, ApplicationUser user)
        {
            if (entity is not ApplicationUser
               )
            {
                _context.Remove(entity);
                await _context.SaveChangesAsync();
            }
            else
            {
                await SoftDelete(entity, user);
            }
        }

        /// <summary>
        /// Supprime l'entité spécifiée du contexte de base de données.
        /// </summary>
        /// <param name="entity">L'entité à supprimer.</param>
        public async Task ForceDelete(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Supprime des entités logiquement par condition
        /// </summary>
        /// <param name="expression">La condition</param>
        /// <returns></returns>
        public async Task<int> SoftDeleteByCondition(Expression<Func<T, bool>> expression, ApplicationUser user)
        {
            var entities = await _context.Set<T>().Where(expression).ToListAsync();
            entities.ForEach(e => e.EstActif = false);
            entities.ForEach(e => e.ModifieEnDernierParId = user.Id);
            entities.ForEach(e => e.ModifieEnDernierLe = DateTime.Now);
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Supprime des entités par condition
        /// </summary>
        /// <param name="expression">La condition</param>
        /// <returns></returns>
        public async Task<int> DeleteByCondition(Expression<Func<T, bool>> expression)
        {
            var entities = await _context.Set<T>().Where(expression).ToListAsync();
            _context.RemoveRange(entities);
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Ajoute une nouvelle entité à ce référentiel et la sauvegarde dans le contexte de base de données.
        /// </summary>
        /// <param name="entity">L'entité à ajouter.</param>
        /// <returns>L'entité ajoutée.</returns>
        public async Task<T> Add(T entity, ApplicationUser user)
        {
            //if (entity is VersionsAffaire)
            //{
            //  await  SoftDelete( _context.Set<T>().Last(), user );
            //    entity.CreeParId = user.Id;
            //    entity.CreeLe = DateTime.Now;
            //    entity.ModifieEnDernierParId = user.Id;
            //    entity.ModifieEnDernierLe = DateTime.Now;
            //    entity.EstActif = true;
            //    await _context.Set<T>().AddAsync(entity);

            //    await _context.SaveChangesAsync();
            //    return entity;
            //}
            entity.CreeParId = user.Id;
            entity.CreeLe = DateTime.Now;
            entity.ModifieEnDernierParId = user.Id;
            entity.ModifieEnDernierLe = DateTime.Now;
            entity.EstActif = true;
            await _context.Set<T>().AddAsync(entity);

            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Ajoute une utilisateur depuis le SSO ou importAuto entité à ce référentiel et la sauvegarde dans le contexte de base de données.
        /// </summary>
        /// <param name="entity">L'entité à ajouter.</param>
        /// <returns>L'entité ajoutée.</returns>
        public async Task<T> AddBySystem(T entity)
        {
            entity.CreeParId = 1;
            entity.CreeLe = DateTime.Now;
            entity.ModifieEnDernierParId = 1;
            entity.ModifieEnDernierLe = DateTime.Now;
            entity.EstActif = true;
            await _context.Set<T>().AddAsync(entity);

            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Met à jour l'entité spécifiée dans le contexte de base de données.
        /// </summary>
        /// <param name="entity">L'entité à mettre à jour.</param>
        public async Task Update(T entity, ApplicationUser user)
        {
            // For other types of entities, update the entity as usual
            entity.ModifieEnDernierParId = user.Id;
            entity.ModifieEnDernierLe = DateTime.Now;
            _context.Entry(entity).State = EntityState.Modified;
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Met à jour l'entité spécifiée dans le contexte de base de données.
        /// </summary>
        /// <param name="entity">L'entité à mettre à jour.</param>
        public async Task UpdateBySystem(T entity)
        {
            entity.ModifieEnDernierParId = 1;
            entity.ModifieEnDernierLe = DateTime.Now;

            _context.Entry(entity).State = EntityState.Modified;
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Vérifie si une entité satisfaisant le prédicat spécifié existe dans le référentiel.
        /// </summary>
        /// <param name="predicate">Le prédicat à vérifier.</param>
        /// <returns>Une valeur booléenne indiquant si une entité satisfaisant le prédicat existe.</returns>
        public async Task<bool> Exists(Expression<Func<T, bool>> predicate) => await _context.Set<T>().AnyAsync(predicate);

        #endregion Methods
    }
}
