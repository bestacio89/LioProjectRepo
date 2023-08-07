using LioProject.Domain.Entities.Helpers;
using LioProject.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LioProject.Persistence.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class, ISoftDeletable
    {
        /// <summary>
        /// Récupère l'entité avec l'ID spécifié.
        /// </summary>
        /// <param name="id">L'ID de l'entité à récupérer.</param>
        /// <returns>L'entité récupérée.</returns>
        public Task<TEntity> Get(int id);

        /// <summary>
        /// Récupère l'entité avec l'ID spécifié.
        /// </summary>
        /// <param name="id">L'ID de l'entité à récupérer.</param>
        /// <returns>L'entité récupérée.</returns>
        /// recupere l'entité avec full Details
        public Task<TEntity> GetWithDetails(int id);
        /// <summary>
        /// Récupère toutes les entités de ce type.
        /// </summary>
        /// <returns>Une liste en lecture seule des entités récupérées.</returns>
        public Task<IQueryable<TEntity>> GetAll();

        /// <summary>
        /// Récupère toutes les entités actives de ce type.
        /// </summary>
        /// <returns>Une liste en lecture seule des entités récupérées.</returns>
        public Task<IQueryable<TEntity>> GetAllActif();
        /// <summary>
        /// Finds the by condition.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public Task<IQueryable<TEntity>> FindByCondition(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Supprime logiquement l'entité spécifiée du référentiel.
        /// </summary>
        /// <param name="entity">L'entité à supprimer.</param>
        public Task<TEntity> SoftDelete(TEntity entity, ApplicationUser user);

        /// <summary>
        /// Supprime l'entité spécifiée du référentiel.
        /// </summary>
        /// <param name="entity">L'entité à supprimer.</param>
        public Task Delete(TEntity entity, ApplicationUser user);
        /// <summary>
        /// Force la Suppression l'entité spécifiée du référentiel.
        /// </summary>
        /// <param name="entity">L'entité à supprimer.</param>
        public Task ForceDelete(TEntity entity);

        /// <summary>
        /// Supprimes logiquement les entités par condition.
        /// </summary>
        /// <param name="expression">La condition</param>
        /// <returns></returns>
        public Task<int> SoftDeleteByCondition(Expression<Func<TEntity, bool>> expression, ApplicationUser user);

        /// <summary>
        ///Supprimes les entités par condition.
        /// </summary>
        /// <param name="expression">La condition.</param>
        /// <returns></returns>
        public Task<int> DeleteByCondition(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Ajoute une nouvelle entité à ce référentiel.
        /// </summary>
        /// <param name="entity">L'entité à ajouter.</param>
        /// <returns>L'entité ajoutée.</returns>
        public Task<TEntity> Add(TEntity entity, ApplicationUser user);

        /// <summary>
        /// Ajoute une nouvelle entité à ce référentiel.
        /// </summary>
        /// <param name="entity">L'entité à ajouter.</param>
        /// <returns>L'entité ajoutée.</returns>
        public Task<TEntity> AddBySystem(TEntity entity);

        /// <summary>
        /// Met à jour l'entité spécifiée.
        /// </summary>
        /// <param name="entity">L'entité à mettre à jour.</param>
        public Task Update(TEntity entity, ApplicationUser user);

        /// <summary>
        /// Met à jour l'entité spécifiée.
        /// </summary>
        /// <param name="entity">L'entité à mettre à jour.</param>
        public Task UpdateBySystem(TEntity entity);

        /// <summary>
        /// Vérifie si une entité satisfaisant le prédicat spécifié existe dans le référentiel.
        /// </summary>
        /// <param name="predicate">Le prédicat à vérifier.</param>
        /// <returns>Une valeur booléenne indiquant si une entité satisfaisant le prédicat existe.</returns>
        public Task<bool> Exists(Expression<Func<TEntity, bool>> predicate);
    }
}
