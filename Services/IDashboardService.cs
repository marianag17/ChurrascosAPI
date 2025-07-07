using ChurrascosAPI.Models;

namespace ChurrascosAPI.Services
{
    public interface IDashboardService
    {
        Task<DashboardData> GetDashboardDataAsync();
    }
}