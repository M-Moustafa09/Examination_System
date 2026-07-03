using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Repositories;
using ExaminationSystem.Infrastructure.Persistence;

namespace ExaminationSystem.Infrastructure.Repositories;

public class QuizRepository(ApplicationDbContext context) : GeneralRepository<Quiz>(context), IQuizRepository
{
}