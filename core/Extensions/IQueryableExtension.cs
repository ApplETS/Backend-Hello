using System.Linq.Expressions;

namespace api.core.Extensions;

public static class IQueryableExtension
{
    public static IQueryable<TEntity> OrderBy<TEntity>(
        this IQueryable<TEntity> source,
        string orderByProperty,
        bool desc)
    {
        string command = desc ? "OrderByDescending" : "OrderBy";
        var type = typeof(TEntity);

        // Split the property string to access nested properties
        var properties = orderByProperty.Split('.');
        var parameter = Expression.Parameter(type, "p");

        Expression propertyAccess = parameter;
        foreach (var propName in properties)
        {
            var property = propertyAccess.Type.GetProperty(propName);
            if (property == null)
            {
                throw new ArgumentException($"Property '{propName}' not found in type '{propertyAccess.Type.Name}'.");
            }
            propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
        }

        var orderByExpression = Expression.Lambda(propertyAccess, parameter);
        var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, propertyAccess.Type },
                                      source.Expression, Expression.Quote(orderByExpression));
        return source.Provider.CreateQuery<TEntity>(resultExpression);
    }
}
