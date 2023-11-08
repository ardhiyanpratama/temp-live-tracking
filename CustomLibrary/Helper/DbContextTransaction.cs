using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomLibrary.Helper
{
    public interface ITransaction<T> where T : DbContext
    {
        Task Run(T context, CancellationToken cancellationToken = default);
    }

    public interface ITransaction<Td, Tr>
        where Td : DbContext
        where Tr : class
    {
        Task<Tr> Run(Td context, CancellationToken cancellationToken = default);
    }

    public static class DbContextTransactionExtensions
    {
        public static async Task DoTransactionAsync<T>(this T context, ITransaction<T> transaction, CancellationToken cancellationToken = default) where T : DbContext
        {
            try
            {
                var strategy = context.Database.CreateExecutionStrategy();
                await strategy.Execute(async () =>
                {
                    using var dbTransaction = await context.Database.BeginTransactionAsync(cancellationToken);
                    try
                    {
                        await transaction.Run(context, cancellationToken);
                        await dbTransaction.CommitAsync(cancellationToken);
                    }
                    catch (Exception)
                    {
                        await dbTransaction.RollbackAsync();
                        throw;
                    }
                    finally
                    {
                        await dbTransaction.DisposeAsync();
                    }
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<Tr> DoTransactionAsync<Td, Tr>(this Td context, ITransaction<Td, Tr> transaction, CancellationToken cancellationToken = default)
            where Td : DbContext
            where Tr : class
        {
            try
            {
                var strategy = context.Database.CreateExecutionStrategy();
                Tr result = default;
                await strategy.Execute(async () =>
                {
                    using var dbTransaction = await context.Database.BeginTransactionAsync(cancellationToken);
                    try
                    {
                        result = await transaction.Run(context, cancellationToken);
                        await dbTransaction.CommitAsync(cancellationToken);
                    }
                    catch (Exception)
                    {
                        await dbTransaction.RollbackAsync();
                        throw;
                    }
                    finally
                    {
                        await dbTransaction.DisposeAsync();
                    }
                });

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
