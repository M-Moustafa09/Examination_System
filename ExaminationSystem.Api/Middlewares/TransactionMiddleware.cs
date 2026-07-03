using ExaminationSystem.Infrastructure.Persistence;

namespace ExaminationSystem.Api.Middlewares;

public class TransactionMiddleware : IMiddleware
{
    private readonly ApplicationDbContext _context;

    public TransactionMiddleware(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
        var method = httpContext.Request.Method.ToUpper();

        if (method == "POST" || method == "PUT" || method == "DELETE")
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await next(httpContext);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        else
        {
            await next(httpContext);
        }
    }
}