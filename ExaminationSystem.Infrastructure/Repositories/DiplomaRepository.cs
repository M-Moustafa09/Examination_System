using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Repositories;
using ExaminationSystem.Infrastructure.Persistence;

namespace ExaminationSystem.Infrastructure.Repositories;

public class DiplomaRepository(ApplicationDbContext _db) : GeneralRepository<Diploma>(_db), IDiplomaRepository
{
}