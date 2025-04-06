using EventPlanner.Domain.Entity;

namespace EventPlanner.Application.Interfaces;
public interface IEmailService
{
    public Task<bool> sendInvitaionEmail(string To, Event eventDetails);
}
