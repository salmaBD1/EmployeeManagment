using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using employeeManagemement.Data;
using Microsoft.EntityFrameworkCore;

public class BookingStatusUpdater : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private Timer _timer;

    public BookingStatusUpdater(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(UpdateBookingStatus, null, TimeSpan.Zero, TimeSpan.FromHours(1)); // Change the interval as needed
        return Task.CompletedTask;
    }

    private void UpdateBookingStatus(object state)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Get bookings that have ended
            var endedBookings = context.Bookings
                .Where(b => b.BookingTo <= DateTime.Now)
                .Include(b => b.Salle)
                .ToList();

            foreach (var booking in endedBookings)
            {
                // Update salle status to "free"
                booking.Salle.Status = "free";
            }

            context.SaveChanges();
        }
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
